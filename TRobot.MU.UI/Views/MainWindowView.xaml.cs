using System;
using System.Windows;
using System.Windows.Media.Imaging;
using TRobot.Core;
using TRobot.MU.UI.ViewModels;

namespace TRobot.MU.UI.Views
{
    /// <summary>
    /// Interaction logic for MainWindowView.xaml
    /// </summary>
    public partial class MainWindowView : Window
    {
        public MainWindowView()
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
