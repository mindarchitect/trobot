using System.Windows;
using TRobot.ECU.UI.ViewModels;

namespace TRobot.ECU.UI.Views
{
    /// <summary>
    /// Interaction logic for AddFactoryView.xaml
    /// </summary>
    public partial class AddFactoryView : Window
    {
        public AddFactoryView()
        {
            InitializeComponent();

            DataContext = new AddFactoryViewModel(this);
        }
    }
}
