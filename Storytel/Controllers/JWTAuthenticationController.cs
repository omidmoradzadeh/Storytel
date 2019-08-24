using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Storytel.Models;
using Storytel.Models.VM;
using Storytel.Repository.Interface;
using Storytel.Security;

namespace Storytel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JWTAuthenticationController : ControllerBase
    {
        private IConfiguration _config;
        private IRepositoryWrapper _repoWrapper;
        private Token token;

        public JWTAuthenticationController(IConfiguration config, IRepositoryWrapper repoWrapper)
        {
            _config = config;
            _repoWrapper = repoWrapper;
            token = new Token(config, repoWrapper);
        }

        [HttpPost]
        public IActionResult Post([FromBody]LoginVM login)
        {
            try
            {
                IActionResult response = Unauthorized();
                var user = token.AuthenticateUser(login);

                if (user != null)
                {
                    var tokenString = token.GenerateJSONWebToken(user);
                    response = Ok(new { token = tokenString });
                }
                else
                    return NotFound("User Not Found");


                return response;
            }
            catch
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }

}

