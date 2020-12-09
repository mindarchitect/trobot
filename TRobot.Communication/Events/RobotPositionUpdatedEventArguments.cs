using System;
using TRobot.Core.Services.Contracts.Data;

namespace TRobot.Communication.Events
{
    public class RobotPositionUpdatedEventArguments : EventArgs
    {
        public RobotPositionUpdatedEventArguments(RobotDescartesTrajectoryPosition robotDescartesTrajectoryPosition)
        {
            RobotDescartesTrajectoryPosition = robotDescartesTrajectoryPosition;
        }

        public RobotDescartesTrajectoryPosition RobotDescartesTrajectoryPosition
        {
            get;
            private set;
        }
    }
}
