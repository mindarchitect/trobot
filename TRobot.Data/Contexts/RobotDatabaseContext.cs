using System.Data.Entity;
using TRobot.Data.Entities;

namespace TRobot.Data.Contexts
{
    public class RobotDatabaseContext : DbContext
    {
        public RobotDatabaseContext() : base("trobot")
        {
        }

        public DbSet<RobotEntity> Robots { get; set; }
    }
}
