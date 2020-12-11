using System;
using System.Windows;
using System.Windows.Media.Imaging;
using TRobot.Core;
using TRobot.MU.UI.ViewModels;

namespace MonitoringUnit.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var mainWindowViewModel = DependencyInjector.Resolve<MainWindowViewModel>();
            Closing += mainWindowViewModel.OnWindowClosing;

            DataContext = mainWindowViewModel;

            Uri iconUri = new Uri("pack://application:,,,/Images/monitor.jpg", UriKind.RelativeOrAbsolute);
            Icon = BitmapFrame.Create(iconUri);
        }
    }
}
