using TRobot.Core.Services;
using TRobot.Core.Data.Entities;
using Unity;
using System.Threading.Tasks;
using TRobot.Core.Data.Repositories;

namespace TRobot.ECU.Services
{
    public class FactoriesService : IFactoriesService
    {
        [Dependency]
        public IFactoriesRepository<FactoryEntity> FactoriesRepository { get; set; }

        public FactoriesService()
        {
        }

        public async Task<FactoryEntity> GetFactoryById(int id)
        {
            return await FactoriesRepository.GetFactoryById(id);
        }
    }
}
