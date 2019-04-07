using TRobot.Core;
using TRobot.Core.Enums;

namespace TRobot.Robots
{
    internal sealed class WarehouseRobotEngine : RobotEngine
    {
        private Thread trajectoryCalculation        
        
        internal WarehouseRobotEngine() : base()
        {
            var xdrive = new RobotDimensionalDrive(Dimension.X);
            var ydrive = new RobotDimensionalDrive(Dimension.Y);

            Drives.Add(xdrive);
            Drives.Add(ydrive);
        }

        public override void Start()
        {

        }
    }
}
