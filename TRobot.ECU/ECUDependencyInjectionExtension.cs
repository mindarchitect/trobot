using System.Windows;
using TRobot.Communication.Services;
using TRobot.Communication.Services.Trajectory;
using TRobot.Core;
using TRobot.Core.Data;
using TRobot.Core.Data.Entities;
using TRobot.Core.Services;
using TRobot.Data;
using TRobot.ECU.Data.Repositories;
using TRobot.ECU.Services;
using Unity.Extension;

namespace TRobot.ECU
{
    public class ECUDependencyInjectionExtension : UnityContainerExtension    
    {
        protected override void Initialize()
        {
            DependencyInjector.AddExtension<DataDependencyInjectionExtension>();

            DependencyInjector.RegisterType<IAsyncRepository<FactoryEntity>, FactoryRepository>();
            DependencyInjector.RegisterType<IFactoryService, FactoryService>();
            DependencyInjector.RegisterInstance(new DescartesRobotFactory("Test robot factory", new Size(300, 300)));

            DependencyInjector.RegisterType<IRobotTrajectoryValidationService, RobotDescartesTrajectoryValidationService>();
            DependencyInjector.RegisterType<IServiceHostProvider<IRobotTrajectoryValidationService>, RobotDescartesTrajectoryValidationServiceHost>();
        }
    }
}
