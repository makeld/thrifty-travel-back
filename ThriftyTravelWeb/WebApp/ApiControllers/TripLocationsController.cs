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
   // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TripLocationsController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly UserManager<AppUser> _userManager;
        private readonly PublicDTOBllMapper<App.DTO.v1_0.TripLocation, App.BLL.DTO.TripLocation> _mapper;
        
        public TripLocationsController(IAppBLL bll, UserManager<AppUser> userManager, IMapper autoMapper)
        {
            _bll = bll;
            _userManager = userManager;
            _mapper = new PublicDTOBllMapper<App.DTO.v1_0.TripLocation, App.BLL.DTO.TripLocation>(autoMapper);
        }

        /// <summary>
        /// Return all trip TripLocations
        /// </summary>
        /// <returns>List of TripLocations</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<App.DTO.v1_0.TripLocation>), (int)HttpStatusCode.OK)]
        // [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<List<App.DTO.v1_0.TripLocation>>> GetTripLocations()
        {
            var bllResult = await _bll.TripLocationService.GetAllAsync();
            var mapped = bllResult.Select(e => _mapper.Map(e)).ToList();
            return Ok(mapped);
        }

        
        /// <summary>
        /// Return TripLocation by its ID
        /// </summary>
        /// <param name="id">TripLocation ID</param>
        /// <returns>TripLocation</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(App.DTO.v1_0.TripLocation), (int)HttpStatusCode.OK)]
        // [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<App.DTO.v1_0.TripLocation>> GetTripLocation(Guid id)
        {
            var tripLocation = await _bll.TripLocationService.FirstOrDefaultAsync(id);

            if (tripLocation == null)
            {
                return NotFound();
            }

            var res = _mapper.Map(tripLocation);

            return Ok(res);
        }

        
        /// <summary>
        /// Update TripLocation
        /// </summary>
        /// <param name="id">TripLocation ID</param>
        /// <param name="tripLocation">TripLocation</param>
        /// <returns>NoContent</returns>
        [HttpPut("{id}")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<IActionResult> PutTripLocation(Guid id, App.DTO.v1_0.TripLocation tripLocation)
        {
            if (id != tripLocation.Id)
            {
                return BadRequest("The ID in the URL does not match the ID in the tripLocation data.");
            }

            if (!await _bll.TripLocationService.ExistsAsync(id))
            {
                return NotFound("The tripLocation with the specified ID does not exist.");
            }
            
            var res = _mapper.Map(tripLocation);

            _bll.TripLocationService.Update(res);
            
            return NoContent();
        }

        /// <summary>
        /// Create new TripLocation
        /// </summary>
        /// <param name="tripLocation"></param>
        /// <returns>NoContent</returns>
        [HttpPost]
        [ProducesResponseType<App.DTO.v1_0.TripLocation>((int)HttpStatusCode.Created)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<App.DTO.v1_0.TripLocation>> PostTripLocation(App.DTO.v1_0.TripLocation tripLocation)
        {
            var mappedTripLocation = _mapper.Map(tripLocation);
            mappedTripLocation!.Id = Guid.NewGuid();
            _bll.TripLocationService.Add(mappedTripLocation);
            await _bll.SaveChangesAsync();

            return CreatedAtAction("GetTripLocation", new
            {
                version = HttpContext.GetRequestedApiVersion()?.ToString(),
                id = mappedTripLocation.Id
            }, mappedTripLocation);
        }

        /// <summary>
        /// Delete TripLocation by ID
        /// </summary>
        /// <param name="id">TripLocation ID</param>
        /// <returns>NoContent</returns>
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpDelete("{id}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<IActionResult> DeleteTripLocation(Guid id)
        {
            var tripLocation = await _bll.TripLocationService.FirstOrDefaultAsync(id);
            
            if (tripLocation == null)
            {
                return NotFound();
            }

            await _bll.TripLocationService.RemoveAsync(id);

            return NoContent();
        }


        /// <summary>
        /// Check if TripLocation exists
        /// </summary>
        /// <param name="id">TripLocation ID</param>
        /// <returns>bool</returns>
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpDelete("{id}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        private bool TripLocationExists(Guid id)
        {
            return _bll.TripLocationService.Exists(id);
        }
    }
}
