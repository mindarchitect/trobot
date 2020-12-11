using System.Collections.Generic;

namespace TRobot.Core
{
    public class RobotEngine : Engine
    {
        protected List<Drive> Drives
        {
            get;
            private set;
        }

        protected RobotEngine()
        {
            Drives = new List<Drive>();
        }    
    }
}
