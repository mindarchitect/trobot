using System.Data.Entity;
using TRobot.Data.Entities;

namespace TRobot.Data.Contexts
{
    public class RobotContext : DbContext
    {
        public RobotContext() : base("trobot")
        {
            Database.CreateIfNotExists();
        }

        public DbSet<RobotEntity> Robots { get; set; }
    }
}
