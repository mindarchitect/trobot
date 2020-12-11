using System;
using System.Windows;
using System.Windows.Media.Imaging;
using TRobot.Core;
using TRobot.Core.UI.ViewModels;
using TRobot.ECU.UI.ViewModels;

namespace TRobot.ECU.UI.Views
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class DashboardView : Window, IView
    {        
        public DashboardView()
        {
            InitializeComponent();

            DependencyInjector.AddExtension<ECUDependencyInjectionExtension>();
            DependencyInjector.RegisterType<DashboardViewControlModel, DashboardViewControlModel>();

            var dashboardViewControlModel = DependencyInjector.Resolve<DashboardViewControlModel>();            
            Closing += dashboardViewControlModel.OnClosing;

            DataContext = dashboardViewControlModel;

            Uri iconUri = new Uri("pack://application:,,,/Images/robot.png", UriKind.RelativeOrAbsolute);
            Icon = BitmapFrame.Create(iconUri);
        }        
    }
}
