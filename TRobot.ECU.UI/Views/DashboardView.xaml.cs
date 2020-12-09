using System;
using System.Windows;
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
            Closing += dashboardViewControlModel.OnWindowClosing;

            DataContext = dashboardViewControlModel;
        }        
    }
}
