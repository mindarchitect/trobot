using System;
using System.Threading;
using TRobot.Core.Enums;

namespace TRobot.Core
{
    public class RobotDimensionalDrive : Drive
    {        
        public Dimension Dimension { get; private set; }

        public double Velocity { get; set; }        

        public RobotDimensionalDrive(Dimension dimension)
        {
            Dimension = dimension;
        }        
    }
}
