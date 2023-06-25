using Domain.Interfaces;
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
    public class ReportingRepository : IReportingRepository
    {
        private readonly MariaDBContext _context;
        private readonly ILogger<ReportingRepository> _logger;

        public ReportingRepository(MariaDBContext context, ILogger<ReportingRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<AccessEventModel>> GetAccessEvents(Guid doorId)
        {
            try
            {
                return await _context.AccessEvents
                .Where(e => e.DoorId == doorId)
                .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception($"An error occurred while retrieving access events for door with ID {doorId}: {ex.Message}", ex);
            }
        }

        public async Task AddAccessEvent(Guid doorId, Guid userId, bool success)
        {
            try
            {
                var accessEvent = new AccessEventModel
                {
                    DoorId = doorId,
                    UserId = userId,
                    Success = success,
                    EventTime = DateTime.UtcNow
                };

                await _context.AccessEvents.AddAsync(accessEvent);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception($"An error occurred while adding the access event: {ex.Message}", ex);
            }
        }
    }
}
