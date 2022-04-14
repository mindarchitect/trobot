using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using TRobot.Core;
using TRobot.Core.Services;
using TRobot.Core.UI.Models;
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

                if (robot.Id == 1)
                {
                    var trajectoryCoordinates = new List<DescartesCoordinatesItem>();

                    trajectoryCoordinates.Add(new DescartesCoordinatesItem(0, 20, 20));
                    trajectoryCoordinates.Add(new DescartesCoordinatesItem(1, 40, 80));
                    trajectoryCoordinates.Add(new DescartesCoordinatesItem(2, 60, 10));
                    trajectoryCoordinates.Add(new DescartesCoordinatesItem(3, 120, 140));
                    trajectoryCoordinates.Add(new DescartesCoordinatesItem(4, 200, 140));

                    warehouseRobot.SetTrajectoryCoordinateItems(trajectoryCoordinates);
                }

                if (robot.Id == 2)
                {
                    var trajectoryCoordinates = new List<DescartesCoordinatesItem>();

                    trajectoryCoordinates.Add(new DescartesCoordinatesItem(0, 120, 170));
                    trajectoryCoordinates.Add(new DescartesCoordinatesItem(1, 140, 230));
                    trajectoryCoordinates.Add(new DescartesCoordinatesItem(2, 160, 160));
                    trajectoryCoordinates.Add(new DescartesCoordinatesItem(3, 220, 290));
                    trajectoryCoordinates.Add(new DescartesCoordinatesItem(4, 300, 290));

                    warehouseRobot.SetTrajectoryCoordinateItems(trajectoryCoordinates);
                }

                Robots.Add(warehouseRobot);
            }
        }
    }
}
