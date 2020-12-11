using System;
using TRobot.Communication.Services.Monitoring;

namespace TRobot.Robots.Services
{
    internal class WarehouseRobotMonitoringServiceCallback : IRobotTrajectoryMonitoringServiceCallback
    {
        private Action trajectorySetCallback;
        private Action trajectoryUpdatedCallback;
        private Action robotPositionResetCallback;

        internal WarehouseRobotMonitoringServiceCallback(Action trajectorySetCallback, Action trajectoryUpdatedCallback, Action robotPositionResetCallback)
        {
            this.trajectorySetCallback = trajectorySetCallback;
            this.trajectoryUpdatedCallback = trajectoryUpdatedCallback;
            this.robotPositionResetCallback = robotPositionResetCallback;
        }

        public void RobotTrajectorySet()
        {
            trajectorySetCallback();
        }

        public void RobotTrajectoryUpdated()
        {
            trajectoryUpdatedCallback();
        }

        public void RobotPositionReset()
        {
            robotPositionResetCallback();
        }
    }
}
