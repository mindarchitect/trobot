using NUnit.Framework;
using System.Data.Entity;
using TRobot.Core.Data;
using TRobot.Core.Data.Entities;
using TRobot.Core.Tests.Data;
using TRobot.Data.Contexts;

namespace TRobot.Core.Tests
{
    [TestFixture]
    public class FactoriesAsyncRepositoryTest
    {
        [SetUp]
        public void SetUp()
        {
            DependencyInjector.RegisterType<DbContext, TestDatabaseContext>();
            DependencyInjector.RegisterType<IAsyncRepository<FactoryEntity>, TestFactoriesAsyncRepository>();
        }

        [Test]
        public void GetByIdTest()
        {
            var testRepository = DependencyInjector.Resolve<IAsyncRepository<FactoryEntity>>();
            Assert.IsNotNull(testRepository);
            var factory = testRepository.GetById(1);
            factory.Wait();
            Assert.IsNotNull(factory);
            Assert.IsInstanceOf<FactoryEntity>(factory.Result);

            var factoryEntity = factory.Result;

            // Test resulting object
            Assert.AreEqual(1, factoryEntity.Id);
            Assert.AreEqual("Warehouse factory", factoryEntity.Name);
        }
    }
}
