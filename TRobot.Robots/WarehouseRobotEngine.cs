﻿using System;
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
            var trajectory = robot.Controller.Trajectory;

            Vector currentVector;
            bool condition = true;

            for (LinkedListNode<Vector> node = trajectory.First; node != null; )
            {
                currentVector = node.Value;
                
                while (condition)
                {                    
                    var arctangRadians = Math.Atan2(currentVector.Y, currentVector.X);

                    var YDriveVelocity = robotVelocity * Math.Cos(arctangRadians);
                    var XDriveVelocity = robotVelocity * Math.Sin(arctangRadians);                                        

                    //Create separate therad for each drive + resources synchronization
                    DriveY.Velocity = CalculateDriveVelocity(YDriveVelocity, DriveY.Velocity);
                    DriveX.Velocity = CalculateDriveVelocity(XDriveVelocity, DriveX.Velocity);                    

                    var resultingVelocityVector = new Vector(DriveX.Velocity, DriveY.Velocity);
                    robot.Velocity = resultingVelocityVector.Length;

                    if (resultingVelocityVector.Length != robotVelocity)
                    {
                        OnVelocityChanged(new VelocityChangedEventArguments(robot.Velocity));
                    }

                    var resultingPosition = Vector.Add(resultingVelocityVector, robot.CurrentPosition);
                    robot.CurrentPosition = resultingPosition;
                    OnPositionChanged(new PositionChangedEventArguments(robot.CurrentPosition));

                    Thread.Sleep(tick);
                }
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

        private double CalculateDriveVelocity(double driveVelocity, double currentDriveVelocity)
        {
            var robotAcceleration = robot.Acceleration;

            if (driveVelocity > currentDriveVelocity)
            {
                currentDriveVelocity += robotAcceleration / RefreshFactor;
            }
            else
            {
                currentDriveVelocity = driveVelocity / RefreshFactor;
            }

            return currentDriveVelocity;
        }
    }
}
