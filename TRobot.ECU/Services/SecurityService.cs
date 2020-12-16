using System;
using System.Threading.Tasks;
using TRobot.Core.Data.Entities;
using TRobot.Core.Data.Repositories;
using TRobot.Core.Services;
using Unity;

namespace TRobot.ECU.Services
{
    public class SecurityService : ISecurityService
    {
        [Dependency]
        public IUsersRepository UsersRepository { get; set; }

        public async Task<UserEntity> LoginUser(string username, string password)
        {
            var userEntity = await UsersRepository.GetUserByUserName(username);
            return userEntity;
        }
    }
}
