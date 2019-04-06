using System.ServiceModel;

namespace TRobot.Communication.Monitoring
{
    public interface IRobotTrajectoryMonitoringServiceCallback
    {
        [OperationContract(IsOneWay = true)]
        void RobotTrajectorySet();

        [OperationContract(IsOneWay = true)]
        void RobotTrajectoryUpdated();
    }
}
