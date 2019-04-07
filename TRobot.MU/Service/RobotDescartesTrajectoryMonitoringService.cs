using System;
using System.ServiceModel;
using TRobot.Communication.Contracts.Data;
using TRobot.Communication.Events;
using TRobot.Communication.Services.Monitoring;

namespace TRobot.MU.Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class RobotDescartesTrajectoryMonitoringService : IRobotTrajectoryMonitoringService
    {
        public event EventHandler<TrajectorySetEventArguments> RobotTrajectorySet;
        public event EventHandler<RobotPositionUpdatedEventArguments> RobotPositionUpdated;

        public RobotDescartesTrajectoryMonitoringService()
        {            
        }        

        public void SetupRobotTrajectory(RobotDescartesTrajectory robotTrajectory)
        {
            OnTrajectorySet(new TrajectorySetEventArguments(robotTrajectory));

            OperationContext operationContext = OperationContext.Current;
            IRobotTrajectoryMonitoringServiceCallback callback = operationContext.GetCallbackChannel<IRobotTrajectoryMonitoringServiceCallback>();
            callback.RobotTrajectorySet();
        }

        public void UpdateRobotPosition(RobotDescartesTrajectoryPosition robotPosition)
        {
            OnRobotPositionUpdated(new RobotPositionUpdatedEventArguments(robotPosition));

            OperationContext operationContext = OperationContext.Current;
            IRobotTrajectoryMonitoringServiceCallback callback = operationContext.GetCallbackChannel<IRobotTrajectoryMonitoringServiceCallback>();
            callback.RobotTrajectoryUpdated();
        }

        protected virtual void OnTrajectorySet(TrajectorySetEventArguments e)
        {
            RobotTrajectorySet?.Invoke(this, e);
        }

        protected virtual void OnRobotPositionUpdated(RobotPositionUpdatedEventArguments e)
        {
            RobotPositionUpdated?.Invoke(this, e);
        }
    }
}
