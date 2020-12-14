using System;
using System.Diagnostics;
using TRobot.Data.Contexts;
using TRobot.Data.Entities;
using TRobot.ECU.UI.Views;

namespace TRobot.ECU.UI.ViewModels
{
    class DataViewModel : BaseViewModel<DataView>
    {
        public DataViewModel()
        {
            using (var robotDatabaseContext = new RobotDatabaseContext())
            {
                try
                {
                    var robots = robotDatabaseContext.Robots;
                }
                catch (Exception ex)
                {
                    var x = ex.InnerException;
                    Debug.WriteLine("Inner Exception: {0}", x);
                    throw;
                }
            }
        }
    }
}
