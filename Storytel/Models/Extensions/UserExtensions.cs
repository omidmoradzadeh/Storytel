using Storytel.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Storytel.Models.Extensions
{
    public static class UserExtensions
    {
        public static void MapForEdit(this User dbUser, UserEditDTO user)
        {
            dbUser.Id = user.Id;
            dbUser.Name = user.Name;
            dbUser.Family = user.Family;
            dbUser.Email = user.Email;
            dbUser.Password = user.Password;
            dbUser.IsActive = user.IsActive;
        }

        public static User MapForAdd(this User dbUser, UserAddDTO user)
        {
            dbUser.Name = user.Name;
            dbUser.Family = user.Family;
            dbUser.UserName = user.UserName;
            dbUser.Email = user.Email;
            dbUser.Password = user.Password;
            return dbUser;
        }
    }
}
