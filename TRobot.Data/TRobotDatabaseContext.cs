using System;
using System.Data.Entity;
using System.IO;
using TRobot.Core.Data.Entities;

namespace TRobot.Data.Contexts
{
    public class TRobotDatabaseContext : DbContext
    {
        public TRobotDatabaseContext() : base("trobot")
        {
            string baseDirectoryPath = AppDomain.CurrentDomain.BaseDirectory;
            string databaseFilePath = Path.GetFullPath(Path.Combine(baseDirectoryPath, @".\trobot.sqlite"));

            if (!File.Exists(databaseFilePath))
            {
                if (!Database.CreateIfNotExists())
                {
                    string sqlFilePath = Path.GetFullPath(Path.Combine(baseDirectoryPath, @".\..\..\Scripts\trobot.sql"));
                    string script = File.ReadAllText(sqlFilePath);
                    Database.ExecuteSqlCommand(script);
                }
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FactoryEntity>().ToTable("Factories");
            modelBuilder.Entity<RobotEntity>().ToTable("Robots");

            modelBuilder.Entity<UserEntity>().ToTable("Users");
            modelBuilder.Entity<RoleEntity>().ToTable("Roles");

            modelBuilder.Entity<UserEntity>()
                .HasMany(u => u.Roles)
                .WithMany(r => r.Users)
                .Map(mc =>
                {
                    mc.MapLeftKey("UserId");
                    mc.MapRightKey("RoleId");
                    mc.ToTable("UserRoles");
                });
        }
    }
}
