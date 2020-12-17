using System.Threading.Tasks;
using TRobot.Core.Services.Models;

namespace TRobot.Core.Services
{
    public interface ISecurityService
    {
        Task<ServiceResponse> LoginUser(string username, string password);
    }
}
