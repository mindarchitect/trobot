using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using TRobot.Core;
using TRobot.Core.Enums;
using TRobot.Core.Robot.Events;

namespace TRobot.Robots
{
    internal class WarehouseRobotEngine: IControllable
    {
        private WarehouseRobot robot;
        private Thread engineThread;

        private static int RefreshFactor = 60;
        private readonly int tick = (int)Math.Round(((double)1 / RefreshFactor) * 1000);

        public RobotDimensionalDrive DriveX { get; set; }
        public RobotDimensionalDrive DriveY { get; set; }

        public event EventHandler<VelocityChangedEventArguments> VelocityChanged;
        public event EventHandler<PositionChangedEventArguments> PositionChanged;

        private object robotPropertyLock = new object();
        private object accelerationFlagLock = new object();

        private ManualResetEvent engineThreadControllingEvent = new ManualResetEvent(false);

        private bool accelerating = true;

        public bool Accelerating
        {
            get
            {
                lock (accelerationFlagLock)
                {
                    return accelerating;
                }
            }

            set
            {
                lock (accelerationFlagLock)
                {
                    accelerating = value;
                }
            }
        }

        public WarehouseRobot Robot
        {
            get
            {
                lock (robotPropertyLock)
                {
                    return robot;
                }
            }

            set
            {
                lock (robotPropertyLock)
                {
                    robot = value;                    
                }
            }
        }

        internal WarehouseRobotEngine(WarehouseRobot robot) : base()
        {
            Robot = robot;

            DriveX = new RobotDimensionalDrive(Dimension.X);
            DriveY = new RobotDimensionalDrive(Dimension.Y);

            var threadStart = new ThreadStart(SimulateRobotMovement);
            engineThread = new Thread(threadStart);            
        }

        public void Start()
        {
            Accelerating = true;
            engineThreadControllingEvent.Set();
            if (!engineThread.IsAlive)
            {
                engineThread.Start();
            }            
        }

        public void Stop()
        {
            //if (engineThread != null)
            //{
            //    engineThread.Abort();
            //}                      

            Accelerating = false;
        }

        public void Resume()
        {
            //engineThreadControllingEvent.Set();
        }

        public void Pause()
        {
            //engineThreadControllingEvent.Reset();
        }

        private void SimulateRobotMovement()
        {
            var robotVelocity = Robot.Velocity;
            var robotAcceleration = Robot.Acceleration;
            var trajectory = Robot.Controller.Trajectory;            

            Vector currentVector = new Vector();
            Vector resultingVelocityVector = new Vector();
            Point positionInCurrentVector = new Point(0, 0);
            
            bool positionIsInCurrentVector = false;

            double YDriveVelocity = 0;
            double XDriveVelocity = 0;

            double YDriveAcceleration = 0;
            double XDriveAcceleration = 0;

            for (LinkedListNode<Vector> node = trajectory.First; node != null; )
            {
                currentVector = node.Value;
                
                positionInCurrentVector = new Point(0, 0);                
                positionIsInCurrentVector = (currentVector.Length - ((Vector)positionInCurrentVector).Length) > 0;

                var arctangRadians = Math.Atan2(currentVector.Y, currentVector.X);                

                YDriveVelocity = (robotVelocity * Math.Sin(arctangRadians)) / RefreshFactor;
                XDriveVelocity = (robotVelocity * Math.Cos(arctangRadians)) / RefreshFactor;

                YDriveAcceleration = (robotAcceleration * Math.Sin(arctangRadians)) / RefreshFactor;
                XDriveAcceleration = (robotAcceleration * Math.Cos(arctangRadians)) / RefreshFactor;

                while (positionIsInCurrentVector)
                {
                    engineThreadControllingEvent.WaitOne();

                    //Create separate therad for each drive + resources synchronization                    

                    if (Accelerating)
                    {
                        DriveY.Velocity = CalculateDriveVelocity(YDriveVelocity, YDriveAcceleration, DriveY.Velocity);
                        DriveX.Velocity = CalculateDriveVelocity(XDriveVelocity, XDriveAcceleration, DriveX.Velocity);
                    }
                    else
                    {
                        DriveX.Velocity = CalculateDriveVelocity2(XDriveVelocity, XDriveAcceleration, DriveX.Velocity);
                        DriveY.Velocity = CalculateDriveVelocity2(YDriveVelocity, YDriveAcceleration, DriveY.Velocity);                        
                    }                    

                    resultingVelocityVector = new Vector(DriveX.Velocity, DriveY.Velocity);                    

                    if (resultingVelocityVector.Length != Robot.Velocity)
                    {
                        Robot.Velocity = resultingVelocityVector.Length;
                        OnVelocityChanged(new VelocityChangedEventArguments(Robot.Velocity));
                    }

                    positionInCurrentVector = Vector.Add(resultingVelocityVector, positionInCurrentVector);

                    positionIsInCurrentVector = (currentVector.Length - ((Vector)positionInCurrentVector).Length) > 0;
                    if (!positionIsInCurrentVector)
                    {
                        continue;
                    }

                    Robot.CurrentPosition = Vector.Add(resultingVelocityVector, Robot.CurrentPosition);
                    OnPositionChanged(new PositionChangedEventArguments(Robot.CurrentPosition));

                    Thread.Sleep(tick);
                }

                node = node.Next;                
            } 
            
            while (resultingVelocityVector.Length != 0)
            {                
                DriveX.Velocity = CalculateDriveVelocity2(XDriveVelocity, XDriveAcceleration, DriveX.Velocity);
                DriveY.Velocity = CalculateDriveVelocity2(YDriveVelocity, YDriveAcceleration, DriveY.Velocity);                

                resultingVelocityVector = new Vector(DriveX.Velocity, DriveY.Velocity);

                if (resultingVelocityVector.Length != Robot.Velocity)
                {
                    Robot.Velocity = resultingVelocityVector.Length;
                    OnVelocityChanged(new VelocityChangedEventArguments(Robot.Velocity));
                }               

                Robot.CurrentPosition = Vector.Add(resultingVelocityVector, Robot.CurrentPosition);
                OnPositionChanged(new PositionChangedEventArguments(Robot.CurrentPosition));

                Thread.Sleep(tick);
            }
            
        }       

        protected virtual void OnVelocityChanged(VelocityChangedEventArguments e)
        {
            VelocityChanged?.Invoke(this, e);
        }

        protected virtual void OnPositionChanged(PositionChangedEventArguments e)
        {
            PositionChanged?.Invoke(this, e);
        }

        private double CalculateDriveVelocity(double driveVelocity, double driveAcceleration, double currentDriveVelocity)
        {
            double absDriveVelocity = Math.Abs(driveVelocity);
            double absCurrentDriveVelocity = Math.Abs(currentDriveVelocity);
            double absDriveAcceleration = Math.Abs(driveAcceleration);

            if (absCurrentDriveVelocity < absDriveVelocity)
            {
                absCurrentDriveVelocity += absDriveAcceleration;

                if (absCurrentDriveVelocity > absDriveVelocity)
                {
                    absCurrentDriveVelocity = absDriveVelocity;
                }
            }

            if (absCurrentDriveVelocity > absDriveVelocity)
            {
                absCurrentDriveVelocity -= absDriveAcceleration;

                if (absCurrentDriveVelocity < absDriveVelocity)
                {
                    absCurrentDriveVelocity = absDriveVelocity;
                }
            }

            if (Math.Sign(driveVelocity) == -1 && Math.Sign(driveAcceleration) == -1)
            {
                return absCurrentDriveVelocity * (-1);
            }

            return absCurrentDriveVelocity;
        }

        private double CalculateDriveVelocity2(double driveVelocity, double driveAcceleration, double currentDriveVelocity)
        {
            double absDriveVelocity = Math.Abs(driveVelocity);
            double absCurrentDriveVelocity = Math.Abs(currentDriveVelocity);
            double absDriveAcceleration = Math.Abs(driveAcceleration);

            if (absCurrentDriveVelocity > 0)
            {
                absCurrentDriveVelocity -= absDriveAcceleration;

                if (absCurrentDriveVelocity < 0)
                {
                    absCurrentDriveVelocity = 0;
                }
            }            

            if (Math.Sign(driveVelocity) == -1 && Math.Sign(driveAcceleration) == -1)
            {
                return absCurrentDriveVelocity * (-1);
            }

            return absCurrentDriveVelocity;
        }
    }
}
