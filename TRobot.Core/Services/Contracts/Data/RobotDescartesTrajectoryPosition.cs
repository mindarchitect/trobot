using System;
using System.Runtime.Serialization;
using System.Windows;

namespace TRobot.Core.Services.Contracts.Data
{    

    [DataContract]
    public class RobotDescartesTrajectoryPosition
    {
        [DataMember]
        public Guid RobotId { get; set; }

        [DataMember]
        public string RobotTitle { get; set; }

        [DataMember]
        public Point CurrentPosition { get; set; }

    }
}
