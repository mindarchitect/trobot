using System;
using TRobot.Communication.Services.Trajectory;

namespace TRobot.Robots.Services
{
    internal class WarehouseRobotTrajectoryValidationServiceCallback : IRobotTrajectoryValidationServiceCallback
    {
        private Action<RobotValidationResult> callback;
        internal WarehouseRobotTrajectoryValidationServiceCallback(Action<RobotValidationResult> callback)
        {
            this.callback = callback;
        }

        public void RobotTrajectoryValidated(RobotValidationResult robotValidation)
        {
            callback(robotValidation);
        }
    }
}
