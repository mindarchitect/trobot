using System.Collections.Generic;

namespace TRobot.Core
{
    public class RobotEngine : Engine, IControllable
    {
        public List<IControllable> Drives
        {
            get;
            private set;
        }

        protected RobotEngine()
        {
            Drives = new List<IControllable>();
        }

        public void Start()
        {
            Drives.ForEach((drive) => drive.Start());
        }

        public void Stop()
        {
            Drives.ForEach((drive) => drive.Stop());
        }

        public void Pause()
        {
            Drives.ForEach((drive) => drive.Pause());
        }
    }
}
