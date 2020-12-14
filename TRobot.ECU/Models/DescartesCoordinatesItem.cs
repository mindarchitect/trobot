using System.Windows;
using TRobot.Core.UI.ViewModels;

namespace TRobot.ECU.Models
{
    public sealed class DescartesCoordinatesItem : ViewModel
    {
        private uint step;
        private Point point;

        public DescartesCoordinatesItem(uint step, uint x, uint y)
        {
            Step = step;
            point = new Point(x, y);
        }        

        public uint Step
        {
            get { return step; }
            set
            {
                step = value;
                OnPropertyChanged("Step");
            }
        }

        public double X
        {
            get { return point.X; }
            set
            {
                point.X = value;
                OnPropertyChanged("X");
            }
        }

        public double Y
        {
            get { return point.Y; }
            set
            {
                point.Y = value;
                OnPropertyChanged("Y");
            }
        }

        public Point Point
        {
            get
            {
                return point;
            }
            private set
            {
                point = value;
            }
        }
    }
}
