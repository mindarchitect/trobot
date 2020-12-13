using System.Windows;
using TRobot.Core;
using TRobot.MU.UI.ViewModels;

namespace TRobot.MU.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            DependencyInjector.AddExtension<MUDependencyInjectionExtension>();            
            DependencyInjector.RegisterType<MainWindowViewModel, MainWindowViewModel>();
        }
    }
}
