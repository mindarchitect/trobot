using System.ServiceModel;
using TRobot.Communication.Trajectory;

namespace TRobot.MU.UI.Service
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
    }
}
