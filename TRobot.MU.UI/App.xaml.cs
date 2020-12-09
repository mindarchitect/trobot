using System.Windows;
using TRobot.Core;
using TRobot.MU;
using TRobot.MU.UI.ViewModels;

namespace MonitoringUnit.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            DependencyInjector.AddExtension<ManagementUnitDependencyInjectionExtension>();
            DependencyInjector.Register<MainWindowViewModel, MainWindowViewModel>();
        }
    }
}
