using System.Linq;

using TRobot.Core.Services;
using TRobot.Core.Data.Entities;
using TRobot.Data.Contexts;
using System.Data.Entity;

namespace TRobot.ECU.Services
{
    public class FactoryService : IFactoryService
    {
        public FactoryService()
        {
        }

        public FactoryEntity GetFactoryById(int id)
        {
            using (var robotDatabaseContext = new FactoriesDatabaseContext())
            {
                var query = robotDatabaseContext.Factories
                      .Where(factory => factory.Id == id)
                      .Include(factory => factory.Robots);
                      
                return query.FirstOrDefault();
            }  
        }
    }
}
