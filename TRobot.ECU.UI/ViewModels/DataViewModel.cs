using System.Windows.Input;
using TRobot.Core.Services;
using TRobot.Core.UI.Commands;
using TRobot.ECU.UI.Views;
using Unity;

namespace TRobot.ECU.UI.ViewModels
{
    class DataViewModel : BaseViewModel<DataView>
    {
        [Dependency]
        public IFactoriesService FactoriesService { get; set; }

        public DataViewModel()
        {
        }

        private ICommand getDataCommand;
        public ICommand GetDataCommand
        {
            get
            {
                if (getDataCommand == null)
                {
                    getDataCommand = new RelayCommand<object>(GetData);
                }

                return getDataCommand;
            }
        }

        internal async void GetData(object param)
        {
            var factory = await FactoriesService.GetFactoryById(1);
            var factoryRobots = factory.Robots;
        }
    }
}
