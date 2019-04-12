using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using TRobot.Core;
using TRobot.Core.Enums;
using TRobot.Core.Robot.Events;
using System.Linq;

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
        private object accelerationPropertyLock = new object();
        private bool accelerating = true;

        private ManualResetEvent engineThreadControllingEvent = new ManualResetEvent(false);        

        public bool Accelerating
        {
            get
            {
                lock (accelerationPropertyLock)
                {
                    return accelerating;
                }
            }

            set
            {
                lock (accelerationPropertyLock)
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

        public void Reset()
        {
            engineThread.Abort();
            DriveX.Velocity = 0;
            DriveY.Velocity = 0;

            Robot.CurrentPosition = robot.Controller.Coordinates.First().Point;
            OnPositionChanged(new PositionChangedEventArguments(Robot.CurrentPosition));

        }

        public void Resume()
        {
            engineThreadControllingEvent.Set();
        }

        public void Stop()
        {
            Accelerating = false;
        }              

        protected virtual void OnVelocityChanged(VelocityChangedEventArguments e)
        {
            VelocityChanged?.Invoke(this, e);
        }

        protected virtual void OnPositionChanged(PositionChangedEventArguments e)
        {
            PositionChanged?.Invoke(this, e);
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

            for (LinkedListNode<Vector> node = trajectory.First; node != null;)
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

                    DriveY.Velocity = CalculateDriveVelocity(YDriveVelocity, YDriveAcceleration, DriveY.Velocity);
                    DriveX.Velocity = CalculateDriveVelocity(XDriveVelocity, XDriveAcceleration, DriveX.Velocity);

                    resultingVelocityVector = new Vector(DriveX.Velocity, DriveY.Velocity);
                    UpdateRobotVelocity(resultingVelocityVector);

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

            Accelerating = false;

            while (resultingVelocityVector.Length != 0)
            {
                DriveX.Velocity = CalculateDriveVelocity(XDriveVelocity, XDriveAcceleration, DriveX.Velocity);
                DriveY.Velocity = CalculateDriveVelocity(YDriveVelocity, YDriveAcceleration, DriveY.Velocity);

                resultingVelocityVector = new Vector(DriveX.Velocity, DriveY.Velocity);
                UpdateRobotVelocity(resultingVelocityVector);

                Robot.CurrentPosition = Vector.Add(resultingVelocityVector, Robot.CurrentPosition);
                OnPositionChanged(new PositionChangedEventArguments(Robot.CurrentPosition));

                Thread.Sleep(tick);
            }
        }

        private double CalculateDriveVelocity(double driveVelocity, double driveAcceleration, double currentDriveVelocity)
        {
            double resultingDriveVelocity;
                      
            if (Accelerating)
            {
                resultingDriveVelocity = IncrementAcceleration(Math.Abs(driveVelocity), Math.Abs(driveAcceleration), Math.Abs(currentDriveVelocity));
            }
            else
            {
                resultingDriveVelocity = DecrementAcceleration(Math.Abs(driveAcceleration), Math.Abs(currentDriveVelocity));
            }            

            if (Math.Sign(driveVelocity) == -1 && Math.Sign(driveAcceleration) == -1)
            {
                return resultingDriveVelocity * (-1);
            }

            return resultingDriveVelocity;
        }       

        private double IncrementAcceleration(double driveVelocity, double driveAcceleration, double currentDriveVelocity)
        {
            if (currentDriveVelocity < driveVelocity)
            {
                currentDriveVelocity += driveAcceleration;

                if (currentDriveVelocity > driveVelocity)
                {
                    currentDriveVelocity = driveVelocity;
                }
            }

            if (currentDriveVelocity > driveVelocity)
            {
                currentDriveVelocity -= driveAcceleration;

                if (currentDriveVelocity < driveVelocity)
                {
                    currentDriveVelocity = driveVelocity;
                }
            }

            return currentDriveVelocity;
        }

        private double DecrementAcceleration(double driveAcceleration, double currentDriveVelocity)
        {
            if (currentDriveVelocity > 0)
            {
                currentDriveVelocity -= driveAcceleration;

                if (currentDriveVelocity < 0)
                {
                    currentDriveVelocity = 0;
                }
            }

            return currentDriveVelocity;
        }

        private void UpdateRobotVelocity(Vector resultingVelocityVector)
        {       
            if (resultingVelocityVector.Length != Robot.Velocity)
            {
                Robot.Velocity = resultingVelocityVector.Length;
                OnVelocityChanged(new VelocityChangedEventArguments(Robot.Velocity));
            }
        }        
    }
}
