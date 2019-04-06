using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Windows;

namespace TRobot.Communication.Trajectory
{
    [DataContract]
    public class RobotDescartesTrajectory
    {
        [DataMember]
        public Guid RobotId { get; set; }

        [DataMember]
        public LinkedList<Vector> Trajectory { get; set; }        
    }
}
