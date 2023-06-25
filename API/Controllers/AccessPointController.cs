using API.Services.AccessPoint;
using Domain.Models.DTOs;
using Domain.Models.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccessPointController : ControllerBase
    {
        private readonly IAccessPointService _accessPointService;

        public AccessPointController(IAccessPointService accessPointService)
        {
            _accessPointService = accessPointService;
        }

        [HttpGet("DEMO-ONLY-get-access-point-ids")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAccessPoints()
        {
            try
            {
                var accessPoints = await _accessPointService.GetAccessPoints();

                return Ok(accessPoints);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("get-access-point-history")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAccessPointEventHistory([FromQuery] Guid accessPointId)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                    return Unauthorized();

                var events = await _accessPointService.GetAccessEventsById(accessPointId, Guid.Parse(userId));

                return Ok(events);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("open-access-point")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> OpenAccessPoint([FromQuery] Guid accessPointId)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                    return Unauthorized();

                var userHasAccess = await _accessPointService.OpenAccessPoint(accessPointId, Guid.Parse(userId));

                if (userHasAccess)
                    return Ok();
                else
                    return Unauthorized();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            
        }

        [HttpPost("add-access-point")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddAccessPoint([FromBody] AccessPointRequestDTO accessPointDTO)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                    return Unauthorized();

                var response = await _accessPointService.AddAccessPoint(new AccessPointModel
                {
                    Name = accessPointDTO.Name,
                    RequiredAccessLevel = accessPointDTO.RequiredAccessLevel
                },
                Guid.Parse(userId));

                if (response.Id != Guid.Empty)
                    return Created("Success", accessPointDTO.Name);
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("delete-access-point")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAccessPoint([FromQuery] Guid accessPointId)
        {
            try
            {
                var success = await _accessPointService.DeleteAccessPoint(accessPointId);

                if (success)
                    return Ok();
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPatch("update-access-point")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateAccessPoint([FromBody] AccessPointRequestDTO accessPointDTO)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                    return Unauthorized();

                var success = await _accessPointService.UpdateAccessPoint(new AccessPointModel
                {
                    Id = accessPointDTO.Id,
                    Name = accessPointDTO.Name,
                    RequiredAccessLevel = accessPointDTO.RequiredAccessLevel
                },
                Guid.Parse(userId));

                if (success)
                    return Ok();
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPatch("register-user-to-access-point")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RegisterUser([FromBody] AccessPointRegistrationDTO registrationDTO)
        {          
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                    return Unauthorized();

                var success = await _accessPointService.RegisterUserToAccessPoint(registrationDTO.AccessPointId, registrationDTO.UserId, Guid.Parse(userId));

                if (success)
                    return Ok();
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPatch("unregister-user-to-access-point")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UnRegisterUser([FromBody] AccessPointRegistrationDTO registrationDTO)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                    return Unauthorized();

                var success = await _accessPointService.UnRegisterUserFromAccessPoint(registrationDTO.AccessPointId, registrationDTO.UserId, Guid.Parse(userId));

                if (success)
                    return Ok();
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
