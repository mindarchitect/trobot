using System.Data.Entity;
using System.IO;

namespace TRobot.Data.Contexts
{
    public class ApplicationDatabaseContext : DbContext
    {
        public ApplicationDatabaseContext() : base("trobot")
        {
            if (!Database.CreateIfNotExists())
            {
                string script = File.ReadAllText(@".\..\..\sql\trobot.sql");
                //Database.ExecuteSqlCommand(script);
            }    
        }
    }
}
