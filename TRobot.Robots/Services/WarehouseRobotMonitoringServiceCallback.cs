using System;
using TRobot.Communication.Monitoring;

namespace TRobot.Robots.Services
{
    internal class WarehouseRobotMonitoringServiceCallback : IRobotTrajectoryMonitoringServiceCallback
    {
        private Action trajectorySetCallback;
        private Action trajectoryUpdatedCallback;
        internal WarehouseRobotMonitoringServiceCallback(Action trajectorySetCallback, Action trajectoryUpdatedCallback)
        {
            this.trajectorySetCallback = trajectorySetCallback;
            this.trajectoryUpdatedCallback = trajectoryUpdatedCallback;
        }

        public void RobotTrajectorySet()
        {
            trajectorySetCallback();
        }

        public void RobotTrajectoryUpdated()
        {
            trajectoryUpdatedCallback();
        }
    }
}
