using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using TRobot.Core.Data.Entities;
using TRobot.Core.Data.Repositories;
using TRobot.Data;

namespace TRobot.ECU.Data.Repositories
{
    public class UsersRepository : EFRepository<UserEntity>, IUsersRepository
    {
        public UsersRepository(DbContext context) : base(context)
        {
        }

        public Task<UserEntity> GetUserByUserName(string userName)
        {
            return Context.Set<UserEntity>()
                    .Where(u => u.UserName == userName)
                    .Include(u => u.Roles)
                    .FirstOrDefaultAsync();
        }
    }
}
