using System.Threading.Tasks;
using TRobot.Core.Data.Repositories;
using TRobot.Core.Services;
using TRobot.Core.Services.Models;
using Unity;

namespace TRobot.ECU.Services
{
    public class SecurityService : ISecurityService
    {
        [Dependency]
        public IUsersRepository UsersRepository { get; set; }

        public async Task<ServiceResponse> LoginUser(string username, string password)
        {
            var userEntity = await UsersRepository.GetUserByUserName(username);

            ServiceResponse serviceResponse = new ServiceResponse();

            if (userEntity == null)
            {
                serviceResponse.ErrorMessage = "User is not found";
                return serviceResponse;
            }

            if (!userEntity.Password.Equals(password))
            {
                serviceResponse.ErrorMessage = "Password is incorrect";
                return serviceResponse;
            }

            serviceResponse.Result = userEntity;
            return serviceResponse;
        }
    }
}
