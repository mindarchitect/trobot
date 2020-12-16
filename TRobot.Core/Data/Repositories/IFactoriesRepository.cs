using System.Threading.Tasks;
using TRobot.Core.Data.Entities;

namespace TRobot.Core.Data.Repositories
{
    public interface IFactoriesRepository : IAsyncRepository<FactoryEntity>
    {
        Task<FactoryEntity> GetFactoryById(int id);
    }
}
