using NUnit.Framework;
using System.Data.Entity;
using TRobot.Core;
using TRobot.Core.Data.Entities;
using TRobot.Core.Data.Repositories;
using TRobot.ECU.Data.Repositories;

namespace TRobot.ECU.Tests.Data.Repositories
{
    [TestFixture]
    public class FactoriesAsyncRepositoryTest
    {
        [SetUp]
        public void SetUp()
        {
            DependencyInjector.RegisterType<DbContext, TestDatabaseContext>();
            DependencyInjector.RegisterType<IFactoriesRepository, FactoriesRepository>();
        }

        [Test]
        public void GetByIdTest()
        {
            var testRepository = DependencyInjector.Resolve<IFactoriesRepository>();
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
