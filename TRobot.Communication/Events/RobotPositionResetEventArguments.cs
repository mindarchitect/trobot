using System;
using TRobot.Core.Services.Contracts.Data;

namespace TRobot.Communication.Events
{
    public class RobotTestEventArguments : EventArgs
    {
        public RobotTestEventArguments(Robot robot)
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
