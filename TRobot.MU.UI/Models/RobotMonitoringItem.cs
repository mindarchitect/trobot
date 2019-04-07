using System;
using System.Windows;
using System.Windows.Media;
using TRobot.ECU.UI.ViewModels;

namespace TRobot.MU.UI.Models
{
    public class RobotMonitoringItem: BaseViewModel
    {
        private Point currentPosition;

        public Point StartPoint { get; set; }
        public PointCollection Trajectory { get; set; }        
        public Point CurrentPosition
        {
            get
            {
                return currentPosition;
            }

            set
            {
                currentPosition = value;
                OnPropertyChanged("CurrentPosition");
            }
        }
        public Color Color { get; set; }
        public Guid Guid { get; set; }
        public string Title { get; set; }
    }
}
