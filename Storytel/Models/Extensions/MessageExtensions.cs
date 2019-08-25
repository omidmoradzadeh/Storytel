using Storytel.Models.DTO;
using System;

namespace Storytel.Models.Extensions
{
    public static class MessageExtensions
    {
        public static void MapForEdit(this Message dbMessage, MessageEditDTO message)
        {
            dbMessage.Text = message.Text;
            dbMessage.EditDate = DateTime.Now;
        }

        public static Message MapForAdd(this Message dbMessage, MessageAddDTO message , int userId)
        {
            dbMessage.Text = message.Text;
            dbMessage.UserId = userId;
            return dbMessage;
        }
    }
}
