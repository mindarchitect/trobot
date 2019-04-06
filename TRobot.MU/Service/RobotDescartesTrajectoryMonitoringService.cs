using System.ServiceModel;
using TRobot.Communication.Contracts.Data;
using TRobot.Communication.Monitoring;
using TRobot.Communication.Trajectory;

namespace TRobot.MU.Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class RobotDescartesTrajectoryMonitoringService : IRobotTrajectoryMonitoringService
    { 
               
        public RobotDescartesTrajectoryMonitoringService()
        {            
        }

        public void SetupRobotTrajectory(RobotDescartesTrajectory robotTrajectory)
        {
            OperationContext operationContext = OperationContext.Current;
            IRobotTrajectoryMonitoringServiceCallback callback = operationContext.GetCallbackChannel<IRobotTrajectoryMonitoringServiceCallback>();
            callback.RobotTrajectorySet();
        }

        public void UpdateRobotPosition(RobotDescartesTrajectoryPosition robotTrajectory)
        {
            OperationContext operationContext = OperationContext.Current;
            IRobotTrajectoryMonitoringServiceCallback callback = operationContext.GetCallbackChannel<IRobotTrajectoryMonitoringServiceCallback>();
            callback.RobotTrajectoryUpdated();
        }
    }
}
