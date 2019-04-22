using System.Windows;
using TRobot.ECU.UI.ViewModels;

namespace TRobot.ECU.Models
{
    public sealed class DescartesCoordinatesItem : BaseViewModel
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

        public Point Point
        {
            get { return point; }
            set
            {
                point = value;
                OnPropertyChanged("Point");
            }
        }               
    }
}
