using System;
using System.Windows;
using System.Windows.Input;
using TRobot.Core.Services;
using TRobot.Core.UI.Commands;
using TRobot.ECU.UI.Views;
using Unity;

namespace TRobot.ECU.UI.ViewModels
{
    class LoginUserViewModel : BaseViewModel<LoginUserView>
    {
        [Dependency]
        public ISecurityService SecurityService { get; set; }

        private string username;
        private string password;

        public LoginUserViewModel()
        {
            
        }

        public string Username
        {
            get
            {
                return username;
            }
            set
            {
                username = value;
                OnPropertyChanged("Username");
            }
        }

        private ICommand loginUserCommand;
        public ICommand LoginUserCommand
        {
            get
            {
                if (loginUserCommand == null)
                {
                    loginUserCommand = new RelayCommand<object>(LoginUser);
                }

                return loginUserCommand;
            }
        }

        internal void OnLoaded(object sender, EventArgs e)
        {
            //View.PasswordBox.PasswordChanged += LocalPasswordBox_PasswordChanged;
        }

        internal async void LoginUser(object param)
        {
            var result = await SecurityService.LoginUser(username, password);            
        }

        private void LocalPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            //password = View.LocalPasswordBox.Password;
        }
    }
}
