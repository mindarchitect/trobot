using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using TRobot.Core;
using TRobot.Core.UI.Commands;
using TRobot.ECU.Service;
using TRobot.Robots;

namespace TRobot.ECU.UI.ViewModels
{
    public class DashboardViewControlModel : BaseViewModel
    {
        private ObservableCollection<RobotFactory> robotFactories;
        private RobotDescartesTrajectoryValidationServiceHost trajectoryValidationServiceHost;

        public DashboardViewControlModel()
        {
            var descartesRobotFactory = new DescartesRobotFactory("Test robot factory", new Size(100, 100));

            trajectoryValidationServiceHost = new RobotDescartesTrajectoryValidationServiceHost(descartesRobotFactory);
            
            var warehouseRobot1 = new WarehouseRobot(descartesRobotFactory);
            warehouseRobot1.Title = "Warehouse Robot 1";

            var warehouseRobot2 = new WarehouseRobot(descartesRobotFactory);
            warehouseRobot2.Title = "Warehouse Robot 2";

            descartesRobotFactory.Robots.Add(warehouseRobot1);
            descartesRobotFactory.Robots.Add(warehouseRobot2);
            RobotFactories.Add(descartesRobotFactory);
        }

        public ObservableCollection<RobotFactory> RobotFactories
        {
            get
            {
                if (robotFactories == null)
                {
                    robotFactories = new ObservableCollection<RobotFactory>();
                    var itemsView = (IEditableCollectionView) CollectionViewSource.GetDefaultView(robotFactories);
                    itemsView.NewItemPlaceholderPosition = NewItemPlaceholderPosition.AtEnd;                    
                }

                return robotFactories;
            }
        }

        private RelayCommand<RobotFactory> newCommand;
        public RelayCommand<RobotFactory> NewCommand
        {
            get
            {
                if (newCommand == null)
                {
                    newCommand = new RelayCommand<RobotFactory>(Add);
                }

                return newCommand;
            }
        }

        private void Add(RobotFactory robotFactory)
        {
            RobotFactories.Add(robotFactory);
        }
    }
}
