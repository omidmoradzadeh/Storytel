using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Storytel.Models.VM
{
    public class UserDetailVM
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Family { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; } = false;
    }
}
