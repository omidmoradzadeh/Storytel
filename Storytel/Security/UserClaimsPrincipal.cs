using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Storytel.Security
{
    public class UserClaimsPrincipal
    {
        public string GetClaimValue(ClaimsPrincipal claims, string claimName)
        {
            if (claims.HasClaim(c => c.Type == claimName))
            {
                return claims.Claims.FirstOrDefault(c => c.Type == claimName).Value.ToLower();
            }
            return null;
        }
    }
}
