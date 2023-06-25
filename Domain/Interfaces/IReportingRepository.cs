using Domain.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IReportingRepository
    {
        Task<IEnumerable<AccessEventModel>> GetAccessEvents(Guid doorId);
        Task AddAccessEvent(Guid doorId, Guid userId, bool success);
    }
}
