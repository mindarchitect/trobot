using System.ServiceModel;

namespace TRobot.Communication.Services.Monitoring
{
    public interface IRobotTrajectoryMonitoringServiceCallback
    {
        [OperationContract(IsOneWay = true)]
        void RobotTrajectorySet();

        [OperationContract(IsOneWay = true)]
        void RobotTrajectoryUpdated();

        [OperationContract(IsOneWay = true)]
        void RobotPositionReset();
    }
}
