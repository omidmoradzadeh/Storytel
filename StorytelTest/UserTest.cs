using System.Collections.Generic;
using Xunit;
using Storytel.Security;
using Storytel.Services;
using Moq;
using Storytel.Repository.Interface;
using Storytel.Controllers;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Storytel.Models.VM;

namespace StorytelTest
{

    public class UserTest
    {


        [Fact]
        public void Should_MockUserFunction_Return_SingleUser()
        {

            //Arrange
            var _mockRepo = new Mock<IRepositoryWrapper>();
            var _mockPrincipal = new Mock<IUserClaimsPrincipal>();
            var _mockLogger = new Mock<ILoggerManager>();

            var user = new UserDetailVM()
            {    
                Id = 1,
                Name = "Admin",
                Family = "",
                UserName = "admin",
                Email = "admin@storytel.com",
                IsAdmin = true,
            };

            _mockRepo.Setup(x => x.User.GetUserWithDetailByIdAsync(user.Id)).ReturnsAsync(user);
            _mockPrincipal.Setup(x => x.IsAdmin(It.IsAny<ClaimsPrincipal>())).Returns(true);

            var controller = new UserController(_mockRepo.Object, _mockLogger.Object, _mockPrincipal.Object);

            //Act
            var actual = controller.Get(user.Id);

            //Assert
            var actualUser = ((UserDetailVM)((ResponseVM)((OkObjectResult)actual.Result).Value).Data);
            Assert.Same(user, actualUser);
            Assert.Equal(user.Id, actualUser.Id);
            Assert.Equal(user.Name, actualUser.Name);
            Assert.Equal(user.Family, actualUser.Family);
            Assert.Equal(user.Email, actualUser.Email);
            Assert.Equal(user.UserName, actualUser.UserName);
            Assert.Equal(user.IsAdmin, actualUser.IsAdmin);

        }


        [Fact]
        public void Should_MockUserFunction_Return_Forbiden()
        {

            //Arrange
            var _mockRepo = new Mock<IRepositoryWrapper>();
            var _mockPrincipal = new Mock<IUserClaimsPrincipal>();
            var _mockLogger = new Mock<ILoggerManager>();

            var user = new UserDetailVM()
            {
                Id = 1,
                Name = "Admin",
                Family = "",
                UserName = "admin",
                Email = "admin@storytel.com",
                IsAdmin = true,
            };

            _mockRepo.Setup(x => x.User.GetUserWithDetailByIdAsync(user.Id)).ReturnsAsync(user);
            _mockPrincipal.Setup(x => x.IsAdmin(It.IsAny<ClaimsPrincipal>())).Returns(false);

            var controller = new UserController(_mockRepo.Object, _mockLogger.Object, _mockPrincipal.Object);

            //Act
            var actual = controller.Get(user.Id).Result;

            //Assert
            Assert.Same(actual.GetType(), typeof(ForbidResult));
        }

        [Fact]
        public void Should_Mock_Function_With_Return_All_Users()
        {

            //Arrange
            List<UserDetailVM> users = new List<UserDetailVM>();
            users.Add(new UserDetailVM
            {
                Id = 1,
                Name = "Admin",
                Family = "",
                UserName = "admin",
                Email = "admin@storytel.com",
                IsAdmin = true
            });
            users.Add(new UserDetailVM
            {
                Id = 2,
                Name = "Omid",
                Family = "Moradzadeh",
                UserName = "omidm",
                Email = "omidm@storytel.com",
                IsAdmin = false
            });


            var _mockRepo = new Mock<IRepositoryWrapper>();
            var _mockPrincipal = new Mock<IUserClaimsPrincipal>();
            var _mockLogger = new Mock<ILoggerManager>();

            _mockRepo.Setup(x => x.User.GetAllUserWithDetailAsync()).ReturnsAsync(users);
            _mockPrincipal.Setup(x => x.IsAdmin(It.IsAny<ClaimsPrincipal>())).Returns(true);

            var controller = new UserController(_mockRepo.Object, _mockLogger.Object, _mockPrincipal.Object);

            //Act
            var actual = controller.Get();

            //Assert
            var actualUser = ((List<UserDetailVM>)((OkObjectResult)actual).Value);
            Assert.True(actualUser.Count == 2);

        }

        [Fact]
        public void Should_Mock_Function_With_Return_All_Users2()
        {

            //Arrange
            List<UserDetailVM> users = new List<UserDetailVM>();
            users.Add(new UserDetailVM
            {
                Id = 1,
                Name = "Admin",
                Family = "",
                UserName = "admin",
                Email = "admin@storytel.com",
                IsAdmin = true
            });
            users.Add(new UserDetailVM
            {
                Id = 2,
                Name = "Omid",
                Family = "Moradzadeh",
                UserName = "omidm",
                Email = "omidm@storytel.com",
                IsAdmin = false
            });


            var _mockRepo = new Mock<IRepositoryWrapper>();
            var _mockPrincipal = new Mock<IUserClaimsPrincipal>();
            var _mockLogger = new Mock<ILoggerManager>();

            _mockRepo.Setup(x => x.User.GetAllUserWithDetailAsync()).ReturnsAsync(users);
            _mockPrincipal.Setup(x => x.IsAdmin(It.IsAny<ClaimsPrincipal>())).Returns(true);

            var controller = new UserController(_mockRepo.Object, _mockLogger.Object, _mockPrincipal.Object);

            //Act
            var actual = controller.Get();

            //Assert
            var actualUser = ((List<UserDetailVM>)((OkObjectResult)actual).Value);
            Assert.True(actualUser.Count == 2);

        }
    }
}
