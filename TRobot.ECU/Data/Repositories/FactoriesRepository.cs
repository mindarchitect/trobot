using System.Data.Entity;
using System.Threading.Tasks;
using TRobot.Core.Data.Entities;
using TRobot.Data;
using System.Linq;
using TRobot.Core.Data.Repositories;

namespace TRobot.ECU.Data.Repositories
{
    public class FactoriesRepository : EFRepository<FactoryEntity>, IFactoriesRepository
    {
        public FactoriesRepository(DbContext context) : base(context)
        {
        }

        public Task<FactoryEntity> GetFactoryById(int id)
        {
            return Context.Set<FactoryEntity>()
                    .Where(f => f.Id == id)
                    .Include(f => f.Robots).FirstOrDefaultAsync();
        }
    }
}
