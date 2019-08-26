using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Storytel.Models.VM;
using Storytel.Repository.Interface;
using Storytel.Security;
using Storytel.Services;
using static Storytel.Security.CryptoLibrary;
using static Storytel.Security.CryptoLibrary.Crypto;

namespace Storytel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("CorsPolicy")]
    public class JWTAuthenticationController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly Token _token;

        public JWTAuthenticationController(IConfiguration config, IRepositoryWrapper repoWrapper, ILoggerManager logger)
        {
            _logger = logger;
            _token = new Token(config, repoWrapper);
        }

        [HttpPost]
        [EnableCors("CorsPolicy")]
        public IActionResult Post([FromBody]LoginVM login)
        {
            try
            {
                IActionResult response = Unauthorized();

                if (!ModelState.IsValid)
                {
                    return BadRequest("user name or password must be enterd.");
                }

                Crypto crypto = new Crypto(CryptoTypes.encTypeTripleDES);
                login.Password = crypto.Encrypt(login.Password);

                var user = _token.AuthenticateUser(login);

                if (user != null)
                {
                    var tokenString = _token.GenerateJSONWebToken(user);
                    response = Ok(new { token = tokenString });
                }
                else
                    return NotFound("User Not Found");


                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError("Invalid user object sent from client.Exception:" + ex.Message ?? "");
                return StatusCode(500, "Internal server error");
            }
        }
    }

}

