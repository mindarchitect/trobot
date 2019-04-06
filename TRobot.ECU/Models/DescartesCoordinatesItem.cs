using System.ComponentModel;
using System.Windows;

namespace TRobot.ECU.Models
{
    public sealed class DescartesCoordinatesItem
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

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
