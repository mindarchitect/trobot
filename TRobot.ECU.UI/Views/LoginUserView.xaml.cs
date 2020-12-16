using System.Windows;
using TRobot.Core;
using TRobot.ECU.UI.ViewModels;

namespace TRobot.ECU.UI.Views
{
    /// <summary>
    /// Interaktionslogik für LoginUserView.xaml
    /// </summary>
    public partial class LoginUserView : Window
    {
        public LoginUserView()
        {
            InitializeComponent();

            var loginUserViewModel = DependencyInjector.Resolve<LoginUserViewModel>();
            loginUserViewModel.View = this;

            Loaded += loginUserViewModel.OnLoaded;

            DataContext = loginUserViewModel;
        }
    }
}
