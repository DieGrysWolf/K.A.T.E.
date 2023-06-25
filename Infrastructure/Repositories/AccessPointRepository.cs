using Domain.Interfaces;
using Domain.Models.DTOs;
using Domain.Models.DTOs.Auth;
using Domain.Models.Entity;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class AccessPointRepository : IAccessPointRepository
    {
        private readonly MariaDBContext _context;
        private readonly ILogger<AccessPointRepository> _logger;

        public AccessPointRepository(MariaDBContext context, ILogger<AccessPointRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<AccessPointResponseDTO> GetAccessPoint(Guid accessPointId)
        {
            try
            {
                var accessPoint = await _context.AccessPoints.Where(ap => ap.Id == accessPointId)
                    .Include(ap => ap.RegisteredUsers)
                    .FirstOrDefaultAsync();

                var accessPointDTO = new AccessPointResponseDTO
                {
                    Id = accessPointId,
                    Name = accessPoint.Name,
                    RequiredAccessLevel = accessPoint.RequiredAccessLevel,
                    RegisteredUsers = accessPoint.RegisteredUsers.Select(u => new UserDTO
                    {
                        Id = u.Id,
                        FullName = u.FullName,
                        AccessLevel = u.AccessLevel,
                        HasReportAccess = u.HasReportAccess,
                        EmailAddress = u.EmailAddress
                    }).ToList()
                };

                return accessPointDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception($"An error occurred while retrieving access point with ID {accessPointId}: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<AccessPointResponseDTO>> GetAccessPoints()
        {
            try
            {
                var accessPoints = await _context.AccessPoints
                    .Include(ap => ap.RegisteredUsers)
                    .ToListAsync();

                var accessPointDtos = accessPoints.Select(ap => new AccessPointResponseDTO
                {
                    Id = ap.Id,
                    Name = ap.Name,
                    RequiredAccessLevel = ap.RequiredAccessLevel,
                    RegisteredUsers = ap.RegisteredUsers.Select(u => new UserDTO
                    {
                        Id = u.Id,
                        FullName = u.FullName,
                        AccessLevel = u.AccessLevel,
                        HasReportAccess = u.HasReportAccess,
                        EmailAddress = u.EmailAddress
                    }).ToList()
                }).ToList(); // Execute the Select operation immediately

                return accessPointDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception($"An error occurred while retrieving access points: {ex.Message}", ex);
            }
        }


        public async Task<AccessPointModel> AddAccessPoint(AccessPointModel accessPoint)
        {
            try
            {
                var newAccessPoint = await _context.AccessPoints.AddAsync(accessPoint);
                await _context.SaveChangesAsync();

                return newAccessPoint.Entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception($"An error occurred while adding the access point: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteAccessPoint(Guid accessPointId)
        {
            try
            {
                var accessPoint = await _context.AccessPoints.FindAsync(accessPointId);

                if (accessPoint is null)
                    return false;

                _context.AccessPoints.Remove(accessPoint);
                var entries = await _context.SaveChangesAsync();

                return entries > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception($"An error occurred while removing the access point: {ex.Message}", ex);
            }
        }

        public async Task<bool> UpdateAccessPoint(AccessPointModel accessPoint)
        {
            try
            {
                var dbEntry = await _context.AccessPoints.FindAsync(accessPoint.Id);

                if (dbEntry is null)
                    return false;

                dbEntry.Name = accessPoint.Name;
                dbEntry.RequiredAccessLevel = accessPoint.RequiredAccessLevel;

                _context.AccessPoints.Update(dbEntry);
                var entries = await _context.SaveChangesAsync();

                return entries > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception($"An error occurred while updating the access point: {ex.Message}", ex);
            }
        }

        public async Task<bool> UserIsRegisteredOnAccessPoint(Guid accessPointId, Guid userId)
        {
            try
            {
                var accessPoint = await _context.AccessPoints
                    .Where(ap => ap.Id == accessPointId)
                    .Include(ap => ap.RegisteredUsers)
                    .FirstOrDefaultAsync();

                if (accessPoint is null)
                    return false;

                return accessPoint.RegisteredUsers?.Any(ru => ru.Id == userId) ?? false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception($"An error occurred while checking user with ID {userId} registration on access point with ID {accessPointId}: {ex.Message}", ex);
            }
        }

        public async Task<bool> RegisterUser(Guid accessPointId, UserModel user)
        {
            try
            {
                var dbEntry = await _context.AccessPoints.FindAsync(accessPointId);

                if (dbEntry is null)
                    return false;

                dbEntry.RegisteredUsers?.Add(user);

                _context.AccessPoints.Update(dbEntry);
                var entries = await _context.SaveChangesAsync();

                return entries > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception($"An error occurred while updating the access point: {ex.Message}", ex);
            }
        }

        public async Task<bool> UnRegisterUser(Guid accessPointId, UserModel user)
        {
            try
            {
                var dbEntry = await _context.AccessPoints.FindAsync(accessPointId);

                if (dbEntry is null)
                    return false;

                dbEntry.RegisteredUsers?.Remove(user);

                _context.AccessPoints.Update(dbEntry);
                var entries = await _context.SaveChangesAsync();

                return entries > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception($"An error occurred while updating the access point: {ex.Message}", ex);
            }
        }
    }
}
