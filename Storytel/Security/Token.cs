﻿using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Storytel.Models;
using Storytel.Models.VM;
using Storytel.Repository.Interface;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Storytel.Security
{
    public class Token : IToken
    {

        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IConfiguration _config;

        public Token(IConfiguration config ,IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
            _config = config;
        }


        public string GenerateJSONWebToken( User userInfo)
        {
            try
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                var claims = new List<Claim>();
                claims.Add(new Claim("user", userInfo.UserName));
                claims.Add(new Claim("is_admin", userInfo.IsAdmin.ToString()));

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

        public User AuthenticateUser(LoginVM login)
        {
            try
            {
                var userDataList =  _repoWrapper.User.FindByCondition(t => t.UserName.ToLower().Equals(login.UserName.ToLower()) && t.Password.Equals(login.Password)).ToList();
                return  userDataList.Count == 1 ? userDataList[0] : null;
            }
            catch
            {
                throw;
            }
        }

    }
}
