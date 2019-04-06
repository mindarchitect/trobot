using System;
using System.Windows;

namespace TRobot.Core.Robot.Events
{
    public class PositionChangedEventArguments : EventArgs
    {
        public PositionChangedEventArguments(Point newPosition)
        {
            this.NewPosition = newPosition;
        }

        public Point NewPosition
        {
            get;
            private set;
        }
    }
}
