using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Storytel.Models;
using Storytel.Models.DTO;
using Storytel.Models.VM;
using Storytel.Repository.Interface;
using Storytel.Security;
using Storytel.Services;

namespace Storytel.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [EnableCors("CorsPolicy")]
    public class MessageController : ControllerBase
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly ILoggerManager _logger;
        private readonly UserClaimsPrincipal _userClaimsPrincipal;



        public MessageController(IRepositoryWrapper repoWrapper, ILoggerManager logger)
        {
            _repoWrapper = repoWrapper;
            _logger = logger;
            _userClaimsPrincipal = new UserClaimsPrincipal();
        }

        [HttpGet]
        [Authorize]
        
        [Produces(typeof(MessageDetailVM))]
        public async Task<IActionResult> Get()
        {
            try
            {
                int userId = _repoWrapper.User.GetUserByUserNameAsync(_userClaimsPrincipal.GetClaimValue(HttpContext.User, "user")).Result;
                var messageList = _repoWrapper.Message.GetAllMessageWithDetailAsync(userId);
                return Ok(messageList.Result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetMessages action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }


        [Authorize]
        [HttpGet("{id}", Name = "MessageById")]
        [Produces(typeof(MessageDetailVM))]
        
        public async Task<IActionResult> Get([FromRoute] int id)
        {

            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var message = await _repoWrapper.Message.GetMessageWithDetailByIdAsync(id);
                if (message == null)
                {
                    return NotFound("Message Not Found");
                }
                return Ok(message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetMessageById action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }


        [Authorize]
        [HttpPost]
        
        public IActionResult Post([FromBody]MessageAddDTO message)
        {
            try
            {
                if (message == null)
                {
                    _logger.LogError("Message object sent from client is null.");
                    return BadRequest("Message object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid message object sent from client.");
                    return BadRequest("Invalid model object");
                }
                int userId = _repoWrapper.User.GetUserByUserNameAsync(_userClaimsPrincipal.GetClaimValue(HttpContext.User, "user")).Result;

                int messageId = _repoWrapper.Message.CreateMessageAsync(new Message(), message , userId).Result;
                _repoWrapper.Save();

                return CreatedAtRoute("MessageById", new { id = messageId }, new { id = messageId });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside CreateMessage action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }


        [Authorize]
        [HttpPut("{id}")]
        
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] MessageEditDTO message)
        {

            try
            {
                int userId = _repoWrapper.User.GetUserByUserNameAsync(_userClaimsPrincipal.GetClaimValue(HttpContext.User, "user")).Result;
                if (message == null)
                {
                    _logger.LogError("Message object sent from client is null.");
                    return BadRequest("Message object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid message object sent from client.");
                    return BadRequest("Invalid model object");
                }

                var dbMessage = await _repoWrapper.Message.GetMessageByIdAsync(id);
                if (dbMessage == null || dbMessage.Id == 0)
                {
                    _logger.LogError($"Message with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else if (userId != dbMessage.UserId)
                {
                    _logger.LogError($"This message not belog to you.");
                    return Unauthorized("This message not belog to you");
                }


                await _repoWrapper.Message.UpdateMessageAsync(dbMessage, message);
                _repoWrapper.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside UpdateMessage action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }


        [Authorize]
        [HttpDelete("{id}")]
        
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                int userId = _repoWrapper.User.GetUserByUserNameAsync(_userClaimsPrincipal.GetClaimValue(HttpContext.User, "user")).Result;
                var message = await _repoWrapper.Message.GetMessageByIdAsync(id);
                if (message == null || message.Id == 0)
                {
                    _logger.LogError($"Message with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else if (userId != message.UserId)
                {
                    _logger.LogError($"This message not belog to you.");
                    return Unauthorized("This message not belog to you");
                }
                await _repoWrapper.Message.DeleteMessageAsync(message);
                _repoWrapper.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside DeleteMessage action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

    }
}