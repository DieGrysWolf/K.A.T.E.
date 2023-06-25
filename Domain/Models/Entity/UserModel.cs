using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Entity
{
    public class UserModel
    {
        public Guid Id { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 4)]
        public string FullName { get; set; } = string.Empty;
        [Required]
        public int AccessLevel { get; set; }
        public bool HasReportAccess { get; set; } = false;

        // Authentication
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; } = string.Empty;
        public byte[]? PasswordHash { get; set; }
        public byte[]? PasswordSalt { get; set; }
        public  ICollection<Role>? Roles { get; set; }
        public ICollection<AccessPointModel>? RegisteredAccessPoints { get; set; }
    }
}
