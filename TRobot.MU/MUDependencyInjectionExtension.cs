using TRobot.Communication.Services;
using TRobot.Communication.Services.Monitoring;
using TRobot.Core;
using TRobot.MU.Service;
using Unity.Extension;

namespace TRobot.MU
{
    public class MUDependencyInjectionExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            DependencyInjector.RegisterType<IRobotTrajectoryMonitoringService, RobotDescartesTrajectoryMonitoringService>();
            DependencyInjector.RegisterType<IServiceHostProvider<IRobotTrajectoryMonitoringService>, RobotDescartesTrajectoryMonitoringServiceHost>();
        }
    }
}
