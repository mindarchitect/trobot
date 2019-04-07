using System;

namespace TRobot.Core
{
    public class VelocityChangedEventArguments : EventArgs
    {
        public VelocityChangedEventArguments(double velocity)
        {
            Velocity = velocity;
        }

        public double Velocity
        {
            get;
            private set;
        }
    }
}
