using System.Windows;
using System.Windows.Input;
using TRobot.Core.Services;
using TRobot.Core.UI.Commands;
using TRobot.ECU.UI.Views;
using Unity;

namespace TRobot.ECU.UI.ViewModels
{
    class LoginViewModel : BaseViewModel<LoginView>
    {
        [Dependency]
        public ISecurityService SecurityService { get; set; }

        public LoginViewModel()
        {
        }

        private ICommand loginCommand;
        public ICommand LoginCommand
        {
            get
            {
                if (loginCommand == null)
                {
                    loginCommand = new RelayCommand<object>(LoginUser);
                }

                return loginCommand;
            }
        }

        private ICommand toolbarCloseCommand;
        public ICommand ToolbarCloseCommand
        {
            get
            {
                if (toolbarCloseCommand == null)
                {
                    toolbarCloseCommand = new RelayCommand<object>(ToolbarClose);
                }

                return toolbarCloseCommand;
            }
        }

        private ICommand toolbarMinimizeCommand;
        public ICommand ToolbarMinimizeCommand
        {
            get
            {
                if (toolbarMinimizeCommand == null)
                {
                    toolbarMinimizeCommand = new RelayCommand<object>(ToolbarMinimize);
                }

                return toolbarMinimizeCommand;
            }
        }

        private ICommand toolbarSystemInformationCommand;
        public ICommand ToolbarSystemInformationCommand
        {
            get
            {
                if (toolbarSystemInformationCommand == null)
                {
                    toolbarSystemInformationCommand = new RelayCommand<object>(ToolbarSystemInformation);
                }

                return toolbarSystemInformationCommand;
            }
        }

        private ICommand windowsCredentialsCheckboxCheckedCommand;
        public ICommand WindowsCredentialsCheckboxCheckedCommand
        {
            get
            {
                if (windowsCredentialsCheckboxCheckedCommand == null)
                {
                    windowsCredentialsCheckboxCheckedCommand = new RelayCommand<bool>(WindowsCredentialsCheckboxChecked);
                }

                return windowsCredentialsCheckboxCheckedCommand;
            }
        }

        private ICommand thumbOnDragDeltaCommand;
        public ICommand ThumbOnDragDeltaCommand
        {
            get
            {
                if (thumbOnDragDeltaCommand == null)
                {
                    thumbOnDragDeltaCommand = new RelayCommand<object>(ThumbOnDragDelta);
                }

                return thumbOnDragDeltaCommand;
            }
        }

        internal async void LoginUser(object param)
        {
            var result = await SecurityService.LoginUser("test", "test");            
        }

        private void ToolbarClose(object param)
        {
            Application.Current.Shutdown();
        }

        private void ToolbarMinimize(object param)
        {
            View.WindowState = WindowState.Minimized;
        }

        private void ToolbarSystemInformation(object param)
        {
            //var systemInformationWindow = new SystemInformationWindow();
            //systemInformationWindow.Show();
        }
        private void WindowsCredentialsCheckboxChecked(bool param)
        {
            if (param)
            {             
                //LocalUserNameTextBox.Text = Environment.UserName;
                //LocalPasswordBox.Password = "SHOW_SOME_PASSWORD";             
            }
            else
            {
                //LocalUserNameTextBox.Text = string.Empty;
                //LocalPasswordBox.Password = string.Empty;
            }
        }

        private void ThumbOnDragDelta(object param)
        {
            //View.Left = View.Left + e.HorizontalChange;
            //View.Top = View.Top + e.VerticalChange;
        }
    }
}
