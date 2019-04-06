using System;

namespace TRobot.Core
{
    public class VelocityChangedEventArguments : EventArgs
    {
        public VelocityChangedEventArguments(uint newVelocity)
        {
            Velocity = newVelocity;
        }

        public uint Velocity
        {
            get;
            private set;
        }
    }
}
