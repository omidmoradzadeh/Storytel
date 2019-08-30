using Microsoft.AspNetCore.Mvc;
using Moq;
using Storytel.Controllers;
using Storytel.Models;
using Storytel.Models.DTO;
using Storytel.Models.VM;
using Storytel.Repository.Interface;
using Storytel.Security;
using Storytel.Services;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Xunit;

namespace StorytelTest
{
    public class MessageTest
    {
        [Fact]
        public void Should_GetMessageFunction_Return_SingleMessage()
        {

            //Arrange
            var _mockRepo = new Mock<IRepositoryWrapper>();
            var _mockPrincipal = new Mock<IUserClaimsPrincipal>();
            var _mockLogger = new Mock<ILoggerManager>();

            var message = new MessageDetailVM()
            {
                Id = 1,
                UserName = "Admin",
                Text = "Hi There.",

            };

            _mockRepo.Setup(x => x.Message.GetMessageWithDetailByIdAsync(message.Id)).ReturnsAsync(message);
            _mockPrincipal.Setup(x => x.GetUserName(It.IsAny<ClaimsPrincipal>())).Returns("admin");
            _mockRepo.Setup(x => x.User.GetUserByUserNameAsync("admin")).ReturnsAsync(1);

            var controller = new MessageController(_mockRepo.Object, _mockLogger.Object, _mockPrincipal.Object);

            //Act
            var actual = controller.Get(message.Id);

            //Assert
            var actualUser = ((MessageDetailVM)(((OkObjectResult)actual.Result).Value));
            Assert.Same(message, actualUser);
            Assert.Equal(message.Id, actualUser.Id);
            Assert.Equal(message.Text, actualUser.Text);
        }
        
        [Fact]
        public void Should_GetFunction_Return_AllMessages()
        {

            //Arrange
            List<MessageDetailVM> messages = new List<MessageDetailVM>();
            messages.Add(new MessageDetailVM
            {
                Id = 1,
                UserName = "Admin",
                Text = "Hi There."
            });
            messages.Add(new MessageDetailVM
            {
                Id = 2,
                UserName = "Admin",
                Text = "How are you?"
            });


            var _mockRepo = new Mock<IRepositoryWrapper>();
            var _mockPrincipal = new Mock<IUserClaimsPrincipal>();
            var _mockLogger = new Mock<ILoggerManager>();

            _mockRepo.Setup(x => x.Message.GetAllMessageWithDetailAsync(1)).ReturnsAsync(messages);
            _mockPrincipal.Setup(x => x.GetUserName(It.IsAny<ClaimsPrincipal>())).Returns("admin");
            _mockRepo.Setup(x => x.User.GetUserByUserNameAsync("admin")).ReturnsAsync(1);

            
            var controller = new MessageController(_mockRepo.Object, _mockLogger.Object, _mockPrincipal.Object);

            //Act
            var actual = controller.Get();

            //Assert
            var actualMessage = ((List<MessageDetailVM>)((ResponseVM)((OkObjectResult)actual).Value).Data);
            Assert.True(actualMessage.Count == 2);
        }

        [Fact]
        public void Should_AddMessageMockFunction_With_Return_MessageID()
        {

            //Arrange
            var newMessage = new MessageAddDTO()
            {
                 Text = "New Message"
            };

            var message = new Message()
            {
                Id = 4,
                Text = "New Message",
                UserId = 1,
            };

            var _mockRepo = new Mock<IRepositoryWrapper>();
            var _mockPrincipal = new Mock<IUserClaimsPrincipal>();
            var _mockLogger = new Mock<ILoggerManager>();

            _mockRepo.Setup(x => x.Message.CreateMessageAsync(message,newMessage,1)).ReturnsAsync(4);
            _mockPrincipal.Setup(x => x.GetUserName(It.IsAny<ClaimsPrincipal>())).Returns("admin");
            _mockRepo.Setup(x => x.User.GetUserByUserNameAsync("admin")).ReturnsAsync(1);

            var controller = new MessageController(_mockRepo.Object, _mockLogger.Object, _mockPrincipal.Object);

            //Act
            var actual = controller.Post(newMessage);

            //Assert
            var actualResult = ((CreatedAtRouteResult)actual).Value;
            System.Reflection.PropertyInfo pi = actualResult.GetType().GetProperty("id");
            int id = (int)(pi.GetValue(actualResult, null));
            Assert.Equal(0, id);
        }

        [Fact]
        public void Should_AddEmptyMessageMockFunction_With_Return_BadRequest()
        {

            //Arrange
            var newMessage = new MessageAddDTO()
            {
                Text = ""
            };

            var message = new Message()
            {
                Id = 4,
                Text = "New Message",
                UserId = 1,
            };

            var _mockRepo = new Mock<IRepositoryWrapper>();
            var _mockPrincipal = new Mock<IUserClaimsPrincipal>();
            var _mockLogger = new Mock<ILoggerManager>();

            _mockRepo.Setup(x => x.Message.CreateMessageAsync(message, newMessage, 1)).ReturnsAsync(4);
            _mockPrincipal.Setup(x => x.GetUserName(It.IsAny<ClaimsPrincipal>())).Returns("admin");
            _mockRepo.Setup(x => x.User.GetUserByUserNameAsync("admin")).ReturnsAsync(1);

            var controller = new MessageController(_mockRepo.Object, _mockLogger.Object, _mockPrincipal.Object);

            //Act
            var actual = controller.Post(newMessage);
            var actualResult = ((BadRequestObjectResult)actual);

            //Assert
            Assert.Same(typeof(BadRequestObjectResult), actualResult.GetType());
            Assert.Equal("Message object is not filled correct", (((ResponseVM)actualResult.Value).Title));
        }

        [Fact]
        public void Should_UpdateEmptyMessageMockFunction_With_Return_BadRequest()
        {

            //Arrange
            var editedMessage = new MessageEditDTO()
            {
                Text = "",
            };

            var message = new Message()
            {
                Id = 1,
                Text = "Message",
                UserId = 1,
            };

            var _mockRepo = new Mock<IRepositoryWrapper>();
            var _mockPrincipal = new Mock<IUserClaimsPrincipal>();
            var _mockLogger = new Mock<ILoggerManager>();

            _mockRepo.Setup(x => x.Message.UpdateMessageAsync(message));
            _mockPrincipal.Setup(x => x.GetUserName(It.IsAny<ClaimsPrincipal>())).Returns("admin");
            _mockRepo.Setup(x => x.User.GetUserByUserNameAsync("admin")).ReturnsAsync(1);

            var controller = new MessageController(_mockRepo.Object, _mockLogger.Object, _mockPrincipal.Object);

            //Act
            var actual = controller.Put(1,editedMessage);
            var actualResult = ((BadRequestObjectResult)actual.Result);

            //Assert
            Assert.Same(typeof(BadRequestObjectResult), actualResult.GetType());
            Assert.Equal("Message object is not filled correct", (((ResponseVM)actualResult.Value).Title));
        }

        [Fact]
        public void Should_UpdateNotExistMessageMockFunction_With_Return_NotFound()
        {

            //Arrange
            var editedMessage = new MessageEditDTO()
            {
                Text = "Update new Message",
            };

            var message = new Message()
            {
                Id = 1,
                Text = "Update Message",
                UserId = 1,
            };

            var _mockRepo = new Mock<IRepositoryWrapper>();
            var _mockPrincipal = new Mock<IUserClaimsPrincipal>();
            var _mockLogger = new Mock<ILoggerManager>();

            _mockRepo.Setup(x => x.Message.UpdateMessageAsync(message));
            _mockRepo.Setup(x => x.Message.GetMessageByIdAsync(5)).ReturnsAsync(new Message());
            _mockPrincipal.Setup(x => x.GetUserName(It.IsAny<ClaimsPrincipal>())).Returns("admin");
            _mockRepo.Setup(x => x.User.GetUserByUserNameAsync("admin")).ReturnsAsync(1);

            var controller = new MessageController(_mockRepo.Object, _mockLogger.Object, _mockPrincipal.Object);

            //Act
            var actual = controller.Put(1, editedMessage);
            var actualResult = ((NotFoundObjectResult)actual.Result);

            //Assert
            Assert.Same(typeof(NotFoundObjectResult), actualResult.GetType());
            Assert.Equal("Message not found.", (((ResponseVM)actualResult.Value).Title));
        }

        [Fact]
        public void Should_UpdateNotMyMessageMockFunction_With_Return_Forbid()
        {

            //Arrange
            var editedMessage = new MessageEditDTO()
            {
                Text = "Update new Message",
            };

            var message = new Message()
            {
                Id = 1,
                Text = "Message",
                UserId = 1,
            };

            var _mockRepo = new Mock<IRepositoryWrapper>();
            var _mockPrincipal = new Mock<IUserClaimsPrincipal>();
            var _mockLogger = new Mock<ILoggerManager>();

            _mockRepo.Setup(x => x.Message.UpdateMessageAsync(message));
            _mockRepo.Setup(x => x.Message.GetMessageByIdAsync(1)).ReturnsAsync(message);
            _mockPrincipal.Setup(x => x.GetUserName(It.IsAny<ClaimsPrincipal>())).Returns("omidm");
            _mockRepo.Setup(x => x.User.GetUserByUserNameAsync("omidm")).ReturnsAsync(2);

            var controller = new MessageController(_mockRepo.Object, _mockLogger.Object, _mockPrincipal.Object);

            //Act
            var actual = controller.Put(1, editedMessage);
            var actualResult = ((ForbidResult)actual.Result);

            //Assert
            Assert.Same(typeof(ForbidResult), actualResult.GetType());
        }

        [Fact]
        public void Should_UpdateMessageMockFunction_With_Return_SuccessWithNoContent()
        {

            //Arrange
            var editedMessage = new MessageEditDTO()
            {
                Text = "Update new Message",
            };

            var message = new Message()
            {
                Id = 1,
                Text = "Message",
                UserId = 1,
            };

            var _mockRepo = new Mock<IRepositoryWrapper>();
            var _mockPrincipal = new Mock<IUserClaimsPrincipal>();
            var _mockLogger = new Mock<ILoggerManager>();

            _mockRepo.Setup(x => x.Message.UpdateMessageAsync(message));
            _mockRepo.Setup(x => x.Message.GetMessageByIdAsync(1)).ReturnsAsync(message);
            _mockPrincipal.Setup(x => x.GetUserName(It.IsAny<ClaimsPrincipal>())).Returns("admin");
            _mockRepo.Setup(x => x.User.GetUserByUserNameAsync("admin")).ReturnsAsync(1);

            var controller = new MessageController(_mockRepo.Object, _mockLogger.Object, _mockPrincipal.Object);

            //Act
            var actual = controller.Put(1, editedMessage);
            var actualResult = ((NoContentResult)actual.Result);

            //Assert
            Assert.Same(typeof(NoContentResult), actualResult.GetType());
        }

        [Fact]
        public void Should_DeleteExistMessageMockFunction_With_Return_NotFound()
        {

            //Arrange
            var message = new Message()
            {
                Id = 1,
                Text = "Update Message",
                UserId = 1,
            };

            var _mockRepo = new Mock<IRepositoryWrapper>();
            var _mockPrincipal = new Mock<IUserClaimsPrincipal>();
            var _mockLogger = new Mock<ILoggerManager>();

            _mockRepo.Setup(x => x.Message.DeleteMessageAsync(message));
            _mockRepo.Setup(x => x.Message.GetMessageByIdAsync(5)).ReturnsAsync(new Message());
            _mockPrincipal.Setup(x => x.GetUserName(It.IsAny<ClaimsPrincipal>())).Returns("admin");
            _mockRepo.Setup(x => x.User.GetUserByUserNameAsync("admin")).ReturnsAsync(1);

            var controller = new MessageController(_mockRepo.Object, _mockLogger.Object, _mockPrincipal.Object);

            //Act
            var actual = controller.Delete(5);
            var actualResult = ((NotFoundObjectResult)actual.Result);

            //Assert
            Assert.Same(typeof(NotFoundObjectResult), actualResult.GetType());
            Assert.Equal("Message not found.", (((ResponseVM)actualResult.Value).Title));
        }

        [Fact]
        public void Should_DeleteNotMyMessageMockFunction_With_Return_Forbid()
        {

            //Arrange
            var message = new Message()
            {
                Id = 1,
                Text = "Message",
                UserId = 1,
            };

            var _mockRepo = new Mock<IRepositoryWrapper>();
            var _mockPrincipal = new Mock<IUserClaimsPrincipal>();
            var _mockLogger = new Mock<ILoggerManager>();

            _mockRepo.Setup(x => x.Message.DeleteMessageAsync(message));
            _mockRepo.Setup(x => x.Message.GetMessageByIdAsync(1)).ReturnsAsync(message);
            _mockPrincipal.Setup(x => x.GetUserName(It.IsAny<ClaimsPrincipal>())).Returns("omidm");
            _mockRepo.Setup(x => x.User.GetUserByUserNameAsync("omidm")).ReturnsAsync(2);

            var controller = new MessageController(_mockRepo.Object, _mockLogger.Object, _mockPrincipal.Object);

            //Act
            var actual = controller.Delete(1);
            var actualResult = ((ForbidResult)actual.Result);

            //Assert
            Assert.Same(typeof(ForbidResult), actualResult.GetType());
        }

        [Fact]
        public void Should_DeleteMessageMockFunction_With_Return_SuccessWithNoCntent()
        {

            //Arrange
            var message = new Message()
            {
                Id = 1,
                Text = "Message",
                UserId = 1,
            };

            var _mockRepo = new Mock<IRepositoryWrapper>();
            var _mockPrincipal = new Mock<IUserClaimsPrincipal>();
            var _mockLogger = new Mock<ILoggerManager>();

            _mockRepo.Setup(x => x.Message.DeleteMessageAsync(message));
            _mockRepo.Setup(x => x.Message.GetMessageByIdAsync(1)).ReturnsAsync(message);
            _mockPrincipal.Setup(x => x.GetUserName(It.IsAny<ClaimsPrincipal>())).Returns("admin");
            _mockRepo.Setup(x => x.User.GetUserByUserNameAsync("admin")).ReturnsAsync(1);

            var controller = new MessageController(_mockRepo.Object, _mockLogger.Object, _mockPrincipal.Object);

            //Act
            var actual = controller.Delete(1);
            var actualResult = ((NoContentResult)actual.Result);

            //Assert
            Assert.Same(typeof(NoContentResult), actualResult.GetType());
        }


    }
}
