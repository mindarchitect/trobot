using System.Collections.ObjectModel;
using System.Windows.Input;
using TRobot.Core;
using TRobot.Core.UI.Commands;
using TRobot.ECU.Models;

namespace TRobot.Robots.ViewModels
{
    public class WarehouseRobotControlPanelViewModel
    {
        public ObservableCollection<DescartesCoordinatesItem> TrajectoryCoordinates { get; private set; }
        public uint Velocity { get; set; }        

        public WarehouseRobot Robot { get; private set; }

        public bool TrajectoryValidated { get; private set; } = false;

        public WarehouseRobotControlPanelViewModel(WarehouseRobot robot)
        {
            Robot = robot;
            Robot.Controller.TrajectoryValidated += Controller_TrajectoryValidated;
                 
            TrajectoryCoordinates = new ObservableCollection<DescartesCoordinatesItem>();
            TrajectoryCoordinates.Add(new DescartesCoordinatesItem(0, 0, 0));
            TrajectoryCoordinates.Add(new DescartesCoordinatesItem(1, 10, 10));
            TrajectoryCoordinates.Add(new DescartesCoordinatesItem(2, 20, 20));
            TrajectoryCoordinates.Add(new DescartesCoordinatesItem(3, 40, 100));

            Velocity = 10;
        }

        private void Controller_TrajectoryValidated(object sender, TrajectoryValidatedEventArguments e)
        {
            TrajectoryValidated = e.ValidationResult;
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

        private ICommand move;

        public ICommand Move
        {
            get
            {
                if (move == null)
                {
                    move = new RelayCommand<object>(
                        param => MoveRobot()
                    );
                }
                return move;
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
            Robot.Controller.UploadTrajectory(TrajectoryCoordinates);                               
        }

        private void MoveRobot()
        {
            Robot.Move();
        }

        private void StopRobot()
        {
            Robot.Stop();
        }
    }
}
