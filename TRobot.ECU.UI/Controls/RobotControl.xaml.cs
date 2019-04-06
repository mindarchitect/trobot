using System.Windows;
using System.Windows.Controls;

namespace TRobot.ECU.UI.Controls
{
    /// <summary>
    /// AbstractRobot control
    /// </summary>
    public partial class RobotControl : UserControl
    {
        public string StartImage
        {
            get { return @"~\..\..\Images\start_128x128.png"; }
        }

        public RobotControl()
        {
            InitializeComponent();
        }       
    }
}
