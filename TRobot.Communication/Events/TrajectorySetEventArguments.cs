using System;
using TRobot.Core.Services.Contracts.Data;

namespace TRobot.Communication.Events
{
    public class TrajectorySetEventArguments : EventArgs
    {
        public TrajectorySetEventArguments(RobotDescartesTrajectory robotDescartesTrajectory)
        {
            RobotDescartesTrajectory = robotDescartesTrajectory;
        }

        public RobotDescartesTrajectory RobotDescartesTrajectory
        {
            get;
            private set;
        }
    }
}
