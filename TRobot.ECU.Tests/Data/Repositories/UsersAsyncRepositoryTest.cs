using NUnit.Framework;
using System.Data.Entity;
using TRobot.Core;
using TRobot.Core.Data.Entities;
using TRobot.Core.Data.Repositories;
using TRobot.Data.Repositories;

namespace TRobot.ECU.Tests.Data.Repositories
{
    [TestFixture]
    public class UsersAsyncRepositoryTest
    {
        [SetUp]
        public void SetUp()
        {
            DependencyInjector.RegisterType<DbContext, TestDatabaseContext>();
            DependencyInjector.RegisterType<IUsersRepository, UsersRepository>();
        }

        [Test]
        public void GetByIdTest()
        {
            var testRepository = DependencyInjector.Resolve<IUsersRepository>();
            Assert.IsNotNull(testRepository);
            var userTask = testRepository.GetById(1);
            userTask.Wait();
            Assert.IsNotNull(userTask);
            Assert.IsInstanceOf<UserEntity>(userTask.Result);

            var userEntity = userTask.Result;

            // Test resulting object
            Assert.AreEqual(1, userEntity.Id);
            Assert.AreEqual("User1", userEntity.UserName);
        }

        [Test]
        public void GetUserByUserNameTest()
        {
            var testRepository = DependencyInjector.Resolve<IUsersRepository>();
            Assert.IsNotNull(testRepository);
            var userTask = testRepository.GetUserByUserName("User1");
            userTask.Wait();
            Assert.IsNotNull(userTask);
            Assert.IsInstanceOf<UserEntity>(userTask.Result);

            var userEntity = userTask.Result;

            // Test resulting object
            Assert.AreEqual(1, userEntity.Id);
            Assert.AreEqual("User1", userEntity.UserName);
            Assert.AreEqual(2, userEntity.Roles.Count);
        }
    }
}
