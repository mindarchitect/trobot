using System.Windows.Input;
using TRobot.Core.UI.Commands;
using TRobot.Core.UI.Models;
using TRobot.ECU.UI.ViewModels;
using TRobot.Robots.Views;

namespace TRobot.Robots.ViewModels
{
    class AddDestinationPointViewModel : BaseViewModel<AddDestinationPointView>
    {  
        private ICommand okCommand;
        private ICommand cancelCommand;

        private uint step;
        private uint x;
        private uint y;

        private WarehouseRobotControlPanelViewModel warehouseRobotControlPanelViewModel;

        internal AddDestinationPointViewModel(AddDestinationPointView addDestinationPointView, WarehouseRobotControlPanelViewModel warehouseRobotControlPanelViewModel) : base(addDestinationPointView)
        {
            this.warehouseRobotControlPanelViewModel = warehouseRobotControlPanelViewModel;
            Step = (uint) warehouseRobotControlPanelViewModel.TrajectoryCoordinates.Count;
        }

        public ICommand OkCommand
        {
            get
            {
                if (okCommand == null)
                {
                    okCommand = new RelayCommand<object>(param => OK());
                }
                return okCommand;
            }
        }

        public ICommand CancelCommand
        {
            get
            {
                if (cancelCommand == null)
                {
                    cancelCommand = new RelayCommand<object>(param => Cancel());
                }
                return cancelCommand;
            }
        }

        public uint Step
        {
            get
            {
                return step;
            }
            set
            {
                step = value;
                OnPropertyChanged("Step");
            }
        }

        public uint X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
                OnPropertyChanged("X");
            }
        }

        public uint Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
                OnPropertyChanged("Y");
            }
        }

        private void OK()
        {
            Step = warehouseRobotControlPanelViewModel.AddTrajectoryCoordinatesItem(new DescartesCoordinatesItem(step, x, y));
        }

        private void Cancel()
        {
            View.Close();
        }
    }
}
