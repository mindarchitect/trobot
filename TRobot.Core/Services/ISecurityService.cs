using System.Threading.Tasks;
using TRobot.Core.Data.Entities;

namespace TRobot.Core.Services
{
    public interface ISecurityService
    {
        Task<UserEntity> LoginUser(string username, string password);
    }
}
