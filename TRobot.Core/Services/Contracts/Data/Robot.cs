using System;
using System.Runtime.Serialization;

namespace TRobot.Core.Services.Contracts.Data
{
    [DataContract]
    public class Robot
    {
        [DataMember]
        public Guid RobotId { get; set; }

        [DataMember]
        public string RobotTitle { get; set; }
    }
}
