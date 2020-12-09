using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
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
            var descartesRobotFactory = new DescartesRobotFactory("Test robot factory", new Size(300, 300));

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
                    var itemsView = (IEditableCollectionView)CollectionViewSource.GetDefaultView(robotFactories);
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

        private ICommand startMonitorCommand;
        public ICommand StartMonitorCommand
        {
            get
            {
                if (startMonitorCommand == null)
                {
                    startMonitorCommand = new RelayCommand<object>(StartMonitor);
                }

                return startMonitorCommand;
            }
        }

        private ICommand exitApplicationCommand;
        public ICommand ExitApplicationCommand
        {
            get
            {
                if (exitApplicationCommand == null)
                {
                    exitApplicationCommand = new RelayCommand<object>(ExitApplication);
                }

                return exitApplicationCommand;
            }
        }

        private void Add(RobotFactory robotFactory)
        {
            RobotFactories.Add(robotFactory);
        }

        private void StartMonitor(object param)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo("TRobot.MU.UI.exe");                       
            processStartInfo.WorkingDirectory = Path.GetFullPath(@"..\..\..\TRobot.MU.UI\bin\Debug");

            Process.Start(processStartInfo);
        }

        private void ExitApplication(object param)
        {
            Application.Current.Shutdown();
        }

        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            trajectoryValidationServiceHost.Close();

            foreach (var robotFactory in robotFactories)
            {
                foreach (var robot in robotFactory.Robots)
                {
                    robot.Stop();
                }
            }
        }
    }
}
