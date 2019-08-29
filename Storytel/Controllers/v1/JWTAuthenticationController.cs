using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
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
    [ApiController]
    [EnableCors("CorsPolicy")]
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    public class JWTAuthenticationController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IConfiguration _config;
        private readonly Token _token;

        public JWTAuthenticationController(IConfiguration config, IRepositoryWrapper repoWrapper, ILoggerManager logger)
        {
            _logger = logger;
            _config = config;
            _token = new Token( repoWrapper);
        }

        [HttpPost]
        [EnableCors("CorsPolicy")]
        public  IActionResult Post([FromBody][Required]LoginVM login)
        {
            try
            {
                IActionResult response = Unauthorized();

                if (!ModelState.IsValid)
                {
                    return BadRequest(new ResponseVM("User name or password must be enterd."));
                }

                Crypto crypto = new Crypto(CryptoTypes.encTypeTripleDES);
                login.Password = crypto.Encrypt(login.Password);

                var user = _token.AuthenticateUser(login);

                if (user != null)
                {
                    var tokenString = _token.GenerateJSONWebToken(_config , user);
                    response = Ok(new ResponseVM(hasError: false, data: new { token = tokenString }));
                }
                else
                    return NotFound(new ResponseVM("User not found"));


                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError("Invalid user object sent from client.Exception:" + ex.Message ?? "");
                return StatusCode(500, new ResponseVM("Internal server error"));
            }
        }
    }

}

