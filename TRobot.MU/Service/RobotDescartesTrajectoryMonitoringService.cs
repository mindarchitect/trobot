using System;
using System.ServiceModel;
using TRobot.Communication.Contracts.Data;
using TRobot.Communication.Trajectory;

namespace TRobot.MU.Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class RobotDescartesTrajectoryMonitoringService : IRobotTrajectoryMonitoringService
    {        
        public RobotDescartesTrajectoryMonitoringService()
        {            
        }

        public void PlotRobotTrajectory(RobotDescartesTrajectory robotTrajectory)
        {            
        }

        public void UpdateRobotPosition(RobotDescartesTrajectoryPosition robotTrajectory)
        {
            throw new NotImplementedException();
        }
    }
}
