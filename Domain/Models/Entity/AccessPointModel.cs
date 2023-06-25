using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Entity
{
    public class AccessPointModel
    {
        public Guid Id { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Name { get; set; } = string.Empty;
        [Required]
        public int RequiredAccessLevel { get; set; }

        public ICollection<UserModel> RegisteredUsers { get; set; } = new List<UserModel>();
    }
}
