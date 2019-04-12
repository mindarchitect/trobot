using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using TRobot.Core;
using TRobot.Core.UI.Commands;
using TRobot.ECU.Models;
using TRobot.ECU.UI.ViewModels;

namespace TRobot.Robots.ViewModels
{
    public class WarehouseRobotControlPanelViewModel : BaseViewModel
    {
        public ObservableCollection<DescartesCoordinatesItem> TrajectoryCoordinates { get; set; }
        public uint Velocity { get; set; }
        public uint Acceleration { get; set; }

        public WarehouseRobot Robot { get; private set; }

        private bool trajectoryValidated;                                          

        public WarehouseRobotControlPanelViewModel(WarehouseRobot robot)
        {
            Robot = robot;
            Robot.Controller.TrajectoryValidated += OnControllerRobotTrajectoryValidated;
                 
            TrajectoryCoordinates = new ObservableCollection<DescartesCoordinatesItem>();
            TrajectoryCoordinates.Add(new DescartesCoordinatesItem(0, 0, 0));
            TrajectoryCoordinates.Add(new DescartesCoordinatesItem(1, 30, 80));
            TrajectoryCoordinates.Add(new DescartesCoordinatesItem(2, 50, 10));
            TrajectoryCoordinates.Add(new DescartesCoordinatesItem(3, 120, 200));

            Velocity = 30;
            Acceleration = 1;
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

        private ICommand startPause;

        public ICommand StartPause
        {
            get
            {
                startPause = new RelayCommand<object>(StartPauseRobot);                                
                return startPause;
            }
        }

        private ICommand stop;

        public ICommand Stop
        {
            get
            {
                if (stop == null)
                {
                    stop = new RelayCommand<object>(
                        param => StopRobot()
                    );
                }
                return stop;
            }
        }       

        private void UploadRobotSettings()
        {
            Robot.UploadTrajectory(TrajectoryCoordinates);
            Robot.Acceleration = Acceleration;
            Robot.Velocity = Velocity;
        }

        private void StartRobot()
        {
            Robot.Controller.Start();
        }

        private void StopRobot()
        {
            Robot.Controller.Stop();            
        }

        private void PauseRobot()
        {
            Robot.Controller.Pause();
        }

        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            Robot.Controller.Stop();
        }

        private void StartPauseRobot(object state)
        {
            if ((bool)state)
            {
                StartRobot();
            }
            else
            {
                PauseRobot();
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
    }
}
