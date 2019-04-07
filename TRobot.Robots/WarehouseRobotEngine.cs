using System;
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

        internal WarehouseRobotEngine(WarehouseRobot robot) : base()
        {
            this.robot = robot;

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
            engineThread.Abort();
        }

        private void calculateDrivesSpeed()
        {
            var robotVelocity = robot.Velocity;
            var robotAcceleration = robot.Acceleration;

            while (true)
            {
                var currentDriveXVelocity = DriveX.Velocity;
                var currentDriveYVelocity = DriveY.Velocity;
                
                var currentVector = robot.Controller.CurrentVector;

                //var XCoordinateVector = new Vector(currentVector.X, 0);
                //var YCoordinateVector = new Vector(0, currentVector.Y);
                //var xAngle = Vector.AngleBetween(currentVector, XCoordinateVector);
                //var yAngleDegrees = Vector.AngleBetween(currentVector, YCoordinateVector);
                
                var yAngleRadians = Math.Atan2(currentVector.Y, currentVector.X);

                //double yAngleRadians = Math.PI * yAngleDegrees / 180.0;

                var YDriveVelocity = robotVelocity * Math.Cos(yAngleRadians);
                var XDriveVelocity = robotVelocity * Math.Sin(yAngleRadians);

                if (YDriveVelocity > currentDriveYVelocity)
                {
                    DriveY.Velocity += robotAcceleration;
                }
                else
                {
                    DriveY.Velocity = YDriveVelocity;
                }

                if (XDriveVelocity > currentDriveXVelocity)
                {
                    DriveX.Velocity += robotAcceleration;
                }
                else
                {
                    DriveX.Velocity = XDriveVelocity;
                }

                //var resultingVelocityVector = Vector.Add(new Vector(DriveX.Velocity, 0), new Vector(0, DriveY.Velocity));
                var resultingVelocityVector = new Vector(DriveX.Velocity, DriveY.Velocity);                

                if (resultingVelocityVector.Length != robotVelocity)
                {
                    OnVelocityChanged(new VelocityChangedEventArguments(resultingVelocityVector.Length));
                }

                var resultingPosition = Vector.Add(resultingVelocityVector, robot.CurrentPosition);
                robot.CurrentPosition = resultingPosition;
                OnPositionChanged(new PositionChangedEventArguments(robot.CurrentPosition));

                Thread.Sleep(1000);
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
    }
}
