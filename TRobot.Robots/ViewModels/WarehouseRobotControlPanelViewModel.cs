﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TRobot.Core;
using TRobot.Core.Enums;
using TRobot.Core.UI.Commands;
using TRobot.ECU.Models;
using TRobot.ECU.UI.ViewModels;

namespace TRobot.Robots.ViewModels
{
    public class WarehouseRobotControlPanelViewModel : BaseViewModel
    {
        public ObservableCollection<DescartesCoordinatesItem> TrajectoryCoordinates { get; set; }

        private DescartesCoordinatesItem selectedTrajectoryCoordinatesItem;               

        public WarehouseRobot Robot { get; set; }

        private bool trajectoryValidated;
        private RobotState robotState;

        private ICommand startStopCommand;
        private ICommand uploadSettingsCommand;
        private ICommand resetCommand;
        private ICommand deleteSelectedTrajectoryCoordinatesItemCommand;

        private uint velocity;
        private uint acceleration;

        public WarehouseRobotControlPanelViewModel(WarehouseRobot robot)
        {
            Robot = robot;
            Robot.Controller.TrajectoryValidated += OnControllerRobotTrajectoryValidated;
               
            // Just for testing  
            TrajectoryCoordinates = new ObservableCollection<DescartesCoordinatesItem>();
            TrajectoryCoordinates.Add(new DescartesCoordinatesItem(0, 0, 0));
            TrajectoryCoordinates.Add(new DescartesCoordinatesItem(1, 30, 80));
            TrajectoryCoordinates.Add(new DescartesCoordinatesItem(2, 50, 10));
            TrajectoryCoordinates.Add(new DescartesCoordinatesItem(3, 120, 200));            

            Velocity = 30;
            Acceleration = 2;

            RobotState = Robot.Controller.State;
        }

        private void OnControllerRobotTrajectoryValidated(object sender, TrajectoryValidatedEventArguments e)
        {
            TrajectoryValidated = e.ValidationResult; 
            
            if (TrajectoryValidated)
            {
                Robot.Controller.SetupTrajectory();
            }
            else
            {
                MessageBox.Show(string.Format("Trajectroy validation error: {0}", e.ValidationMessage), "Trajectroy validation error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
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
                    deleteSelectedTrajectoryCoordinatesItemCommand = new RelayCommand<object>(param => DeleteTrajectoryCoordinatesItem((DescartesCoordinatesItem)param), param => CanDeleteSelectedTrajectoryCoordinatesItem);
                }
                return deleteSelectedTrajectoryCoordinatesItemCommand;
            }
        }


        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            StopRobot();
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

        private bool CanDeleteSelectedTrajectoryCoordinatesItem
        {
            get { return SelectedTrajectoryCoordinatesItem != null; }
        }

        private void UploadRobotSettings()
        {
            Robot.UploadTrajectory(TrajectoryCoordinates);            
        }

        private void StartRobot()
        {
            Robot.Controller.Start();
            RobotState = Robot.Controller.State;
        }

        private void StopRobot()
        {
            Robot.Controller.Stop();
            RobotState = Robot.Controller.State;
        }

        private void ResetRobot()
        {            
            Robot.Controller.Reset();
            RobotState = Robot.Controller.State;
        }

        private void DeleteTrajectoryCoordinatesItem(DescartesCoordinatesItem descartesCoordinatesItem)
        {
            TrajectoryCoordinates.Remove(descartesCoordinatesItem);
        }

        private void TrajectoryDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {            
            DataGrid grid = (DataGrid)sender;
            grid.CommitEdit(DataGridEditingUnit.Row, true);                        
        }
    }
}
