using System.Data.Entity;
using TRobot.Core.Data.Entities;
using TRobot.Data;

namespace TRobot.Core.Tests.Data
{
    class TestAsyncRepository : EFRepository<FactoryEntity>
    {
        public TestAsyncRepository(DbContext context) : base(context)
        {
        }
    }
}
