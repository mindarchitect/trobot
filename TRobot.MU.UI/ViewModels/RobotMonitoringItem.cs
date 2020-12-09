using System;
using System.Windows;
using System.Windows.Media;
using TRobot.ECU.UI.ViewModels;

namespace TRobot.MU.UI.Models
{
    class RobotMonitoringItem: BaseViewModel
    {
        private Point startPosition;
        private Point currentPosition;
        private Color color;
        private string title;
        private PointCollection trajectory;
        public Point StartPoint
        {
            get
            {
                return startPosition;
            }
            set
            {
                startPosition = value;
                OnPropertyChanged("StartPoint");
            }
        }
        public Guid Guid { get; set; }
        public PointCollection Trajectory
        {
            get
            {
                return trajectory;
            }
            set
            {
                trajectory = value;
                OnPropertyChanged("Trajectory");
            }
        }        
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
        public Color Color
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
                OnPropertyChanged("Color");
            }
        }        
        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
                OnPropertyChanged("Title");
            }
        }
    }
}
