using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Storytel.Controllers;
using Storytel.Models;
using Storytel.Models.VM;
using Storytel.Repository.Interface;
using Storytel.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Xunit;
using static Storytel.Security.CryptoLibrary;
using static Storytel.Security.CryptoLibrary.Crypto;

namespace StorytelTest
{
    public class JWTAuthentication
    {


        [Fact]
        public void Should_MockUserFunction_Return_ValidToken()
        {


            //Arrange
            var _configuration = new Mock<IConfiguration>();
            _configuration.SetupGet(x => x[It.Is<string>(s => s == "Key")]).Returns("ThisismySecretKey");
            _configuration.SetupGet(x => x[It.Is<string>(s => s == "Issuer")]).Returns("Test.com");

            var _mockRepo = new Mock<IRepositoryWrapper>();
            var _mockLogger = new Mock<ILoggerManager>();

            List<User> userResult = new List<User>();
            userResult.Add(new User
            {
                Id = 1,
                Name = "Admin",
                Family = "",
                UserName = "admin",
                Email = "admin@storytel.com",
                IsAdmin = true,
                IsActive = false,
                Password = "MbNfMe01OfQaSaZJY5smbA=="
            });


            var user = new LoginVM()
            {
                UserName = "Admin",
                Password = "Aa@123456"
            };

            //_mockRepo.Setup(x => x.User.FindByCondition(y => y.UserName == userResult[0].UserName && y.Password == userResult[0].Password)).Returns(userResult.AsQueryable());
            _mockRepo.Setup(x => x.User.FindByCondition(It.IsAny<Expression<Func<User, bool>>>())).Returns(userResult.AsQueryable());

            var controller = new JWTAuthenticationController(_configuration.Object, _mockRepo.Object, _mockLogger.Object);

            //Act
            var actual = controller.Post(user);

            //Assert
            var token = (((ResponseVM)((OkObjectResult)actual).Value).Data);
            Assert.Same(true, true);

        }

        [Fact]
        public void Should_ModelNotValid_MockFunction_Return_BadRequest()
        {


            //Arrange
            var _configuration = new Mock<IConfiguration>();
            _configuration.SetupGet(x => x[It.Is<string>(s => s == "Jwt:Key")]).Returns("ThisismySecretKey");
            _configuration.SetupGet(x => x[It.Is<string>(s => s == "Jwt:Issuer")]).Returns("Test.com");

            var _mockRepo = new Mock<IRepositoryWrapper>();
            var _mockLogger = new Mock<ILoggerManager>();

            List<User> userResult = new List<User>();
            userResult.Add(new User
            {
                Id = 1,
                Name = "Admin",
                Family = "",
                UserName = "admin",
                Email = "admin@storytel.com",
                IsAdmin = true,
                IsActive = false,
                Password = "MbNfMe01OfQaSaZJY5smbA=="
            });


            var user = new LoginVM()
            {
                UserName = "Admin",
                Password= "pppppppppppppppppppppppppppppppppppppppppppppppppppppppppppp"

            };

            var controller = new JWTAuthenticationController(_configuration.Object, _mockRepo.Object, _mockLogger.Object);

            //Act
            var actual = controller.Post(user);

            //Assert
            Assert.Same(typeof(BadRequestResult), actual.GetType());

        }

        [Fact]
        public void Should_PasswordNotValid_MockFunction_Return_NotFound()
        {


            //Arrange
            var _configuration = new Mock<IConfiguration>();
            _configuration.SetupGet(x => x[It.Is<string>(s => s == "Jwt:Key")]).Returns("ThisismySecretKey");
            _configuration.SetupGet(x => x[It.Is<string>(s => s == "Jwt:Issuer")]).Returns("Test.com");

            var _mockRepo = new Mock<IRepositoryWrapper>();
            var _mockLogger = new Mock<ILoggerManager>();

            List<User> userResult = new List<User>();
            userResult.Add(new User
            {
                Id = 1,
                Name = "Admin",
                Family = "",
                UserName = "admin",
                Email = "admin@storytel.com",
                IsAdmin = true,
                IsActive = false,
                Password = "MbNfMe01OfQaSaZJY5smbA=="
            });


            var user = new LoginVM()
            {
                UserName = "Admin",
                Password = "Aa@123"

            };

            _mockRepo.Setup(x => x.User.FindByCondition(y => y.UserName == user.UserName && y.Password == user.Password)).Returns((new List<User>()).AsQueryable());
            var controller = new JWTAuthenticationController(_configuration.Object, _mockRepo.Object, _mockLogger.Object);

            //Act
            var actual = controller.Post(user);

            //Assert
            Assert.Same(typeof(NotFoundObjectResult), actual.GetType());

        }


    }
}
