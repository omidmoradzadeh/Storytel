using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Storytel.Models.DTO
{
    public class MessageEditDTO
    {

        [Required]
        [StringLength(200)]
        public string Text { get; set; }
    }
}
