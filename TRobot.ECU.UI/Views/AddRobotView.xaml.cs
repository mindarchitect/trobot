using System.Windows;
using TRobot.ECU.UI.ViewModels;

namespace TRobot.ECU.UI.Views
{
    /// <summary>
    /// Interaction logic for AddRobot.xaml
    /// </summary>
    public partial class AddRobotView : Window
    {
        public AddRobotView()
        {
            InitializeComponent();
            DataContext = new AddRobotViewModel(this);
        }
    }
}
