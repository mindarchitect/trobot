using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Windows;

namespace TRobot.Communication.Trajectory
{
    [DataContract]
    public class RobotDescartesTrajectoryPoints
    {
        [DataMember]
        public Guid RobotId { get; set; }

        [DataMember]
        public List<Point> TrajectoryPoints { get; set; }        
    }
}
