using System.Windows;
using TRobot.Core;
using TRobot.ECU.UI.ViewModels;
using TRobot.ECU.UI.Views;

namespace TRobot.ECU.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            DependencyInjector.AddExtension<ECUDependencyInjectionExtension>();

            DependencyInjector.RegisterType<AddFactoryViewModel, AddFactoryViewModel>();
            DependencyInjector.RegisterType<AddRobotViewModel, AddRobotViewModel>();
            DependencyInjector.RegisterType<DashboardViewModel, DashboardViewModel>();
        }
    }
}
