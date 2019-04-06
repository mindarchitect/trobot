using System;
using System.Runtime.Serialization;

namespace TRobot.Communication.Services.Trajectory
{
    [DataContract]
    public class RobotValidationResult
    {
        [DataMember]
        public Guid RobotId { get; set; }

        [DataMember]
        public bool ValidationResult { get; set; }

        [DataMember]
        public string ValidationMessage { get; set; }

    }
}
