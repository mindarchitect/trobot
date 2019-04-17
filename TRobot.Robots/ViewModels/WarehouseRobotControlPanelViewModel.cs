using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
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
        public uint Velocity
        {
            get;
            set;
        }
        public uint Acceleration { get; set; }       

        public WarehouseRobot Robot { get; private set; }

        private bool trajectoryValidated;
        private RobotState robotState;

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

            Robot.Settings.Velocity = 30;
            Robot.Settings.Acceleration = 1;

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

        private ICommand uploadSettings;

        public ICommand UploadSettings
        {
            get
            {
                if (uploadSettings == null)
                {
                    uploadSettings = new RelayCommand<object>(
                        param => UploadRobotSettings()
                    );
                }
                return uploadSettings;
            }
        }

        private ICommand startStop;

        public ICommand StartStop
        {
            get
            {
                startStop = new RelayCommand<object>(StartStopRobot);                                
                return startStop;
            }
        }

        private ICommand reset;

        public ICommand Reset
        {
            get
            {
                if (reset == null)
                {
                    reset = new RelayCommand<object>(
                        param => ResetRobot()
                    );
                }
                return reset;
            }
        }       

        private void UploadRobotSettings()
        {
            Robot.UploadTrajectory(TrajectoryCoordinates);
            Robot.Settings.Acceleration = Acceleration;
            Robot.Settings.Velocity = Velocity;
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
            StopRobot();
            Robot.Controller.Reset();
            RobotState = Robot.Controller.State;
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
    }
}
