using System;
using System.Windows;
using System.Windows.Media.Imaging;
using TRobot.Core;
using TRobot.ECU.UI.ViewModels;

namespace TRobot.ECU.UI.Views
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class DashboardView : Window
    {        
        public DashboardView()
        {
            InitializeComponent();

            var dashboardViewControlModel = DependencyInjector.Resolve<DashboardViewModel>();
            dashboardViewControlModel.View = this;

            Closing += dashboardViewControlModel.OnClosing;
            Initialized += dashboardViewControlModel.OnInitialized;

            DataContext = dashboardViewControlModel;

            Uri iconUri = new Uri("pack://application:,,,/Images/robot.png", UriKind.RelativeOrAbsolute);
            Icon = BitmapFrame.Create(iconUri);
        }
    }
}
