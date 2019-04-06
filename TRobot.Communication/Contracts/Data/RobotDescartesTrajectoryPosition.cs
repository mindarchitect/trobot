using System;
using System.Runtime.Serialization;
using System.Windows;

namespace TRobot.Communication.Contracts.Data
{    

    [DataContract]
    public class RobotDescartesTrajectoryPosition
    {
        [DataMember]
        public Guid RobotId { get; set; }

        [DataMember]
        public Point CurrentPosition { get; set; }
    }
}
