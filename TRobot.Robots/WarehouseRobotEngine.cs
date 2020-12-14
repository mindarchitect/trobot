using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using TRobot.Core;
using TRobot.Core.Enums;
using TRobot.Core.Robot.Events;
using System.Threading.Tasks;

namespace TRobot.Robots
{
    internal class WarehouseRobotEngine: RobotEngine, IControllable
    {
        private WarehouseRobot robot;        

        private static readonly int RefreshFactor = 60;
        private static readonly int Tick = (int)Math.Round(((double)1 / RefreshFactor) * 1000);

        private RobotDimensionalDrive DriveX { get; set; }
        private RobotDimensionalDrive DriveY { get; set; }

        public event EventHandler<VelocityChangedEventArguments> VelocityChanged;
        public event EventHandler<PositionChangedEventArguments> PositionChanged;

        private object robotPropertyLock = new object();
        private object accelerationPropertyLock = new object();
        private bool accelerating = true;

        // Synchronisation primitive to control engine task execution thread        
        private ManualResetEvent engineTaskControllingEvent;
        //Cancelation token for engine task thread
        private CancellationTokenSource cancellationTokenSource;
        private Task engineTask;

        private bool Accelerating
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

        private WarehouseRobot Robot
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

            Drives.Add(DriveX);
            Drives.Add(DriveY);

            engineTaskControllingEvent = new ManualResetEvent(false);
        }

        public void Start()
        {
            Accelerating = true;
            engineTaskControllingEvent.Set();

            if (engineTask?.IsCompleted ?? true)
            {
                TaskStatus? engineTaskStatus = engineTask?.Status;
                
                // This condition is required to prevent task from running second time without explicit reset
                // wich sets task to null
                if (!engineTaskStatus.HasValue || engineTaskStatus.Value != TaskStatus.RanToCompletion)
                {
                    cancellationTokenSource = new CancellationTokenSource();
                    engineTask = Task.Factory.StartNew(SimulateRobotMovement, cancellationTokenSource.Token);

                    // Post task action
                    // engineTask.ContinueWith((antecedentTask) => 
                    //{                        
                    //});
                }               
            }            
        }

        public void Stop()
        {            
            Accelerating = false;            
        }

        public void Reset()
        {                        
            if (engineTask?.IsCompleted ?? true)
            {                
                ResetRobotDrives();                               
            }
            else
            {
                cancellationTokenSource.Cancel();                                
            }

            engineTask = null;
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
            var robotVelocity = Robot.Settings.Velocity;
            var robotAcceleration = Robot.Settings.Acceleration;
            var trajectory = Robot.Controller.Trajectory;

            var currentVector = new Vector();
            var resultingVelocityVector = new Vector();
            var positionInCurrentVector = new Point(0, 0);

            bool positionIsInCurrentVector = false;

            double xDriveVelocity = 0;
            double yDriveVelocity = 0;

            double xDriveAcceleration = 0;
            double yDriveAcceleration = 0;            
             
            for (LinkedListNode<Vector> node = trajectory.First; node != null;)
            {
                currentVector = node.Value;

                positionInCurrentVector = new Point(0, 0);
                positionIsInCurrentVector = (currentVector.Length - ((Vector)positionInCurrentVector).Length) > 0;

                var arctangRadians = Math.Atan2(currentVector.Y, currentVector.X);

                var x = Math.Cos(arctangRadians);
                var y = Math.Sin(arctangRadians);

                xDriveVelocity = (robotVelocity * x) / RefreshFactor;
                yDriveVelocity = (robotVelocity * y) / RefreshFactor;

                xDriveAcceleration = (robotAcceleration * x) / RefreshFactor;
                yDriveAcceleration = (robotAcceleration * y) / RefreshFactor;

                // Stop each drive before changing vector
                DriveX.Velocity = 0;
                DriveY.Velocity = 0;                

                while (positionIsInCurrentVector)
                {
                    engineTaskControllingEvent.WaitOne();
                    if (cancellationTokenSource.Token.IsCancellationRequested)
                    {
                        ResetRobotDrives();
                        return;
                    }

                    //Create separate thread for each drive + resources synchronization
                    resultingVelocityVector = UpdateRobotDrives(xDriveVelocity, yDriveVelocity, xDriveAcceleration, yDriveAcceleration);
                    positionInCurrentVector = Vector.Add(resultingVelocityVector, positionInCurrentVector);

                    positionIsInCurrentVector = (currentVector.Length - ((Vector)positionInCurrentVector).Length) > 0;
                    if (!positionIsInCurrentVector)
                    {
                        continue;
                    }

                    var newPosition = Vector.Add(resultingVelocityVector, Robot.CurrentPosition);
                    UpdateRobotCurrentPosition(newPosition);

                    Thread.Sleep(Tick);
                }

                node = node.Next;
            }

            Accelerating = false;
            while (resultingVelocityVector.Length != 0)
            {
                engineTaskControllingEvent.WaitOne();
                if (cancellationTokenSource.Token.IsCancellationRequested)
                {
                    ResetRobotDrives();
                    return;
                }

                resultingVelocityVector = UpdateRobotDrives(xDriveVelocity, yDriveVelocity, xDriveAcceleration, yDriveAcceleration);                

                var newPosition = Vector.Add(resultingVelocityVector, Robot.CurrentPosition);
                UpdateRobotCurrentPosition(newPosition);

                Thread.Sleep(Tick);
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

        private void UpdateRobotCurrentVelocity(Vector resultingVelocityVector)
        {            
            var resultingVelocity = resultingVelocityVector.Length;

            if (resultingVelocity != Robot.CurrentVelocity)
            {
                Robot.CurrentVelocity = resultingVelocity;
                OnVelocityChanged(new VelocityChangedEventArguments(Robot.CurrentVelocity));
            }
        }
        private void UpdateRobotCurrentPosition(Point currentPosition)
        {            
            if (currentPosition != Robot.CurrentPosition)
            {
                Robot.CurrentPosition = currentPosition;
                OnPositionChanged(new PositionChangedEventArguments(Robot.CurrentPosition));
            }
        }

        private Vector UpdateRobotDrives(double xDriveVelocity, double yDriveVelocity, double xDriveAcceleration, double yDriveAcceleration)
        {
            DriveX.Velocity = CalculateDriveVelocity(xDriveVelocity, xDriveAcceleration, DriveX.Velocity);
            DriveY.Velocity = CalculateDriveVelocity(yDriveVelocity, yDriveAcceleration, DriveY.Velocity);

            var resultingVelocityVector = new Vector(DriveX.Velocity, DriveY.Velocity);
            UpdateRobotCurrentVelocity(resultingVelocityVector);

            return resultingVelocityVector;
        }

        private void ResetRobotDrives()
        {
            DriveX.Velocity = 0;
            DriveY.Velocity = 0;

            var resultingVelocityVector = new Vector(DriveX.Velocity, DriveY.Velocity);
            UpdateRobotCurrentVelocity(resultingVelocityVector);

            // In monitoring unit, current position property defines an offset to start position
            // Sending Point(0, 0) resets current position offset
            UpdateRobotCurrentPosition(new Point(0, 0));            
        }
    }
}
