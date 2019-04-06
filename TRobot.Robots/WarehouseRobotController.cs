using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using TRobot.Communication.Trajectory;
using TRobot.Core;
using TRobot.ECU.Models;
using TRobot.Robots.Services;

namespace TRobot.Robots
{    
    internal class WarehouseRobotController
    {
        private WarehouseRobot robot;
        private IList<DescartesCoordinatesItem> coordinates;
        private LinkedList<Vector> trajectory;
        //private Thread workerProcessingThread;
        private WarehouseRobotTrajectoryValidationServiceClient warehouseRobotTrajectoryValidationServiceClient;

        public event EventHandler<TrajectoryValidatedEventArguments> TrajectoryValidated;
        internal WarehouseRobotController(WarehouseRobot robot)
        {
            this.robot = robot;

            warehouseRobotTrajectoryValidationServiceClient = new WarehouseRobotTrajectoryValidationServiceClient(TrajectoryValidatedCallback);
        }

        internal async void UploadTrajectory(IList<DescartesCoordinatesItem> coordinates)
        {
            if (coordinates.Count < 2)
            {
                throw new Exception("Trajectory coordinates should containe at least 2 coordinates");
            }

            this.coordinates = coordinates;

            await Task.Run(() => ValidateTrajectory());
            await Task.Run(() => BuildTrajectory());
        }

        public void Start()
        {
            //ThreadStart threadDelegate = new ThreadStart(TestMethod);
            //workerProcessingThread = new Thread(threadDelegate);
            //workerProcessingThread.Start();
        }

        private void BuildTrajectory()
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
        
        private Vector GetTrajectoryVectors(Point start, Point end)
        {
            return Point.Subtract(end, start);
        }

        private void ValidateTrajectory()
        {            

            List<Point> trajectoryPoints = new List<DescartesCoordinatesItem>(coordinates).ConvertAll<Point>(item => new Point
            {
                X = item.Point.X,
                Y = item.Point.Y
            });

            warehouseRobotTrajectoryValidationServiceClient.ValidateTrajectory(robot.Id, trajectoryPoints);
        }

        protected virtual void OnTrajectoryValidated(TrajectoryValidatedEventArguments e)
        {
            EventHandler<TrajectoryValidatedEventArguments> handler = TrajectoryValidated;
            handler?.Invoke(this, e);
        }

        private void TrajectoryValidatedCallback(RobotValidationResult robotValidationResult)
        {
            OnTrajectoryValidated(new TrajectoryValidatedEventArguments(robotValidationResult.RobotId, robotValidationResult.ValidationResult, robotValidationResult.ValidationMessage));
        }
    }
}
