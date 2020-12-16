using System.Threading.Tasks;

namespace TRobot.Core.Services
{
    public interface ISecurityService
    {
        Task<bool> LoginUser(string username, string password);
    }
}
