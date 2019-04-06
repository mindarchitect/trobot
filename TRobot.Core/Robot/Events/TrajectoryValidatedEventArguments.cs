using System;
using System.Windows;

namespace TRobot.Core
{
    public class TrajectoryValidatedEventArguments : EventArgs
    {
        public TrajectoryValidatedEventArguments(Guid robotId, bool validationResult, string validationMessage)
        {
            RobotId = robotId;
            ValidationResult = validationResult;
            ValidationMessage = validationMessage;
        }

        public bool ValidationResult
        {
            get;
            private set;
        }

        public string ValidationMessage
        {
            get;
            private set;
        }

        public Guid RobotId
        {
            get;
            private set;
        }
    }
}
