using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.DTOs
{
    public class AddAccessPointDTO
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public int RequiredAccessLevel { get; set; }
    }
}
