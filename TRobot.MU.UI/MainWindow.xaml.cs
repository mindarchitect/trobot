using System.Windows;
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

            var mainWindowViewModel = new MainWindowViewModel();
            Closing += mainWindowViewModel.OnWindowClosing;

            DataContext = mainWindowViewModel;            
        }
    }
}
