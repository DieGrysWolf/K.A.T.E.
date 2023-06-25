using Domain.Models.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.DTOs
{
    public class AccessPointResponseDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int RequiredAccessLevel { get; set; }
        public List<UserDTO>? RegisteredUsers { get; set; }
    }
}
