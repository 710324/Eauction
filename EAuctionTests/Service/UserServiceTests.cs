using AutoFixture.NUnit3;
using EAuction.Models.Enum;
using EAuction.Models.Seller;
using EAuction.Processor.Exceptions;
using EAuction.Processor.Interface;
using EAuction.Processor.Services;
using EAuctionTests.TestDependencies;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace EAuction.Tests.Service
{
    public class UserServiceTests
    {
        [Test]
        [FakeDependencies]
        public void UserService_WithUserRepositoryNull_ThrownError(IUserRepository userRepository)
        {
            userRepository = null;
            Assert.Throws<ArgumentException>(() => new UserService(userRepository));
        }

        [Test]
        [FakeDependencies]
        public async Task UserService_WithNewUserCreateSeller_Success(UserService userService)
        {
            var user = GetUser();
            var result = await userService.CreateOrUpdateAsync(user);
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.UserId);
            Assert.AreEqual(UserType.Seller.ToString(), result.UserType);
        }

        [Test]
        [FakeDependencies]
        public async Task UserService_WithNewUserCreateBuyer_Success(UserService userService)
        {
            var user = GetUser();
            user.UserType = "2";
            var result = await userService.CreateOrUpdateAsync(user);
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.UserId);
            Assert.AreEqual(UserType.Buyer.ToString(), result.UserType);
        }

        [Test]
        [FakeDependencies]
        public async Task UserService_WithupdateUserCreateBuyer_Success(UserService userService)
        {
            var user = GetUser();
            user.UserId = Guid.NewGuid().ToString();
            user.UserType = "2";
            var result = await userService.CreateOrUpdateAsync(user);
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Updateddate);
        }

        [Test]
        [FakeDependencies]
        public void UserService_WithUpdateUserCreateNotFoundUser_ThrownError(UserService userService, [Frozen] Mock<IUserRepository> mockUserRepository)
        {
            var user = GetUser();
            user.UserId = Guid.NewGuid().ToString();
            user.UserType = "2";

            User userOutput = null;
            mockUserRepository.Setup(x => x.GetUserByIDAsync(It.IsAny<string>()))
                .ReturnsAsync(userOutput);

            Assert.ThrowsAsync<UserNotFounException>(async () =>
            {
                var result = await userService.CreateOrUpdateAsync(user);
            });
        }

        private static User GetUser()
        {
            var usermodel = new User()
            {
                FirstName = "FName001",
                LastName = "LName001",
                Address = "Address001",
                City = "City001",
                Email = "test@gmail.com",
                Phone = "1234567890",
                UserName = "test001",
                Password = "test001",
                UserType = "1"
            };
            return usermodel;
        }
    }
}
