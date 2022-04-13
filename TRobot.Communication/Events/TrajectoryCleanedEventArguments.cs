using System;

namespace TRobot.Communication.Events
{
    public class TrajectoryCleanedEventArguments : EventArgs
    {
        public TrajectoryCleanedEventArguments(Guid robotId)
        {
            RobotId = robotId;
        }

        public Guid RobotId
        {
            get;
            private set;
        }
    }
}
