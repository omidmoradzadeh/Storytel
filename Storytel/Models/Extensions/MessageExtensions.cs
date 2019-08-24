using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Storytel.Models.Extensions
{
    public static class MessageExtensions
    {
        public static void Map(this Message dbMessage, Message message)
        {
            dbMessage.Id = message.Id;
            dbMessage.Text = message.Text;
            dbMessage.IsDeleted = message.IsDeleted;
        }
    }
}
