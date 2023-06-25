using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.DTOs.Auth
{
    public class SignupModel
    {
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; } = string.Empty;
        [Required]
        [StringLength(100, MinimumLength = 4)]
        public string FullName { get; set; } = string.Empty;
        [Required]
        [StringLength(24, MinimumLength = 4)]
        public string Password { get; set; } = string.Empty;
        [Required]
        public bool isAdmin { get; set; } = false;
        [Required]
        public int AccessLevel { get; set; }
    }
}
