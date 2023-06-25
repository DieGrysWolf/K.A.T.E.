using Domain.Models.DTOs;
using Domain.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IAccessPointRepository
    {
        Task<AccessPointResponseDTO> GetAccessPoint(Guid accessPointId);
        Task<IEnumerable<AccessPointResponseDTO>> GetAccessPoints();
        Task<AccessPointModel> AddAccessPoint(AccessPointModel accessPoint);

        Task<bool> DeleteAccessPoint(Guid accessPointId);
        Task<bool> UpdateAccessPoint(AccessPointModel accessPoint);
        Task<bool> UserIsRegisteredOnAccessPoint(Guid accessPointId, Guid userId);
        Task<bool> RegisterUser(Guid accessPointId, UserModel user);
        Task<bool> UnRegisterUser(Guid accessPointId, UserModel user);
    }
}
