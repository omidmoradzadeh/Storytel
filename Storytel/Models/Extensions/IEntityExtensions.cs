using Storytel.Models.Extensions.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Storytel.Models.Extensions
{
    public static class IEntityExtensions
    {
        public static bool IsObjectNull(this IEntity entity)
        {
            return entity == null;
        }

        public static bool IsEmptyObject(this IEntity entity)
        {
            return entity.Id.Equals(0);
        }
    }
}
