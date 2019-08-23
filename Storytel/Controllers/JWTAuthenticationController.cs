using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Storytel.Models;
using Storytel.Models.VM;
using Storytel.Repository.Interface;

namespace Storytel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JWTAuthenticationController : ControllerBase
    {
        private IConfiguration _config;
        private IRepositoryWrapper _repoWrapper;

        public JWTAuthenticationController(IConfiguration config, IRepositoryWrapper repoWrapper)
        {
            _config = config;
            _repoWrapper = repoWrapper;
        }

        [HttpPost]
        public IActionResult Post([FromBody]LoginVM login)
        {
            try
            {
                IActionResult response = Unauthorized();
                var user = AuthenticateUser(login);

                if (user != null)
                {
                    var tokenString = GenerateJSONWebToken(user);
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

        private string GenerateJSONWebToken(User userInfo)
        {
            try
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                var claims = new List<Claim>();
                claims.Add(new Claim("user", userInfo.UserName));

                var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                  _config["Jwt:Issuer"],
                  claims,
                  expires: DateTime.Now.AddMinutes(120),
                  signingCredentials: credentials);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch
            {
                throw;
            }
        }

        private User AuthenticateUser(LoginVM login)
        {
            try
            {
                User userData = null;
                userData = _repoWrapper.User.FindByCondition(t => t.UserName.ToLower() == login.UserName.ToLower()).FirstOrDefault();
                return userData;
            }
            catch
            {
                throw;
            }
        }
    }

}

