using TRobot.Core.Services;
using TRobot.Core.Data.Entities;
using Unity;
using System.Threading.Tasks;
using TRobot.Core.Data;

namespace TRobot.ECU.Services
{
    public class FactoryService : IFactoryService
    {
        [Dependency]
        public IAsyncRepository<FactoryEntity> FactoriesRepository { get; set; }

        public FactoryService()
        {
        }

        public async Task<FactoryEntity> GetFactoryById(int id)
        {
            return await FactoriesRepository.GetById(id);
        }
    }
}
