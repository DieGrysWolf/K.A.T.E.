using Domain.Interfaces;
using Domain.Models.DTOs;
using Domain.Models.Entity;

namespace API.Services.AccessPoint
{
    public class AccessPointService : IAccessPointService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAccessPointRepository _accessPointRepository;
        private readonly IReportingRepository _reportingRepository;

        public AccessPointService(IUserRepository userRepository,
                                  IAccessPointRepository accessPointRepository,
                                  IReportingRepository reportingRepository)
        {
            _userRepository = userRepository;
            _accessPointRepository = accessPointRepository;
            _reportingRepository = reportingRepository;
        }

        public async Task<IEnumerable<AccessPointResponseDTO>> GetAccessPoints()
        {
            try
            {
                return await _accessPointRepository.GetAccessPoints();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<AccessEventModel>> GetAccessEventsById(Guid accessPointId, Guid userId)
        {
            try
            {
                var hasAccess = await CheckAccess(accessPointId, userId, isReportRequest: true);

                if (hasAccess)
                {
                    return await _reportingRepository.GetAccessEvents(accessPointId);
                }
                else
                    return new List<AccessEventModel>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> OpenAccessPoint(Guid accessPointId, Guid userId)
        {
            try
            {
                var hasAccess = await CheckAccess(accessPointId, userId);

                await _reportingRepository.AddAccessEvent(accessPointId, userId, hasAccess);

                return hasAccess;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<AccessPointModel> AddAccessPoint(AccessPointModel accessPoint, Guid userId)
        {
            try
            {
                var newAccessPoint = await _accessPointRepository.AddAccessPoint(accessPoint);
                var user = await _userRepository.GetUserById(userId);

                var success = await _accessPointRepository.RegisterUser(newAccessPoint.Id, user);

                return newAccessPoint;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<bool> DeleteAccessPoint(Guid accessPointId)
        {
            try
            {
                return _accessPointRepository.DeleteAccessPoint(accessPointId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> UpdateAccessPoint(AccessPointModel accessPoint, Guid userId)
        {
            try
            {
                var hasAccess = await CheckAccess(accessPoint.Id, userId);

                if (!hasAccess)
                    return false;

                return await _accessPointRepository.UpdateAccessPoint(accessPoint);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<bool> CheckAccess(Guid accessPointId, Guid userId, bool isReportRequest = false)
        {
            try
            {
                var hasAccess = false;

                var user = await _userRepository.GetUserById(userId);

                if (user is null)
                    return hasAccess;

                var accessPoint = await _accessPointRepository.GetAccessPoint(accessPointId);

                if (accessPoint is null)
                    return hasAccess;

                if (!accessPoint.RegisteredUsers.Any(ru => ru.Id == userId))
                    return hasAccess;

                // Making use of an Access Level scoring metric, a separate table to record access point and user combinations is also possible
                hasAccess = user.AccessLevel >= accessPoint.RequiredAccessLevel;

                // Utilizing basic multiple layer access checks
                if (isReportRequest)
                    hasAccess = user.HasReportAccess;

                return hasAccess;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> RegisterUserToAccessPoint(Guid accessPointId, Guid userId, Guid adminId)
        {
            try
            {
                var hasAccess = await CheckAccess(accessPointId, adminId, false);

                if (!hasAccess)
                    return false;

                var user = await _userRepository.GetUserById(userId);

                if (user is null)
                    return false;

                return await _accessPointRepository.RegisterUser(accessPointId, user);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> UnRegisterUserFromAccessPoint(Guid accessPointId, Guid userId, Guid adminId)
        {
            try
            {
                var hasAccess = await CheckAccess(accessPointId, adminId, false);

                if (!hasAccess)
                    return false;

                var user = await _userRepository.GetUserById(userId);

                if (user is null)
                    return false;

                return await _accessPointRepository.UnRegisterUser(accessPointId, user);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
