using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.DTOs.Auth
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public int AccessLevel { get; set; }
        public bool HasReportAccess { get; set; }
        public string EmailAddress { get; set; }
    }
}
