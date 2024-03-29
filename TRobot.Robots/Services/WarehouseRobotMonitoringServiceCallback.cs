﻿using System;
using TRobot.Communication.Services.Monitoring;

namespace TRobot.Robots.Services
{
    internal class WarehouseRobotMonitoringServiceCallback : IRobotTrajectoryMonitoringServiceCallback
    {
        private Action trajectorySetCallback;
        private Action trajectoryCleanedCallback;
        private Action trajectoryUpdatedCallback;
        private Action testOperationCallback;

        internal WarehouseRobotMonitoringServiceCallback(Action trajectorySetCallback, Action trajectoryCleanedCallback, Action trajectoryUpdatedCallback, Action testOperationCallback)
        {
            this.trajectorySetCallback = trajectorySetCallback;
            this.trajectoryCleanedCallback = trajectoryCleanedCallback;
            this.trajectoryUpdatedCallback = trajectoryUpdatedCallback;
            this.testOperationCallback = testOperationCallback;
        }

        public void RobotTrajectorySetCallback()
        {
            trajectorySetCallback();
        }

        public void RobotTrajectoryCleanedCallback()
        {
            trajectoryCleanedCallback();
        }

        public void RobotTrajectoryUpdatedCallback()
        {
            trajectoryUpdatedCallback();
        }

        public void TestOperationCallback()
        {
            testOperationCallback();
        }
    }
}
