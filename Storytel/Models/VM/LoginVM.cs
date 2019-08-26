using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Storytel.Models.VM
{
    public class LoginVM
    {
        [Required]
        [StringLength(50,MinimumLength =4)]
        public string UserName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 4)]
        public string Password { get; set; }
    }
}
