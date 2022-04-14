using System;
using System.Threading.Tasks;
using System.Windows;
using TRobot.Core;
using TRobot.Core.Services;
using TRobot.Robots;
using Unity;

namespace TRobot.ECU
{
    public sealed class DescartesRobotFactory : RobotFactory
    {
        [Dependency]
        public IFactoriesService FactoriesService { get; set; }

        public Size Dimensions { get; private set; }  
        
        public DescartesRobotFactory(string name, Size dimensions) : base(name)
        {
            Dimensions = dimensions;
        }

        public async Task BuildRobots()
        {
            // Reading database settings
            // TODO
            // Put factory type in database
            FactoriesService = DependencyInjector.Resolve<IFactoriesService>();

            var factory = await FactoriesService.GetFactoryById(1);
            var factoryRobots = factory.Robots;

            foreach (var robot in factoryRobots)
            {
                var warehouseRobot = new WarehouseRobot(Guid.Parse(robot.Guid));
                warehouseRobot.Title = robot.Name;
                Robots.Add(warehouseRobot);
            }
        }
    }
}
