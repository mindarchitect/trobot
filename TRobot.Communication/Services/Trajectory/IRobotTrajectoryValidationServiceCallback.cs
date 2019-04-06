using System.ServiceModel;

namespace TRobot.Communication.Services.Trajectory
{
    public interface IRobotTrajectoryValidationServiceCallback
    {
        [OperationContract(IsOneWay = true)]
        void RobotTrajectoryValidated(RobotValidationResult robotValidation);
    }
}
