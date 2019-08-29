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
        //[Authorize]
        [Produces(typeof(MessageDetailVM))]
        public IActionResult Get()
        {
            try
            {
                int userId = _repoWrapper.User.GetUserByUserNameAsync(_userClaimsPrincipal.GetUserName(HttpContext.User)).Result;
                var messageList = _repoWrapper.Message.GetAllMessageWithDetailAsync(userId);
                return Ok( new ResponseVM (hasError: false,data: messageList.Result));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetMessages action: {ex.Message}");
                return StatusCode(500, new ResponseVM("Internal server error"));
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
                    return NotFound(new ResponseVM("Message Not Found"));
                }
                return Ok(message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetMessageById action: {ex.Message}");
                return StatusCode(500, new ResponseVM("Internal server error"));
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
                    return BadRequest(new ResponseVM("Message object is not filled correct"));
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(new ResponseVM("Invalid model object"));
                }

                int userId = _repoWrapper.User.GetUserByUserNameAsync(_userClaimsPrincipal.GetUserName(HttpContext.User)).Result;

                int messageId = _repoWrapper.Message.CreateMessageAsync(new Message(), message, userId).Result;
                _repoWrapper.Save();

                return CreatedAtRoute("MessageById", new { id = messageId }, new { id = messageId });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside CreateMessage action: {ex.Message}");
                return StatusCode(500, new ResponseVM("Internal server error"));
            }
        }


        [Authorize]
        [HttpPut("{id}")]

        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] MessageEditDTO message)
        {

            try
            {
                int userId = _repoWrapper.User.GetUserByUserNameAsync(_userClaimsPrincipal.GetUserName(HttpContext.User)).Result;

                if (message == null)
                {
                    return BadRequest(new ResponseVM("Message object is not filled correct"));
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(new ResponseVM("Invalid message object"));
                }

                var dbMessage = await _repoWrapper.Message.GetMessageByIdAsync(id);
                if (dbMessage == null || dbMessage.Id == 0)
                {
                    return NotFound(new ResponseVM("User not found."));
                }
                else if (userId != dbMessage.UserId)
                {
                    return Unauthorized(new ResponseVM("This message not belog to you"));
                }


                await _repoWrapper.Message.UpdateMessageAsync(dbMessage, message);
                _repoWrapper.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside UpdateMessage action: {ex.Message}");
                return StatusCode(500, new ResponseVM("Internal server error"));
            }
        }


        [Authorize]
        [HttpDelete("{id}")]

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                int userId = _repoWrapper.User.GetUserByUserNameAsync(_userClaimsPrincipal.GetUserName(HttpContext.User)).Result;

                var message = await _repoWrapper.Message.GetMessageByIdAsync(id);
                if (message == null || message.Id == 0)
                {
                    return NotFound();
                }
                else if (userId != message.UserId)
                {
                    return Unauthorized(new ResponseVM("This message not belog to you"));
                }
                await _repoWrapper.Message.DeleteMessageAsync(message);
                _repoWrapper.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside DeleteMessage action: {ex.Message}");
                return StatusCode(500, new ResponseVM("Internal server error"));
            }
        }

    }
}