using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Storytel.Models
{
    public class DataGenerator
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new StorytelContext(
                serviceProvider.GetRequiredService<DbContextOptions<StorytelContext>>()))
            {
                // Look for any User.
                if (context.Users.Any())
                {
                    return;   // Data was already seeded
                }

                context.Users.AddRange(
                    new User
                    {
                        Name = "Admin",
                        Family = "",
                        UserName = "admin",
                        Password = "Aa@123456",
                        Email = "admin@storytel.com",
                        IsAdmin =true
                    },
                    new User
                    {
                        Name = "Omid",
                        Family = "Moradzadeh",
                        UserName = "omidm",
                        Password = "Aa@123456",
                        Email = "omidm@storytel.com"
                    });

                context.SaveChanges();
            }
        }
    }
}
