using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Windows;

namespace TRobot.Communication.Contracts.Data
{
    [DataContract]
    public class RobotDescartesTrajectory : RobotDescartesTrajectoryPosition
    {
        [DataMember]
        public IEnumerable<Point> Trajectory { get; set; }        
    }
}
