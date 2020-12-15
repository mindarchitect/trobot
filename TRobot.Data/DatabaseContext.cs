using System.Data.Entity;

namespace TRobot.Data
{
    abstract class DatabaseContext : DbContext
    {
        protected DatabaseContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
        }
    }
}
