using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Configuration;
using Moq;
using Storytel.Controllers;
using Storytel.Models;
using Storytel.Models.VM;
using Storytel.Repository.Interface;
using Storytel.Security;
using Storytel.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Xunit;

namespace StorytelTest
{
    public class JWTAuthentication
    {

        [Fact]
        public void Should_ModelRequierd_MockFunction_Return_ValidationError()
        {
            //Arrange
            var result = new List<ValidationResult>();

            var user = new LoginVM()
            {
                UserName = "A",
                Password = "A"

            };

            //Act
            var isValid = Validator.TryValidateObject(user, new ValidationContext(user), result, true);

            //Assert
            Assert.True(!isValid);
            Assert.Equal("UserName", result[0].MemberNames.ElementAt(0));
            Assert.Equal("Password", result[1].MemberNames.ElementAt(0));
            Assert.Equal("The field UserName must be a string with a minimum length of 4 and a maximum length of 50.", result[0].ErrorMessage);
            Assert.Equal("The field Password must be a string with a minimum length of 4 and a maximum length of 50.", result[1].ErrorMessage);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void Should_PasswordNotValid_MockFunction_Return_NotFound()
        {
            //Arrange
            var _configuration = new Mock<IConfiguration>();
            _configuration.SetupGet(x => x[It.Is<string>(s => s == "Jwt:Key")]).Returns("ThisismySecretKey");
            _configuration.SetupGet(x => x[It.Is<string>(s => s == "Jwt:Issuer")]).Returns("Test.com");
            var _mockToken = new Mock<IToken>();
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
            var controller = new JWTAuthenticationController(_configuration.Object, _mockRepo.Object, _mockLogger.Object, _mockToken.Object);

            //Act
            var actual = controller.Post(user);

            //Assert
            Assert.Same(typeof(NotFoundObjectResult), actual.GetType());

        }

        [Fact]
        public void Should_GetMockUserTokenFunction_Return_ValidToken()
        {

            //Arrange
            var _configuration = new Mock<IConfiguration>();
            var _mockRepo = new Mock<IRepositoryWrapper>();
            var _mockLogger = new Mock<ILoggerManager>();
            var _mockToken = new Mock<IToken>();
            string generatedToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VyIjoiYWRtaW4iLCJpc19hZG1pbiI6IlRydWUiLCJleHAiOjE1NjcwMjkwNDAsImlzcyI6IlRlc3QuY29tIiwiYXVkIjoiVGVzdC5jb20ifQ.cqBycJ7bWw7bTCtnw8V0tQGA-XOdsCo6K_CimzPQd5Q";
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

            _mockToken.Setup(x => x.GenerateJSONWebToken(userResult[0])).Returns(generatedToken);
            _mockToken.Setup(x => x.AuthenticateUser(user)).Returns(userResult[0]);
            _mockRepo.Setup(x => x.User.FindByCondition(It.IsAny<Expression<Func<User, bool>>>())).Returns(userResult.AsQueryable());

            var controller = new JWTAuthenticationController(_configuration.Object, _mockRepo.Object, _mockLogger.Object, _mockToken.Object);

            //Act
            var actual = controller.Post(user);

            //Assert
            var actualParsed = (((ResponseVM)((OkObjectResult)actual).Value).Data);
            System.Reflection.PropertyInfo pi = actualParsed.GetType().GetProperty("token");
            string token = (string)(pi.GetValue(actualParsed, null));
            Assert.Equal(generatedToken ,token);
        }

    }
}
