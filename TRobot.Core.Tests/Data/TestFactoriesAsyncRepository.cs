using System.Data.Entity;
using TRobot.Core.Data.Entities;
using TRobot.Data;

namespace TRobot.Core.Tests.Data
{
    class TestFactoriesAsyncRepository : EFRepository<FactoryEntity>
    {
        public TestFactoriesAsyncRepository(DbContext context) : base(context)
        {
        }
    }
}
