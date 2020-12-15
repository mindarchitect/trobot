using System.Data.Entity;
using System.IO;
using TRobot.Core.Data.Entities;

namespace TRobot.Data.Contexts
{
    public class TRobotDatabaseContext : DbContext
    {
        public TRobotDatabaseContext() : base("trobot")
        {
            if (!Database.CreateIfNotExists())
            {
                string sqlFilePath = @"./../../sql/trobot.sql";
                if (!File.Exists(sqlFilePath))
                {
                    string script = File.ReadAllText(sqlFilePath);
                    Database.ExecuteSqlCommand(script);
                }
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FactoryEntity>().ToTable("Factories");
        }
    }
}
