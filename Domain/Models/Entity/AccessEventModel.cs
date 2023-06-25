using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Entity
{
    public class AccessEventModel
    {
        public Guid Id { get; set; }
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public Guid DoorId { get; set; }
        public DateTime EventTime { get; set; } = DateTime.Now;
        [Required]
        public bool Success { get; set; }
    }
}
