using Domain.Models.DTOs;
using Domain.Models.Entity;

namespace API.Services.AccessPoint
{
    public interface IAccessPointService
    {
        Task<IEnumerable<AccessPointResponseDTO>> GetAccessPoints();
        Task<IEnumerable<AccessEventModel>> GetAccessEventsById(Guid accessPointId, Guid userId);
        Task<bool> OpenAccessPoint(Guid accessPointId, Guid userId);
        Task<AccessPointModel> AddAccessPoint(AccessPointModel accessPoint, Guid userId);
        Task<bool> DeleteAccessPoint(Guid accessPointId);
        Task<bool> UpdateAccessPoint(AccessPointModel accessPoint, Guid userId);
        Task<bool> RegisterUserToAccessPoint(Guid accessPointId, Guid userId, Guid adminId);
        Task<bool> UnRegisterUserFromAccessPoint(Guid accessPointId, Guid userId, Guid adminId);
    }
}