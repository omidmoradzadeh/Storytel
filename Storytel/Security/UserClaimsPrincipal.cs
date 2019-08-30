using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Storytel.Security
{
    public class UserClaimsPrincipal : IUserClaimsPrincipal
    {
        private string GetClaimValue(ClaimsPrincipal claims, string claimName)
        {
            if (claims == null)
                return null;
            if (claims.HasClaim(c => c.Type == claimName))
            {
                return claims.Claims.FirstOrDefault(c => c.Type == claimName).Value.ToLower();
            }
            return null;
        }

        public string GetUserName(ClaimsPrincipal claims)
        {
            return GetClaimValue(claims, "user");
        }

        public bool IsAdmin(ClaimsPrincipal claims)
        {
            return GetClaimValue(claims, "is_admin") == "true";
        }


    }
}
