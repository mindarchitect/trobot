using System.ServiceModel;

namespace TRobot.Communication.Trajectory
{
    public interface IRobotTrajectoryValidationServiceCallback
    {
        [OperationContract(IsOneWay = true)]
        void RobotTrajectoryValidated(RobotValidationResult robotValidation);
    }
}
