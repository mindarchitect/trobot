using System;
using System.Threading;
using TRobot.Core.Enums;

namespace TRobot.Core
{
    public class RobotDimensionalDrive : Drive, IControllable
    {
        event EventHandler<VelocityChangedEventArguments> VelocityChanged;
        public Dimension Dimension { get; private set; }

        public uint AngularVelocity { get; private set; }

        public uint AngularAcceleration { get; private set; }

        private Thread driveThread;

        private uint Radius { get; set; }

        public RobotDimensionalDrive(Dimension dimension)
        {
            Dimension = dimension;
        }

        public void Start()
        {
            ThreadStart threadDelegate = new ThreadStart(this.ProduceVelocity);
            driveThread = new Thread(threadDelegate);
            driveThread.Start();
        }

        public void Stop()
        {
            if (driveThread != null)
            {
                driveThread.Abort();
            }
        }

        public void Pause()
        {
            throw new NotImplementedException();
        }

        private void ProduceVelocity()
        {

        }
    }
}
