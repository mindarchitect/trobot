using System.Data.Entity;
using System.IO;
using TRobot.Core.Data.Entities;

namespace TRobot.Data.Contexts
{
    public class TRobotDatabaseContext : DbContext
    {
        public TRobotDatabaseContext() : base("trobot")
        {
            string sqliteFilePath = @".\trobot.sqlite";

            if (!File.Exists(sqliteFilePath))
            {
                if (!Database.CreateIfNotExists())
                {
                    string sqlFilePath = @"./../../sql/trobot.sql";
                    string script = File.ReadAllText(sqlFilePath);
                    Database.ExecuteSqlCommand(script);
                }
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FactoryEntity>().ToTable("Factories");
            modelBuilder.Entity<RobotEntity>().ToTable("Robots");
        }
    }
}
