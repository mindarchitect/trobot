﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using TRobot.Communication.Services;
using TRobot.Communication.Services.Trajectory;
using TRobot.Core;
using TRobot.Core.UI.Commands;
using TRobot.ECU.UI.Views;

namespace TRobot.ECU.UI.ViewModels
{
    public class DashboardViewModel : BaseViewModel<DashboardView>
    {
        private ObservableCollection<RobotFactory> robotFactories;
        private IServiceHostProvider<IRobotTrajectoryValidationService> serviceHostProvider;

        private Process monitorProcess;

        public DashboardViewModel(IServiceHostProvider<IRobotTrajectoryValidationService> serviceHostProvider)
        {
            this.serviceHostProvider = serviceHostProvider;
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

        private ICommand newCommand;
        public ICommand NewCommand
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

        private ICommand stopMonitorCommand;
        public ICommand StopMonitorCommand
        {
            get
            {
                if (stopMonitorCommand == null)
                {
                    stopMonitorCommand = new RelayCommand<object>(StopMonitor);
                }

                return stopMonitorCommand;
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

        private ICommand addRobotFactoryCommand;
        public ICommand AddRobotFactoryCommand
        {
            get
            {
                if (addRobotFactoryCommand == null)
                {
                    addRobotFactoryCommand = new RelayCommand<object>(AddRobotFactory);
                }

                return addRobotFactoryCommand;
            }
        }

        private ICommand addRobotCommand;
        public ICommand AddRobotCommand
        {
            get
            {
                if (addRobotCommand == null)
                {
                    addRobotCommand = new RelayCommand<object>(AddRobot);
                }

                return addRobotCommand;
            }
        }

        private void Add(RobotFactory robotFactory)
        {
            RobotFactories.Add(robotFactory);
        }

        private void StartMonitor(object param)
        {
            var isRunning = Process.GetProcessesByName("TRobot.MU.UI.exe").Any();
            if (!isRunning)
            {
                monitorProcess = null;
            }

            if (monitorProcess == null)
            {
                ProcessStartInfo processStartInfo = new ProcessStartInfo("TRobot.MU.UI.exe");                
                processStartInfo.WorkingDirectory = Path.GetFullPath(@"..\..\..\TRobot.MU.UI\bin\Debug");

                monitorProcess = Process.Start(processStartInfo);
            }      
        }

        private void StopMonitor(object param)
        {
            if (monitorProcess != null && !monitorProcess.HasExited)
            {                
                monitorProcess.Kill();
                monitorProcess = null;
            }
        }

        private void ExitApplication(object param)
        {
            Application.Current.Shutdown();
        }

        internal async void OnLoaded(object sender, EventArgs e)
        {
            // For demo purposes all factories are descartes factories, should be defined in database            
            var descartesRobotFactory = DependencyInjector.Resolve<DescartesRobotFactory>();
            await descartesRobotFactory.BuildRobots();
            RobotFactories.Add(descartesRobotFactory);
        }

        internal void OnClosing(object sender, CancelEventArgs e)
        {
            StopMonitor(null);
            serviceHostProvider.Close();

            foreach (var robotFactory in robotFactories)
            {
                foreach (var robot in robotFactory.Robots)
                {
                    robot.Stop();
                }
            }
        }

        private void AddRobotFactory(object param)
        {
            
        }

        private void AddRobot(object param)
        {

        }
    }
}
