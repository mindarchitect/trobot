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
            using (var db = new RobotContext())
            {
                var robotEntity = new RobotEntity { Id = 0, Name = "Warehouse Robot 1", Guid = Guid.NewGuid().ToString() };
                db.Robots.Add(robotEntity);
                try
                {
                    db.SaveChanges();
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
