using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows;
using TRobot.Communication.Services.Trajectory;
using TRobot.Core;
using TRobot.Core.Enums;
using TRobot.Core.Robot.Events;
using TRobot.ECU.Models;
using TRobot.Robots.Services;

namespace TRobot.Robots
{
    internal class WarehouseRobotController: IControllable
    {
        public event EventHandler<TrajectoryValidatedEventArguments> TrajectoryValidated;
        public event EventHandler<EventArgs> MonitoringServiceClientInnerChannelStateChanged;

        internal IList<DescartesCoordinatesItem> Coordinates { get; set; }
        internal LinkedList<Vector> Trajectory { get; set; }

        public RobotState State { get; set; } = RobotState.Reset;

        private WarehouseRobot robot;
        private WarehouseRobotTrajectoryValidationServiceClient warehouseRobotTrajectoryValidationServiceClient;
        private WarehouseRobotMonitoringSeviceClient warehouseRobotMonitoringSeviceClient;        

        internal WarehouseRobotController(WarehouseRobot robot)
        {
            this.robot = robot;

            this.robot.Engine.VelocityChanged += OnEngineVelocityChanged;
            this.robot.Engine.PositionChanged += OnEnginePositionChanged;
        }

        public void Initialize()
        {
            var warehouseRobotTrajectoryValidationServiceCallback = new WarehouseRobotTrajectoryValidationServiceCallback(TrajectoryValidatedCallback);            
            warehouseRobotTrajectoryValidationServiceClient = new WarehouseRobotTrajectoryValidationServiceClient(warehouseRobotTrajectoryValidationServiceCallback, new NetNamedPipeBinding(), new EndpointAddress("net.pipe://localhost/validation/ValidationService"));            

            var warehouseRobotMonitoringServiceCallback = new WarehouseRobotMonitoringServiceCallback(TrajectorySetupCallback, TrajectoryUpdatedCallback, TestOperationCallback);
            warehouseRobotMonitoringSeviceClient = new WarehouseRobotMonitoringSeviceClient(warehouseRobotMonitoringServiceCallback, new NetNamedPipeBinding(), new EndpointAddress("net.pipe://localhost/monitoring/MonitoringService"));

            // TODO
            // Investigate Reset button issue when event handlers are activated

            /*warehouseRobotMonitoringSeviceClient.InnerChannel.Opening += MonitorServiceInnerChannelStateChanged;
            warehouseRobotMonitoringSeviceClient.InnerChannel.Opened += MonitorServiceInnerChannelStateChanged;
            warehouseRobotMonitoringSeviceClient.InnerChannel.Closing += MonitorServiceInnerChannelStateChanged;
            warehouseRobotMonitoringSeviceClient.InnerChannel.Closed += MonitorServiceInnerChannelStateChanged;
            warehouseRobotMonitoringSeviceClient.InnerChannel.Faulted += MonitorServiceInnerChannelStateChanged;*/
        }

        public void Terminate()
        {
            try
            {
                warehouseRobotTrajectoryValidationServiceClient?.Close();
                warehouseRobotMonitoringSeviceClient?.Close();                
            }
            catch (CommunicationException e)
            {
                warehouseRobotTrajectoryValidationServiceClient.Abort();
                warehouseRobotMonitoringSeviceClient.Abort();
            }
            catch (TimeoutException e)
            {
                warehouseRobotTrajectoryValidationServiceClient.Abort();
                warehouseRobotMonitoringSeviceClient.Abort();
            }
            catch (Exception e)
            {
                warehouseRobotTrajectoryValidationServiceClient.Abort();
                warehouseRobotMonitoringSeviceClient.Abort();
                throw;
            }            
        }        

        internal async void UploadTrajectory(IList<DescartesCoordinatesItem> coordinates)
        {
            if (coordinates.Count < 2)
            {
                throw new Exception("Trajectory coordinates should contain at least 2 coordinates");
            }

            Coordinates = coordinates;

            await Task.Run(() => ValidateTrajectory());
        }

        internal CommunicationState GetWarehouseRobotMonitoringSeviceConnectionState()
        {
            return warehouseRobotMonitoringSeviceClient.State;
        }

        private async void SetupTrajectory()
        {
            await Task.Run(() => BuildTrajectory());
            await Task.Run(() => SetupTrajectoryMonitoring());
        }

        public void Start()
        {
            robot.Engine.Start();
            State = RobotState.Started;
        }

        public void Stop()
        {
            robot.Engine.Stop();
            State = RobotState.Stopped;
        }

        public void Reset()
        {
            robot.Engine.Reset();            
            warehouseRobotMonitoringSeviceClient.UpdateRobotPosition(robot.Id, new Point(0,0));

            State = RobotState.Reset;
        }

        private void BuildTrajectory()
        {            
            Trajectory = new LinkedList<Vector>();

            LinkedListNode<Vector> currentNode = null;

            for (var i = 0; i < Coordinates.Count; i++)
            {
                try
                {
                    var vector = GetTrajectoryVectors(Coordinates[i].Point, Coordinates[i + 1].Point);                   

                    if (i == 0)
                    {
                        currentNode = new LinkedListNode<Vector>(vector);
                        Trajectory.AddFirst(currentNode);
                    }
                    else
                    {
                        currentNode = Trajectory.AddAfter(currentNode, vector);
                    }
                }
                catch (ArgumentOutOfRangeException)
                {
                }                
            }                      
        }               

        private void ValidateTrajectory()
        {
            List<Point> trajectoryPoints = new List<DescartesCoordinatesItem>(Coordinates).ConvertAll(item => new Point
            {
                X = item.Point.X,
                Y = item.Point.Y
            });

            warehouseRobotTrajectoryValidationServiceClient.ValidateTrajectory(robot.Id, trajectoryPoints);
        }

        private void SetupTrajectoryMonitoring()
        {
            List<Point> trajectoryPoints = new List<DescartesCoordinatesItem>(Coordinates).ConvertAll(item => new Point
            {
                X = item.Point.X,
                Y = item.Point.Y
            });

            warehouseRobotMonitoringSeviceClient.SetupRobotTrajectory(robot.Id, robot.Title, trajectoryPoints);
        }

        private Vector GetTrajectoryVectors(Point start, Point end)
        {
            return Point.Subtract(end, start);
        }

        private void TrajectoryValidatedCallback(RobotValidationResult robotValidationResult)
        {
            OnTrajectoryValidated(new TrajectoryValidatedEventArguments(robotValidationResult.RobotId, robotValidationResult.ValidationResult, robotValidationResult.ValidationMessage));

            if (robotValidationResult.ValidationResult)
            {
                SetupTrajectory();
            }
        }        

        private void OnTrajectoryValidated(TrajectoryValidatedEventArguments e)
        {
            TrajectoryValidated?.Invoke(this, e);
        }        

        private void OnEnginePositionChanged(object sender, PositionChangedEventArguments e)
        {
            warehouseRobotMonitoringSeviceClient.UpdateRobotPosition(robot.Id, e.NewPosition);
        }

        private void OnEngineVelocityChanged(object sender, VelocityChangedEventArguments e)
        {
        }
        private void MonitorServiceInnerChannelStateChanged(object sender, EventArgs e)
        {
            MonitoringServiceClientInnerChannelStateChanged?.Invoke(this, e);
        }
        private void TrajectorySetupCallback()
        {
        }

        private void TrajectoryUpdatedCallback()
        {
        }

        private void TestOperationCallback()
        {
        }
    }
}
