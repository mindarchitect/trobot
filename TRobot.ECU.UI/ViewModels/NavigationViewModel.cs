using System.Windows.Input;

namespace TRobot.ECU.UI.ViewModels
{
    public class NavigationViewModel : BaseViewModel
    {        
        private AddRobotViewControlModel addRobotViewControlModel;

        public ICommand AddRobotCommand { get; internal set; }        

        private object selectedViewModel;

        public object SelectedViewModel
        {
            get
            {
                return selectedViewModel;
            }

            set
            {
                selectedViewModel = value;
                OnPropertyChanged("SelectedViewModel");
            }
        }


        public NavigationViewModel()
        {            
        }

        private void AddRobot(object obj)
        {
            if (addRobotViewControlModel == null)
            {
                addRobotViewControlModel = new AddRobotViewControlModel();
            }

            SelectedViewModel = addRobotViewControlModel;
        }       
    }
}
