using System.ServiceModel;

namespace TRobot.Communication.Services.Monitoring
{
    public interface IRobotTrajectoryMonitoringServiceCallback
    {
        [OperationContract(IsOneWay = true)]
        void RobotTrajectorySetCallback();

        [OperationContract(IsOneWay = true)]
        void RobotTrajectoryCleanedCallback();

        [OperationContract(IsOneWay = true)]
        void RobotTrajectoryUpdatedCallback();

        [OperationContract(IsOneWay = true)]
        void TestOperationCallback();
    }
}
