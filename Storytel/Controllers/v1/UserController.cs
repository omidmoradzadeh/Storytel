using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
    public class UserController : ControllerBase
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly ILoggerManager _logger;
        private readonly UserClaimsPrincipal _userClaimsPrincipal;


        public UserController(IRepositoryWrapper repoWrapper, ILoggerManager logger)
        {
            _repoWrapper = repoWrapper;
            _logger = logger;
            _userClaimsPrincipal = new UserClaimsPrincipal();
        }

        [HttpGet]
        [Authorize]
        [Produces(typeof(UserDetailVM))]
        public IActionResult Get()
        {
            try
            {
                if (_userClaimsPrincipal.GetClaimValue(HttpContext.User, "is_admin") == "true")
                {
                    var userList = _repoWrapper.User.GetAllUserWithDetailAsync();
                    return Ok(userList.Result);
                }
                return Forbid();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetUser action: {ex.Message}");
                return StatusCode(500, new ResponseVM("Internal server error"));
            }
        }


        [Authorize]
        [HttpGet("{id}", Name = "UserById")]
        [Produces(typeof(UserDetailVM))]
        public async Task<IActionResult> Get([FromRoute] int id)
        {

            try
            {
                if (_userClaimsPrincipal.GetClaimValue(HttpContext.User, "is_admin") == "true")
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(new ResponseVM("Error in modelState"));
                    }

                    var user = await _repoWrapper.User.GetUserWithDetailByIdAsync(id);
                    if (user == null)
                    {
                        return NotFound(new ResponseVM("User Not Found"));
                    }
                    return Ok(new ResponseVM(hasError:false, data: user));
                }
                return Forbid();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetUserByID action: {ex.Message}");
                return StatusCode(500, new ResponseVM("Internal server error"));
            }
        }


        [Authorize]
        [HttpPost]
        public IActionResult Post([FromBody]UserAddDTO user)
        {
            try
            {
                if (_userClaimsPrincipal.GetClaimValue(HttpContext.User, "is_admin") == "true")
                {
                    if (user == null)
                    {
                        return BadRequest(new ResponseVM("User object is not filled correct"));
                    }

                    if (!ModelState.IsValid)
                    {
                        return BadRequest(new ResponseVM("Invalid model object"));
                    }

                    int userId = _repoWrapper.User.CreateUserAsync(new User(), user).Result;
                    _repoWrapper.Save();

                    return CreatedAtRoute("UserById", new { id = userId }, new { id = userId });
                }
                return Forbid();

            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside Createuser action: {ex.Message}");
                return StatusCode(500, new ResponseVM("Internal server error"));
            }
        }


        [Authorize]
        [HttpPut("{id}")]
        [Produces(typeof(UserVM))]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] UserEditDTO user)
        {

            try
            {
                if (_userClaimsPrincipal.GetClaimValue(HttpContext.User, "is_admin") == "true")
                {
                    if (user == null)
                    {
                        return BadRequest(new ResponseVM("User object is not filled correct"));
                    }

                    if (!ModelState.IsValid)
                    {
                        return BadRequest(new ResponseVM("Invalid model object"));
                    }

                    var dbUser = await _repoWrapper.User.GetUserByIdAsync(id);
                    if (dbUser == null || dbUser.Id == 0)
                    {
                        return NotFound(new ResponseVM("User not found"));
                    }

                    await _repoWrapper.User.UpdateUserAsync(dbUser, user);
                    _repoWrapper.Save();

                    return NoContent();
                }
                return Forbid();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside UpdateUser action: {ex.Message}");
                return StatusCode(500, new ResponseVM("Internal server error"));
            }
        }


        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (_userClaimsPrincipal.GetClaimValue(HttpContext.User, "is_admin") == "true")
                {

                    var user = await _repoWrapper.User.GetUserByIdAsync(id);
                    if (user == null || user.Id == 0)
                    {
                        return NotFound(new ResponseVM("User not found"));
                    }

                    await _repoWrapper.User.DeleteUserAsync(user);
                    _repoWrapper.Save();

                    return NoContent();
                }
                return Forbid();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside DeleteUser action: {ex.Message}");
                return StatusCode(500, new ResponseVM("Internal server error"));
            }
        }

    }
}