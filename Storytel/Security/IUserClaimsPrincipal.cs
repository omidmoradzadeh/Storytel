using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Storytel.Security
{
    public interface IUserClaimsPrincipal
    {
        bool IsAdmin(ClaimsPrincipal claims);
        string GetUserName(ClaimsPrincipal claims);
    }
}
