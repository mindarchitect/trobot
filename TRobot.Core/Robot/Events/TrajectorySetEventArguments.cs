using System;
using TRobot.Communication.Contracts.Data;

namespace TRobot.Core.Robot.Events
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
