using System.Data.Entity;
using TRobot.Core;
using TRobot.Data.Contexts;
using Unity.Extension;

namespace TRobot.Data
{
    public class DataDependencyInjectionExtension : UnityContainerExtension    
    {
        protected override void Initialize()
        {
            DependencyInjector.RegisterType<DbContext, TRobotDatabaseContext>();
        }
    }
}
