using System;
using TRobot.Core.Services.Contracts.Data;

namespace TRobot.Communication.Events
{
    public class RobotPositionResetEventArguments : EventArgs
    {
        public RobotPositionResetEventArguments(Robot robot)
        {
            Robot = robot;
        }

        public Robot Robot
        {
            get;
            private set;
        }
    }
}
