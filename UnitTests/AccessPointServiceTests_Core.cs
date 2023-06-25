using API.Services.AccessPoint;
using Domain.Interfaces;
using Domain.Models.DTOs;
using Domain.Models.DTOs.Auth;
using Domain.Models.Entity;
using Infrastructure.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests
{
    public class AccessPointServiceTests_Core
    {
        private Mock<IAccessPointRepository> _accessPointRepositoryMock;
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IReportingRepository> _reportingRepositoryMock;
        private AccessPointService _accessPointService;

        public AccessPointServiceTests_Core()
        {
            _accessPointRepositoryMock = new Mock<IAccessPointRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _reportingRepositoryMock = new Mock<IReportingRepository>();
            _accessPointService = new AccessPointService(_userRepositoryMock.Object, _accessPointRepositoryMock.Object, _reportingRepositoryMock.Object);
        }

        [Fact]
        public async Task OpenAccessPoint_ShouldReturnFalse_WhenUserHasInsufficientAccessLevel()
        {
            // Arrange
            var accessPointId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            var registeredUsers = new List<UserDTO>
            {
                new UserDTO { Id = userId, AccessLevel = 4, HasReportAccess = false }
            };

            _accessPointRepositoryMock.Setup(r => r.GetAccessPoint(accessPointId))
                .ReturnsAsync(new AccessPointResponseDTO { Id = accessPointId, RequiredAccessLevel = 5, RegisteredUsers = registeredUsers });

            _userRepositoryMock.Setup(r => r.GetUserById(userId))
                .ReturnsAsync(new UserModel { Id = userId, AccessLevel = 4 });

            // Act
            var result = await _accessPointService.OpenAccessPoint(accessPointId, userId);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task OpenAccessPoint_ShouldReturnTrue_WhenUserHasSufficientAccessLevel()
        {
            // Arrange
            var accessPointId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            var registeredUsers = new List<UserDTO>
            {
                new UserDTO { Id = userId, AccessLevel = 4, HasReportAccess = false }
            };

            _accessPointRepositoryMock.Setup(r => r.GetAccessPoint(accessPointId))
                .ReturnsAsync(new AccessPointResponseDTO { Id = accessPointId, RequiredAccessLevel = 3, RegisteredUsers = registeredUsers });

            _userRepositoryMock.Setup(r => r.GetUserById(userId))
                .ReturnsAsync(new UserModel { Id = userId, AccessLevel = 4 });

            // Act
            var result = await _accessPointService.OpenAccessPoint(accessPointId, userId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task GetAccessEventsById_ReturnsCorrectData_WhenAccessIsAllowed()
        {
            // Arrange
            var testAccessPointId = Guid.NewGuid();
            var testUserId = Guid.NewGuid();

            var expectedEvents = new List<AccessEventModel>
            {
                new AccessEventModel { DoorId = Guid.NewGuid() },
                new AccessEventModel { DoorId = Guid.NewGuid() }
            };

            var user = new UserModel
            {
                Id = testUserId,
                AccessLevel = 5,
                HasReportAccess = true
            };

            var accessPoint = new AccessPointResponseDTO
            {
                Id = testAccessPointId,
                RequiredAccessLevel = 4,
                RegisteredUsers = new List<UserDTO>
                {
                    new UserDTO
                    {
                        Id = testUserId,
                        AccessLevel = 5,
                        HasReportAccess = true
                    }
                }
            };

            _userRepositoryMock.Setup(r => r.GetUserById(testUserId)).ReturnsAsync(user);
            _accessPointRepositoryMock.Setup(r => r.GetAccessPoint(testAccessPointId)).ReturnsAsync(accessPoint);
            _reportingRepositoryMock.Setup(r => r.GetAccessEvents(testAccessPointId)).ReturnsAsync(expectedEvents);

            // Act
            var result = await _accessPointService.GetAccessEventsById(testAccessPointId, testUserId);

            // Assert
            Assert.Equal(expectedEvents, result);
            _reportingRepositoryMock.Verify(r => r.GetAccessEvents(testAccessPointId), Times.Once);
        }

        [Fact]
        public async Task GetAccessEventsById_ReturnsNoDataData_WhenAccessIsNotAllowedAtAccessLevel()
        {
            // Arrange
            var testAccessPointId = Guid.NewGuid();
            var testUserId = Guid.NewGuid();

            var user = new UserModel
            {
                Id = testUserId,
                AccessLevel = 3,
                HasReportAccess = true
            };

            var accessPoint = new AccessPointResponseDTO
            {
                Id = testAccessPointId,
                RequiredAccessLevel = 4,
                RegisteredUsers = new List<UserDTO>
                {
                    new UserDTO
                    {
                        Id = testUserId,
                        AccessLevel = 3,
                        HasReportAccess = true
                    }
                }
            };

            _userRepositoryMock.Setup(r => r.GetUserById(testUserId)).ReturnsAsync(user);
            _accessPointRepositoryMock.Setup(r => r.GetAccessPoint(testAccessPointId)).ReturnsAsync(accessPoint);
            _reportingRepositoryMock.Setup(r => r.GetAccessEvents(testAccessPointId)).ReturnsAsync(new List<AccessEventModel>());

            // Act
            var result = await _accessPointService.GetAccessEventsById(testAccessPointId, testUserId);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAccessEventsById_ReturnsNoDataData_WhenAccessIsNotAllowedAtReportAccess()
        {
            // Arrange
            var testAccessPointId = Guid.NewGuid();
            var testUserId = Guid.NewGuid();

            var user = new UserModel
            {
                Id = testUserId,
                AccessLevel = 5,
                HasReportAccess = false
            };

            var accessPoint = new AccessPointResponseDTO
            {
                Id = testAccessPointId,
                RequiredAccessLevel = 4,
                RegisteredUsers = new List<UserDTO>
                {
                    new UserDTO
                    {
                        Id = testUserId,
                        AccessLevel = 5,
                        HasReportAccess = false
                    }
                }
            };

            _userRepositoryMock.Setup(r => r.GetUserById(testUserId)).ReturnsAsync(user);
            _accessPointRepositoryMock.Setup(r => r.GetAccessPoint(testAccessPointId)).ReturnsAsync(accessPoint);
            _reportingRepositoryMock.Setup(r => r.GetAccessEvents(testAccessPointId)).ReturnsAsync(new List<AccessEventModel>());

            // Act
            var result = await _accessPointService.GetAccessEventsById(testAccessPointId, testUserId);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}
