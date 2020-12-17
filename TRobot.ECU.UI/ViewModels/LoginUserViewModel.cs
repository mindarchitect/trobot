﻿using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using TRobot.Core.Data.Entities;
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

        private string userName;
        private string password;

        public LoginUserViewModel()
        {            
        }

        public string UserName
        {
            get
            {
                return userName;
            }
            set
            {
                userName = value;
                OnPropertyChanged("Username");
            }
        }

        private ICommand okCommand;
        public ICommand OKCommand
        {
            get
            {
                if (okCommand == null)
                {
                    okCommand = new RelayCommand<object>(OK);
                }

                return okCommand;
            }
        }

        private ICommand cancelCommand;
        public ICommand CancelCommand
        {
            get
            {
                if (cancelCommand == null)
                {
                    cancelCommand = new RelayCommand<object>(Cancel);
                }

                return cancelCommand;
            }
        }

        internal void OnLoaded(object sender, EventArgs e)
        {
            View.PasswordBox.PasswordChanged += LocalPasswordBox_PasswordChanged;
        }

        private void LocalPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            password = View.PasswordBox.Password;
        }

        private async void OK(object param)
        {
           var result = await SecurityService.LoginUser(userName, password);

            if (result.ErrorMessage != null)
            {
                MessageBoxResult messageBoxResult = MessageBox.Show(result.ErrorMessage, "Error", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation);
            }
            else
            {
                var userEntity = (UserEntity) result.Result;
                var userRoles = string.Join(", ", userEntity.Roles.Select(role => role.ToString()).ToArray());
                MessageBoxResult messageBoxResult = MessageBox.Show(String.Format("User {0} is successfuly authenticated with roles:\n{1}", userEntity.UserName, userRoles), "Success", MessageBoxButton.OKCancel, MessageBoxImage.Information);
            }
        }

        private void Cancel(object param)
        {
            View.Close();
        }
    }
}
