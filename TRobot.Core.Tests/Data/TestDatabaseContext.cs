using System.Data.Entity;
using System.IO;
using TRobot.Core.Data.Entities;

namespace TRobot.Data.Contexts
{
    public class TestDatabaseContext : DbContext
    {
        public TestDatabaseContext() : base("test")
        {
            string sqliteFilePath = @".\TRobot.Core.Tests\bin\Debug\test.sqlite";

            if (!File.Exists(sqliteFilePath))
            {
                if (!Database.CreateIfNotExists())
                {
                    string sqlFilePath = @".\TRobot.Core.Tests\sql\trobot.sql";
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
