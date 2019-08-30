using Storytel.Models;
using Storytel.Models.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Storytel.Security
{
    public interface IToken
    {
        string GenerateJSONWebToken(User userInfo);
        User AuthenticateUser(LoginVM login);
    }
}
