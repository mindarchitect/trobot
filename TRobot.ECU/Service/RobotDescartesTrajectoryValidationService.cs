using System.ServiceModel;
using TRobot.Communication.Contracts.Data;
using TRobot.Communication.Services.Trajectory;
using TRobot.Core;

namespace TRobot.ECU.UI.Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class RobotDescartesTrajectoryValidationService : IRobotTrajectoryValidationService
    {
        private DescartesRobotFactory descartesRobotFactory;
        public RobotDescartesTrajectoryValidationService(DescartesRobotFactory descartesRobotFactory)
        {
            this.descartesRobotFactory = descartesRobotFactory;
        }

        public void ValidateRobotTrajectory(RobotDescartesTrajectory robotTrajectory)
        {
            var robotValidationResult = new RobotValidationResult();
            robotValidationResult.RobotId = robotTrajectory.RobotId;

            var trajectoryPoints = robotTrajectory.Trajectory;

            bool result = true;

            foreach (var trajectoryPoint in trajectoryPoints)
            {
                result = trajectoryPoint.X <= descartesRobotFactory.Dimensions.Width && trajectoryPoint.Y <= descartesRobotFactory.Dimensions.Height;

                if (!result)
                {
                    robotValidationResult.ValidationMessage = "Trajectory coordinare are out of bounds";
                    break;
                }              
            }

            robotValidationResult.ValidationResult = result;

            OperationContext operationContext = OperationContext.Current;
            IRobotTrajectoryValidationServiceCallback callback = operationContext.GetCallbackChannel<IRobotTrajectoryValidationServiceCallback>();
            callback.RobotTrajectoryValidated(robotValidationResult);
        }
    }
}
