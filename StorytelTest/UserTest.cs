using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Storytel.Security;
using static Storytel.Security.CryptoLibrary.Crypto;
using static Storytel.Security.CryptoLibrary;
using Storytel.Models;
using Storytel.Services;
using Moq;
using Storytel.Repository.Interface;
using Storytel.Controllers;

namespace StorytelTest
{
    public class UserTest
    {
        [Fact]
        public async System.Threading.Tasks.Task Should_Mock_Function_With_Return_ValueAsync()
        {

            //Arrange
            Crypto crypto = new Crypto(CryptoTypes.encTypeTripleDES);
            var user = new User
            {
                Id = 1,
                Name = "Admin",
                Family = "",
                UserName = "admin",
                Password = crypto.Encrypt("Aa@123456"),
                Email = "admin@storytel.com",
                IsAdmin = true
            };

            var mockLogger = new Mock<ILoggerManager>();
            var mock = new Mock<IRepositoryWrapper>();
            mockLogger.Setup(x => x.LogInfo(""));
            mock.Setup(x => x.User.Find(user.Id)).ReturnsAsync(user);

            var controller = new UserController(mock.Object, mockLogger.Object);

            //Act
            var actual = controller.Get(1);

            //Assert
            Assert.Same(user, actual);
            Assert.Equal(user.Id, actual.Id);
            Assert.Equal(user.Name, actual.Name);
            Assert.Equal(user.Family, actual.Family);
            Assert.Equal(user.UserName, actual.UserName);
            Assert.Equal(user.Password, actual.Password);
            Assert.Equal(user.Email, actual.Email);
            Assert.Equal(user.IsAdmin, actual.IsAdmin);
        }
    }
}
