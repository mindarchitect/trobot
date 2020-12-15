using System.Data.Entity;
using TRobot.Core.Data.Entities;

namespace TRobot.Data.Contexts
{
    public class FactoriesDatabaseContext : DbContext
    {
        public FactoriesDatabaseContext() : base("trobot")
        {
        }

        public DbSet<FactoryEntity> Factories { get; set; }
    }
}
