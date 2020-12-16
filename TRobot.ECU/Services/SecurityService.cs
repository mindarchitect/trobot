using System;
using System.Threading.Tasks;
using TRobot.Core.Services;

namespace TRobot.ECU.Services
{
    public class SecurityService : ISecurityService
    {
        public Task<bool> LoginUser(string username, string password)
        {
            throw new NotImplementedException();
        }
    }
}
