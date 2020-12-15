﻿using TRobot.Core.Services;
using TRobot.ECU.UI.Views;
using Unity;

namespace TRobot.ECU.UI.ViewModels
{
    class DataViewModel : BaseViewModel<DataView>
    {
        [Dependency]
        public IFactoryService FactoryService { get; set; }

        public DataViewModel(IFactoryService factoryService)
        {
            FactoryService = factoryService;
            var factory = FactoryService.GetFactoryById(1);
            var robots = factory.Robots;
        }
    }
}
