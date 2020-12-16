using System.Threading.Tasks;
using TRobot.Core.Data.Entities;

namespace TRobot.Core.Data.Repositories
{
    public interface IUsersRepository : IAsyncRepository<UserEntity>
    {
        Task<UserEntity> GetUserByUserName(string userName);
    }
}
