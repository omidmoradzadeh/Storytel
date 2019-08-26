using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Storytel.Security.CryptoLibrary;
using static Storytel.Security.CryptoLibrary.Crypto;

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

                Crypto crypto = new Crypto(CryptoTypes.encTypeTripleDES);

                context.Users.AddRange(
                    new User
                    {
                        Name = "Admin",
                        Family = "",
                        UserName = "admin",
                        Password = crypto.Encrypt("Aa@123456"),
                        Email = "admin@storytel.com",
                        IsAdmin = true
                    },
                    new User
                    {
                        Name = "Omid",
                        Family = "Moradzadeh",
                        UserName = "omidm",
                        Password = crypto.Encrypt("Aa@78945"),
                        Email = "omidm@storytel.com"
                    });

                context.Messages.AddRange(
                    new Message
                    {
                        UserId = 1,
                        Text = "Hi There."
                    },
                    new Message
                    {
                        UserId = 1,
                        Text = "How are you?"
                    },
                    new Message
                    {
                        UserId = 2,
                        Text = "Hi Admin, Thanks a lot."
                    });

                context.SaveChanges();
            }
        }
    }
}
