using System.Net;
using App.Contracts.BLL;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Domain.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using WebApp.Helpers;

namespace WebApp.ApiControllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TripUsersController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly UserManager<AppUser> _userManager;
        private readonly PublicDTOBllMapper<App.DTO.v1_0.TripUser, App.BLL.DTO.TripUser> _mapper;
        
        public TripUsersController(IAppBLL bll, UserManager<AppUser> userManager, IMapper autoMapper)
        {
            _bll = bll;
            _userManager = userManager;
            _mapper = new PublicDTOBllMapper<App.DTO.v1_0.TripUser, App.BLL.DTO.TripUser>(autoMapper);
        }

        /// <summary>
        /// Return all trip Trip Users
        /// </summary>
        /// <returns>List of TripUsers</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<App.DTO.v1_0.TripUser>), (int)HttpStatusCode.OK)]
        // [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<List<App.DTO.v1_0.TripUser>>> GetTripUsers()
        {
            var bllResult = await _bll.TripUserService.GetAllAsync();
            var mapped = bllResult.Select(e => _mapper.Map(e)).ToList();
            return Ok(mapped);
        }

        /// <summary>
        /// Return TripUser by its ID
        /// </summary>
        /// <param name="id">TripUser ID</param>
        /// <returns>TripUser</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(App.DTO.v1_0.TripUser), (int)HttpStatusCode.OK)]
        // [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<App.DTO.v1_0.TripUser>> GetTripUser(Guid id)
        {
            var tripUser = await _bll.TripUserService.FirstOrDefaultAsync(id);

            if (tripUser == null)
            {
                return NotFound();
            }

            var res = _mapper.Map(tripUser);

            return Ok(res);
        }

        /// <summary>
        /// Update TripUser
        /// </summary>
        /// <param name="id">TripUser ID</param>
        /// <param name="tripUser">TripUser</param>
        /// <returns>NoContent</returns>
        [HttpPut("{id}")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<IActionResult> PutTripUser(Guid id, App.DTO.v1_0.TripUser tripUser)
        {
            if (id != tripUser.Id)
            {
                return BadRequest("The ID in the URL does not match the ID in the tripUser data.");
            }

            if (!await _bll.TripUserService.ExistsAsync(id))
            {
                return NotFound("The tripUser with the specified ID does not exist.");
            }
            
            var res = _mapper.Map(tripUser);

            _bll.TripUserService.Update(res);
            
            return NoContent();
        }

        
        /// <summary>
        /// Create new TripUser
        /// </summary>
        /// <param name="tripUser"></param>
        /// <returns>NoContent</returns>
        [HttpPost]
        [ProducesResponseType<App.DTO.v1_0.TripUser>((int)HttpStatusCode.Created)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<App.DTO.v1_0.TripUser>> PostTripUser(App.DTO.v1_0.TripUser tripUser)
        {
            var mappedTripUser = _mapper.Map(tripUser);
            _bll.TripUserService.Add(mappedTripUser);

            return CreatedAtAction("GetTripUser", new
            {
                version = HttpContext.GetRequestedApiVersion()?.ToString(),
                id = tripUser.Id
            }, tripUser);
        }

         
        /// <summary>
        /// Delete TripUser by ID
        /// </summary>
        /// <param name="id">TripUser ID</param>
        /// <returns>NoContent</returns>
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpDelete("{id}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<IActionResult> DeleteTripUser(Guid id)
        {
            var tripUser = await _bll.TripUserService.FirstOrDefaultAsync(id);
            
            if (tripUser == null)
            {
                return NotFound();
            }

            await _bll.TripUserService.RemoveAsync(id);

            return NoContent();
        }


        /// <summary>
        /// Check if TripUser exists
        /// </summary>
        /// <param name="id">TripUser ID</param>
        /// <returns>bool</returns>
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpDelete("{id}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        private bool TripUserExists(Guid id)
        {
            return _bll.TripUserService.Exists(id);
        }
    }
}
