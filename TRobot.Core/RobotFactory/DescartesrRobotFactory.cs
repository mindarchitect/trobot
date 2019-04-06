using System.Windows;

namespace TRobot.Core
{
    public sealed class DescartesRobotFactory : RobotFactory
    {      
        public Size Dimensions { get; private set; }  
        
        public DescartesRobotFactory(string name, Size dimensions) : base(name)
        {
            Dimensions = dimensions;
        }
    }
}
