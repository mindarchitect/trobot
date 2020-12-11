using System.Runtime.Serialization;
using System.Windows;

namespace TRobot.Core.Services.Contracts.Data
{

    [DataContract]
    public class RobotDescartesTrajectoryPosition : Robot
    {
        [DataMember]
        public Point CurrentPosition { get; set; }

    }
}
