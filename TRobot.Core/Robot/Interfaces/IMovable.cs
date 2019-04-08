using System;
using System.Windows;
using TRobot.Core.Robot.Events;

namespace TRobot.Core
{
    public interface IMovable
    {
        double Velocity { get; set; }
        double Acceleration { get; set; }

        void Start();

        void Stop();

        Point CurrentPosition { get; set; }        

        event EventHandler<PositionChangedEventArguments> PositionChanged;
    }
}
