using System;
using TRobot.Communication.Trajectory;

namespace TRobot.Robots.Services
{
    internal class WarehouseRobotTrajectoryValidationCallback : IRobotTrajectoryValidationServiceCallback
    {
        private Action<RobotValidationResult> callback;
        internal WarehouseRobotTrajectoryValidationCallback(Action<RobotValidationResult> callback)
        {
            this.callback = callback;
        }
        public void RobotTrajectoryValidated(RobotValidationResult robotValidation)
        {
            callback(robotValidation);
        }
    }
}
