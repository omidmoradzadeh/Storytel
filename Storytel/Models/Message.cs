using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Storytel.Models
{
    public class Message
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [StringLength(200)]
        public string Text { get; set; }

        public bool IsDeleted { get; set; } = false;

        [Required]
        public DateTime CreateDate { get; set; } = DateTime.Now;

        public DateTime EditDate { get; set; }

        public DateTime DeleteDate { get; set; }
    }
}
