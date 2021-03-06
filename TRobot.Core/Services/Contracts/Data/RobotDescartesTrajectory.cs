﻿using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Windows;

namespace TRobot.Core.Services.Contracts.Data
{
    [DataContract]
    public class RobotDescartesTrajectory : RobotDescartesTrajectoryPosition
    {
        [DataMember]
        public IEnumerable<Point> Trajectory { get; set; }        
    }
}
