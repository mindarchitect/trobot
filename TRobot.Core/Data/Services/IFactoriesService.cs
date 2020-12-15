using System.Threading.Tasks;
using TRobot.Core.Data.Entities;

namespace TRobot.Core.Services
{
    public interface IFactoriesService : IService
    {
        Task<FactoryEntity> GetFactoryById(int id);
    }
}
