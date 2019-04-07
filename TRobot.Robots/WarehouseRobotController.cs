using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using TRobot.Communication.Services.Trajectory;
using TRobot.Core;
using TRobot.Core.Robot.Events;
using TRobot.ECU.Models;
using TRobot.Robots.Services;

namespace TRobot.Robots
{    
    internal class WarehouseRobotController: IControllable
    {
        private WarehouseRobot robot;
        private IList<DescartesCoordinatesItem> coordinates;
        private LinkedList<Vector> trajectory;
        public Vector CurrentVector { get; set; }        

        private WarehouseRobotTrajectoryValidationServiceClient warehouseRobotTrajectoryValidationServiceClient;
        private WarehouseRobotMonitoringSeviceClient warehouseRobotMonitoringSeviceClient;

        public event EventHandler<TrajectoryValidatedEventArguments> TrajectoryValidated;    
        
        internal WarehouseRobotController(WarehouseRobot robot)
        {
            this.robot = robot;

            var warehouseRobotTrajectoryValidationServiceCallback = new WarehouseRobotTrajectoryValidationServiceCallback(TrajectoryValidatedCallback);
            warehouseRobotTrajectoryValidationServiceClient = new WarehouseRobotTrajectoryValidationServiceClient(warehouseRobotTrajectoryValidationServiceCallback, new NetNamedPipeBinding(), new EndpointAddress("net.pipe://localhost/validation/ValidationService"));            

            var warehouseRobotMonitoringServiceCallback = new WarehouseRobotMonitoringServiceCallback(TrajectorySetupCallback, TrajectoryUpdatedCallback);
            warehouseRobotMonitoringSeviceClient = new WarehouseRobotMonitoringSeviceClient(warehouseRobotMonitoringServiceCallback, new NetNamedPipeBinding(), new EndpointAddress("net.pipe://localhost/monitoring/MonitoringService"));

            this.robot.Engine.VelocityChanged += OnEngineVelocityChanged;
            this.robot.Engine.PositionChanged += OnEnginePositionChanged;
        }

        private void OnEnginePositionChanged(object sender, PositionChangedEventArguments e)
        {
            warehouseRobotMonitoringSeviceClient.UpdatePosition(robot.Id, e.NewPosition);
        }

        private void OnEngineVelocityChanged(object sender, VelocityChangedEventArguments e)
        {            
        }

        internal async void SetupTrajectory()
        {
            await Task.Run(() => BuildTrajectory());
            await Task.Run(() => SetupTrajectoryMonitoring());            
        }

        internal async void UploadTrajectory(IList<DescartesCoordinatesItem> coordinates)
        {
            if (coordinates.Count < 2)
            {
                throw new Exception("Trajectory coordinates should containe at least 2 coordinates");
            }

            this.coordinates = coordinates;

            await Task.Run(() => ValidateTrajectory());

            //Testing         
            await Task.Run(() => BuildTrajectory());
        }

        public void Start()
        {
            robot.Engine.Start();
        }

        public void Stop()
        {         
            robot.Engine.Stop();
        }

        public void Pause()
        {
            throw new NotImplementedException();
        }

        internal void BuildTrajectory()
        {            
            trajectory = new LinkedList<Vector>();

            LinkedListNode<Vector> currentNode = null;

            for (var i = 0; i < coordinates.Count; i++)
            {
                try
                {
                    var vector = GetTrajectoryVectors(coordinates[i].Point, coordinates[i + 1].Point);                   

                    if (i == 0)
                    {
                        currentNode = new LinkedListNode<Vector>(vector);
                        trajectory.AddFirst(currentNode);
                        CurrentVector = vector;
                    }
                    else
                    {
                        currentNode = trajectory.AddAfter(currentNode, vector);
                    }
                }
                catch (ArgumentOutOfRangeException)
                {
                }                
            }                      
        }  

        protected virtual void OnTrajectoryValidated(TrajectoryValidatedEventArguments e)
        {            
            TrajectoryValidated?.Invoke(this, e);
        }        

        private void ValidateTrajectory()
        {
            List<Point> trajectoryPoints = new List<DescartesCoordinatesItem>(coordinates).ConvertAll(item => new Point
            {
                X = item.Point.X,
                Y = item.Point.Y
            });

            warehouseRobotTrajectoryValidationServiceClient.ValidateTrajectory(robot.Id, trajectoryPoints);
        }

        private void SetupTrajectoryMonitoring()
        {
            List<Point> trajectoryPoints = new List<DescartesCoordinatesItem>(coordinates).ConvertAll(item => new Point
            {
                X = item.Point.X,
                Y = item.Point.Y
            });

            warehouseRobotMonitoringSeviceClient.SetupTrajectory(robot.Id, robot.Title, trajectoryPoints);
        }

        private Vector GetTrajectoryVectors(Point start, Point end)
        {
            return Point.Subtract(end, start);
        }

        private void TrajectoryValidatedCallback(RobotValidationResult robotValidationResult)
        {
            OnTrajectoryValidated(new TrajectoryValidatedEventArguments(robotValidationResult.RobotId, robotValidationResult.ValidationResult, robotValidationResult.ValidationMessage));
        }

        private void TrajectorySetupCallback()
        {
        }

        private void TrajectoryUpdatedCallback()
        {
        }               
    }
}
