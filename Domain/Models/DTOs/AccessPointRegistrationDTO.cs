using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.DTOs
{
    public class AccessPointRegistrationDTO
    {
        public Guid AccessPointId { get; set; }
        public Guid UserId { get; set; }
    }
}
