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
        public event EventHandler<TrajectorySetEventArguments> RobotTrajectorySet;
        public event EventHandler<RobotPositionUpdatedEventArguments> RobotPositionUpdated;
        public event EventHandler<RobotPositionResetEventArguments> RobotPositionReset;

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

        [Obsolete]
        public void ResetRobotPosition(Robot robot)
        {
            OnRobotPositionReset(new RobotPositionResetEventArguments(robot));

            OperationContext operationContext = OperationContext.Current;
            IRobotTrajectoryMonitoringServiceCallback callback = operationContext.GetCallbackChannel<IRobotTrajectoryMonitoringServiceCallback>();
            callback.RobotPositionReset();
        }

        protected virtual void OnTrajectorySet(TrajectorySetEventArguments e)
        {
            RobotTrajectorySet?.Invoke(this, e);
        }

        protected virtual void OnRobotPositionUpdated(RobotPositionUpdatedEventArguments e)
        {
            RobotPositionUpdated?.Invoke(this, e);
        }

        protected virtual void OnRobotPositionReset(RobotPositionResetEventArguments e)
        {
            RobotPositionReset?.Invoke(this, e);
        }
    }
}
