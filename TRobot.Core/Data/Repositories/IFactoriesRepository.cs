using System.Threading.Tasks;
using TRobot.Core.Data.Entities;

namespace TRobot.Core.Data.Repositories
{
    public interface IFactoriesRepository<T> : IAsyncRepository<T> where T : FactoryEntity
    {
        Task<T> GetFactoryById(int id);
    }
}
