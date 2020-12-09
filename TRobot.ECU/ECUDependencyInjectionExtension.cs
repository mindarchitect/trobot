using System.Windows;
using TRobot.Communication.Services;
using TRobot.Communication.Services.Trajectory;
using TRobot.Core;
using TRobot.ECU.Service;
using Unity.Extension;
using Unity.Injection;

namespace TRobot.ECU
{
    public class ECUDependencyInjectionExtension : UnityContainerExtension    
    {
        protected override void Initialize()
        {        
            DependencyInjector.RegisterInstance(new DescartesRobotFactory("Test robot factory", new Size(300, 300)));

            DependencyInjector.RegisterType<IRobotTrajectoryValidationService, RobotDescartesTrajectoryValidationService>();
            DependencyInjector.RegisterType<IServiceHostProvider<IRobotTrajectoryValidationService>, RobotDescartesTrajectoryValidationServiceHost>();
        }
    }
}
