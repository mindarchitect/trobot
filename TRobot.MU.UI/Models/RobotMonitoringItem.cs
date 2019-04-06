using System;
using System.Windows;
using System.Windows.Media;

namespace TRobot.MU.UI.Models
{
    public class RobotMonitoringItem
    {
        public Point StartPoint { get; set; }
        public PointCollection Trajectory { get; set; }
        public Point CurrentPosition { get; set; }
        public Color Color { get; set; }
        public Guid Guid { get; set; }
        public string Title { get; set; }
    }
}
