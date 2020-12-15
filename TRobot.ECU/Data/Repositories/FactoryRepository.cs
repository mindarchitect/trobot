using System.Data.Entity;
using TRobot.Core.Data.Entities;
using TRobot.Data;

namespace TRobot.ECU.Data.Repositories
{
    public class FactoryRepository : EFRepository<FactoryEntity>
    {
        public FactoryRepository(DbContext context) : base(context)
        {
        }
    }
}
