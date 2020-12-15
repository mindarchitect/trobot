using TRobot.Core.Data.Entities;

namespace TRobot.Core.Services
{
    public interface IFactoryService : IService
    {
        FactoryEntity GetFactoryById(int Id);
    }
}
