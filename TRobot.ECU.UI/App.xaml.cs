using System.Windows;
using TRobot.Core;
using TRobot.Core.Services;
using TRobot.Data.Contexts;
using TRobot.ECU.Services;
using TRobot.ECU.UI.ViewModels;
using Unity.Injection;

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

            DependencyInjector.RegisterType<DataViewModel, DataViewModel>();
            DependencyInjector.RegisterType<AddFactoryViewModel, AddFactoryViewModel>();
            DependencyInjector.RegisterType<AddRobotViewModel, AddRobotViewModel>();

            DependencyInjector.RegisterType<DashboardViewModel, DashboardViewModel>();

            using (new TRobotDatabaseContext())
            {
            }
        }
    }
}
