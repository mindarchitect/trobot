using System;
using System.ServiceModel;
using TRobot.Communication.Events;
using TRobot.Communication.Services.Monitoring;
using TRobot.Core.Services.Contracts.Data;

namespace TRobot.MU.Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class RobotDescartesTrajectoryMonitoringService : IRobotTrajectoryMonitoringService
    {
        public event EventHandler<TrajectoryCleanedEventArguments> RobotTrajectoryCleaned;
        public event EventHandler<TrajectorySetEventArguments> RobotTrajectorySet;
        public event EventHandler<RobotPositionUpdatedEventArguments> RobotPositionUpdated;
        public event EventHandler<RobotTestEventArguments> TestEvent;

        public RobotDescartesTrajectoryMonitoringService()
        {            
        }        

        public void SetupRobotTrajectory(RobotDescartesTrajectory robotTrajectory)
        {
            OnTrajectorySet(new TrajectorySetEventArguments(robotTrajectory));

            OperationContext operationContext = OperationContext.Current;
            IRobotTrajectoryMonitoringServiceCallback callback = operationContext.GetCallbackChannel<IRobotTrajectoryMonitoringServiceCallback>();
            callback.RobotTrajectorySetCallback();
        }

        public void UpdateRobotPosition(RobotDescartesTrajectoryPosition robotPosition)
        {
            OnRobotPositionUpdated(new RobotPositionUpdatedEventArguments(robotPosition));

            OperationContext operationContext = OperationContext.Current;
            IRobotTrajectoryMonitoringServiceCallback callback = operationContext.GetCallbackChannel<IRobotTrajectoryMonitoringServiceCallback>();
            callback.RobotTrajectoryUpdatedCallback();
        }

        public void ClearRobotTrajectory(Guid robotId)
        {
            OnTrajectoryCleaned(new TrajectoryCleanedEventArguments(robotId));

            OperationContext operationContext = OperationContext.Current;
            IRobotTrajectoryMonitoringServiceCallback callback = operationContext.GetCallbackChannel<IRobotTrajectoryMonitoringServiceCallback>();
            callback.RobotTrajectoryCleanedCallback();
        }

        public void TestOperation(Robot robot)
        {
            OnTestOperationEvent(new RobotTestEventArguments(robot));

            OperationContext operationContext = OperationContext.Current;
            IRobotTrajectoryMonitoringServiceCallback callback = operationContext.GetCallbackChannel<IRobotTrajectoryMonitoringServiceCallback>();
            callback.TestOperationCallback();
        }

        protected virtual void OnTrajectorySet(TrajectorySetEventArguments e)
        {
            RobotTrajectorySet?.Invoke(this, e);
        }

        protected virtual void OnTrajectoryCleaned(TrajectoryCleanedEventArguments e)
        {
            RobotTrajectoryCleaned?.Invoke(this, e);
        }

        protected virtual void OnRobotPositionUpdated(RobotPositionUpdatedEventArguments e)
        {
            RobotPositionUpdated?.Invoke(this, e);
        }

        protected virtual void OnTestOperationEvent(RobotTestEventArguments e)
        {
            TestEvent?.Invoke(this, e);
        }
    }
}
