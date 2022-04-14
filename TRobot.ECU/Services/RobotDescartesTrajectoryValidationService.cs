using System.ServiceModel;
using TRobot.Core.Services.Contracts.Data;
using TRobot.Communication.Services.Trajectory;
using Unity;

namespace TRobot.ECU.Services
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class RobotDescartesTrajectoryValidationService : IRobotTrajectoryValidationService
    {        
        public RobotDescartesTrajectoryValidationService()
        {            
        }

        public void ValidateRobotTrajectory(RobotDescartesTrajectory robotTrajectory)
        {
            var robotValidationResult = new RobotValidationResult();
            robotValidationResult.RobotId = robotTrajectory.RobotId;

            var trajectoryPoints = robotTrajectory.Trajectory;

            bool result = true;

            foreach (var trajectoryPoint in trajectoryPoints)
            {
                result = trajectoryPoint.X <= DescartesRobotFactory.Dimensions.Width && trajectoryPoint.Y <= DescartesRobotFactory.Dimensions.Height;

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

        [Dependency]
        public DescartesRobotFactory DescartesRobotFactory
        {
            get;
            set;
        }
    }
}
