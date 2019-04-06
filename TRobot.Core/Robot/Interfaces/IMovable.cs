using System;
using System.Windows;

namespace TRobot.Core
{
    public interface IMovable
    {
        uint Velocity { get; set; }
        uint Acceleration { get; set; }

        void Move();

        void Stop();

        Point CurrentPosition { get; set; }        

        event EventHandler<PositionChangedEventArguments> PositionChanged;
    }
}
