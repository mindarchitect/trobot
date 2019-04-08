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
        private int tick = (int)Math.Round(((double)1 / RefreshFactor) * 1000);

        public RobotDimensionalDrive DriveX { get; set; }
        public RobotDimensionalDrive DriveY { get; set; }

        public event EventHandler<VelocityChangedEventArguments> VelocityChanged;
        public event EventHandler<PositionChangedEventArguments> PositionChanged;

        private object pobotPropertyLock = new object();

        public WarehouseRobot Robot
        {
            get
            {
                lock (pobotPropertyLock)
                {
                    return robot;
                }
            }

            set
            {
                lock (pobotPropertyLock)
                {
                    robot = value;                    
                }
            }
        }

        internal WarehouseRobotEngine(WarehouseRobot robot) : base()
        {
            this.Robot = robot;

            DriveX = new RobotDimensionalDrive(Dimension.X);
            DriveY = new RobotDimensionalDrive(Dimension.Y);
        }

        public void Start()
        {
            var threadStart = new ThreadStart(calculateDrivesSpeed);
            engineThread = new Thread(threadStart);
            engineThread.Start();
        }

        public void Stop()
        {
            if (engineThread != null)
            {
                engineThread.Abort();
            }                      
        }

        private void calculateDrivesSpeed()
        {
            var robotVelocity = Robot.Velocity;
            var robotAcceleration = Robot.Acceleration;
            var trajectory = Robot.Controller.Trajectory;            

            Vector currentVector;            
            Point positionInCurrentVector;
            
            bool positionIsInCurrentVector = false;

            int i = 1;
            for (LinkedListNode<Vector> node = trajectory.First; node != null; )
            {
                currentVector = node.Value;
                
                positionInCurrentVector = new Point(0, 0);                
                positionIsInCurrentVector = (currentVector.Length - ((Vector)positionInCurrentVector).Length) > 0;

                var arctangRadians = Math.Atan2(currentVector.Y, currentVector.X);                

                var YDriveVelocity = (robotVelocity * Math.Sin(arctangRadians)) / RefreshFactor;
                var XDriveVelocity = (robotVelocity * Math.Cos(arctangRadians)) / RefreshFactor;

                var YDriveAcceleration = (robotAcceleration * Math.Sin(arctangRadians)) / RefreshFactor;
                var XDriveAcceleration = (robotAcceleration * Math.Cos(arctangRadians)) / RefreshFactor;                

                while (positionIsInCurrentVector)
                {
                    //Create separate therad for each drive + resources synchronization                    
                    //DriveY.Velocity = CalculateDriveVelocity(YDriveVelocity, YDriveAcceleration, DriveY.Velocity);
                    //DriveX.Velocity = CalculateDriveVelocity(XDriveVelocity, XDriveAcceleration, DriveX.Velocity);

                    DriveY.Velocity = YDriveVelocity;
                    DriveX.Velocity = XDriveVelocity;

                    var resultingVelocityVector = new Vector(DriveX.Velocity, DriveY.Velocity);                    
                    Robot.Velocity = resultingVelocityVector.Length;

                    if (resultingVelocityVector.Length != robotVelocity)
                    {
                        OnVelocityChanged(new VelocityChangedEventArguments(Robot.Velocity));
                    }                                      

                    Robot.CurrentPosition = Vector.Add(resultingVelocityVector, Robot.CurrentPosition);
                    OnPositionChanged(new PositionChangedEventArguments(Robot.CurrentPosition));

                    //if (DriveY.Velocity < YDriveVelocity && DriveX.Velocity < XDriveVelocity)
                    if (Math.Abs(DriveY.Velocity) < Math.Abs(YDriveVelocity) || Math.Abs(DriveX.Velocity) < Math.Abs(XDriveVelocity))
                    {
                        var resultingAccelerationVector = new Vector(XDriveAcceleration, YDriveAcceleration);
                        Robot.CurrentPosition = Vector.Add(resultingAccelerationVector, Robot.CurrentPosition);
                        OnPositionChanged(new PositionChangedEventArguments(Robot.CurrentPosition));

                        DriveY.Velocity += YDriveAcceleration;
                        DriveX.Velocity += XDriveAcceleration;

                    }

                    positionInCurrentVector = Vector.Add(resultingVelocityVector, positionInCurrentVector);
                    positionIsInCurrentVector = (currentVector.Length - ((Vector)positionInCurrentVector).Length) > 0;

                    Thread.Sleep(tick);
                }
                
                node = node.Next;                
            }            
        }

        public void Pause()
        {
            throw new NotImplementedException();
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
            }

            if (Math.Sign(driveVelocity) == -1 && Math.Sign(driveAcceleration) == -1)
            {
                return absCurrentDriveVelocity * (-1);
            }

            return absCurrentDriveVelocity;
        }        
    }
}
