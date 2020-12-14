using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TRobot.Core;
using TRobot.Core.Enums;
using TRobot.Core.UI.Commands;
using TRobot.ECU.Models;
using TRobot.ECU.UI.ViewModels;
using TRobot.Robots.Views;

namespace TRobot.Robots.ViewModels
{
    class WarehouseRobotControlPanelViewModel : BaseViewModel
    {
        public ObservableCollection<DescartesCoordinatesItem> TrajectoryCoordinates { get; set; }
        public WarehouseRobot Robot { get; set; }

        private DescartesCoordinatesItem selectedTrajectoryCoordinatesItem;

        private bool trajectoryValidated;
        private RobotState robotState;

        private ICommand startStopCommand;

        private ICommand uploadSettingsCommand;
        private ICommand resetCommand;
        private ICommand deleteSelectedTrajectoryCoordinatesItemCommand;
        private ICommand addDestinationPointCommand;

        private uint velocity;
        private uint acceleration;
        private CommunicationState communicationState;

        internal WarehouseRobotControlPanelViewModel(WarehouseRobot robot)
        {
            Robot = robot;
            Robot.Controller.TrajectoryValidated += OnControllerRobotTrajectoryValidated;

            // Just for testing  
            TrajectoryCoordinates = new ObservableCollection<DescartesCoordinatesItem>();

            TrajectoryCoordinates.Add(new DescartesCoordinatesItem(0, 20, 20));
            TrajectoryCoordinates.Add(new DescartesCoordinatesItem(1, 40, 80));
            TrajectoryCoordinates.Add(new DescartesCoordinatesItem(2, 60, 10));
            TrajectoryCoordinates.Add(new DescartesCoordinatesItem(3, 120, 140));
            TrajectoryCoordinates.Add(new DescartesCoordinatesItem(4, 200, 140));

            Velocity = 50;
            Acceleration = 4;

            RobotState = Robot.Controller.State;
            Robot.Controller.MonitoringServiceClientInnerChannelStateChanged += Controller_MonitoringServiceClientStateChanged;
        }

        public ICommand UploadSettingsCommand
        {
            get
            {
                if (uploadSettingsCommand == null)
                {
                    uploadSettingsCommand = new RelayCommand<object>(param => UploadRobotSettings());
                }
                return uploadSettingsCommand;
            }
        }

        public ICommand StartStopCommand
        {
            get
            {
                startStopCommand = new RelayCommand<object>(StartStopRobot);
                return startStopCommand;
            }
        }

        public ICommand ResetCommand
        {
            get
            {
                if (resetCommand == null)
                {
                    resetCommand = new RelayCommand<object>(param => ResetRobot());
                }
                return resetCommand;
            }
        }

        public ICommand DeleteSelectedTrajectoryCoordinatesItemCommand
        {
            get
            {
                if (deleteSelectedTrajectoryCoordinatesItemCommand == null)
                {
                    deleteSelectedTrajectoryCoordinatesItemCommand = new RelayCommand<object>(param => DeleteTrajectoryCoordinatesItem((DescartesCoordinatesItem)param), param => true);
                }
                return deleteSelectedTrajectoryCoordinatesItemCommand;
            }
        }

        public ICommand AddDestinationPointCommand
        {
            get
            {
                if (addDestinationPointCommand == null)
                {
                    addDestinationPointCommand = new RelayCommand<object>(param => AddDestinationPoint());
                }
                return addDestinationPointCommand;
            }
        }

        public bool TrajectoryValidated
        {
            get
            {
                return trajectoryValidated;
            }

            set
            {
                trajectoryValidated = value;
                OnPropertyChanged("TrajectoryValidated");
            }
        }

        public RobotState RobotState
        {
            get
            {
                return robotState;
            }

            set
            {
                robotState = value;
                OnPropertyChanged("RobotState");
            }
        }

        public DescartesCoordinatesItem SelectedTrajectoryCoordinatesItem
        {
            get
            {
                return selectedTrajectoryCoordinatesItem;
            }
            set
            {
                selectedTrajectoryCoordinatesItem = value;
                OnPropertyChanged("SelectedTrajectoryCoordinatesItem");
            }
        }

        public uint Velocity
        {
            get
            {
                return velocity;
            }
            set
            {
                velocity = value;
                Robot.Settings.Velocity = velocity;
                OnPropertyChanged("Velocity");
            }
        }

        public uint Acceleration
        {
            get
            {
                return acceleration;
            }
            set
            {
                acceleration = value;
                Robot.Settings.Acceleration = acceleration;
                OnPropertyChanged("Acceleration");
            }
        }

        public CommunicationState CommunicationState
        {
            get
            {
                return communicationState;
            }
            set
            {
                communicationState = value;
                OnPropertyChanged("CommunicationState");
            }
        }

        private void OnControllerRobotTrajectoryValidated(object sender, TrajectoryValidatedEventArguments e)
        {
            TrajectoryValidated = e.ValidationResult;

            if (!TrajectoryValidated)
            {
                MessageBox.Show(string.Format("Trajectroy validation error: {0}", e.ValidationMessage), "Trajectroy validation error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private bool CanDeleteSelectedTrajectoryCoordinatesItem
        {
            get { return SelectedTrajectoryCoordinatesItem != null; }
        }

        private void UploadRobotSettings()
        {
            Robot.UploadTrajectory(TrajectoryCoordinates);
        }

        private void StartStopRobot(object state)
        {
            if ((bool)state)
            {
                StartRobot();
            }
            else
            {
                StopRobot();
            }
        }

        private void StartRobot()
        {
            Robot.Controller.Start();
            RobotState = Robot.Controller.State;
        }

        public void StopRobot()
        {
            Robot.Controller.Stop();
            RobotState = Robot.Controller.State;
        }

        private void ResetRobot()
        {
            Robot.Controller.Reset();
            RobotState = Robot.Controller.State;
        }

        internal uint AddTrajectoryCoordinatesItem(DescartesCoordinatesItem descartesCoordinatesItem)
        {
            TrajectoryCoordinates.Add(descartesCoordinatesItem);
            return (uint)TrajectoryCoordinates.Count;
        }

        private void DeleteTrajectoryCoordinatesItem(DescartesCoordinatesItem descartesCoordinatesItem)
        {
            TrajectoryCoordinates.Remove(descartesCoordinatesItem);
        }

        private void AddDestinationPoint()
        {
            var addDestinationPointView = new AddDestinationPointView(this);
            addDestinationPointView.Show();
        }

        private void TrajectoryDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            DataGrid grid = (DataGrid)sender;
            grid.CommitEdit(DataGridEditingUnit.Row, true);
        }

        private void Controller_MonitoringServiceClientStateChanged(object sender, EventArgs e)
        {
            CommunicationState = Robot.Controller.GetWarehouseRobotMonitoringSeviceConnectionState();
        }
    }
}
