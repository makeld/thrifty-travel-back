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
    public class LocationsController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly UserManager<AppUser> _userManager;
        private readonly PublicDTOBllMapper<App.DTO.v1_0.Location, App.BLL.DTO.Location> _mapper;
        
        public LocationsController(IAppBLL bll, UserManager<AppUser> userManager, IMapper autoMapper)
        {
            _bll = bll;
            _userManager = userManager;
            _mapper = new PublicDTOBllMapper<App.DTO.v1_0.Location, App.BLL.DTO.Location>(autoMapper);
        }

        /// <summary>
        /// Return all locations
        /// </summary>
        /// <returns>List of Locations</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<App.DTO.v1_0.Location>), (int)HttpStatusCode.OK)]
        // [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<List<App.DTO.v1_0.Location>>> GetLocations()
        {
            var bllResult = await _bll.LocationService.GetAllAsync();
            var mapped = bllResult.Select(e => _mapper.Map(e)).ToList();
            return Ok(mapped);
        }

        /// <summary>
        /// Return Location by its ID
        /// </summary>
        /// <param name="id">Location ID</param>
        /// <returns>Location</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(App.DTO.v1_0.Location), (int)HttpStatusCode.OK)]
        // [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<App.DTO.v1_0.Location>> GetLocation(Guid id)
        {
            var location = await _bll.LocationService.FirstOrDefaultAsync(id);

            if (location == null)
            {
                return NotFound();
            }

            var res = _mapper.Map(location);

            return Ok(res);
        }

        
        /// <summary>
        /// Update Location
        /// </summary>
        /// <param name="id">Location ID</param>
        /// <param name="location">Location</param>
        /// <returns>NoContent</returns>
        [HttpPut("{id}")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<IActionResult> PutLocation(Guid id, App.DTO.v1_0.Location location)
        {
            if (id != location.Id)
            {
                return BadRequest("The ID in the URL does not match the ID in the location data.");
            }

            if (!await _bll.LocationService.ExistsAsync(id))
            {
                return NotFound("The location with the specified ID does not exist.");
            }
            
            var res = _mapper.Map(location);

            _bll.LocationService.Update(res);
            
            return NoContent();
        }
        

        /// <summary>
        /// Create new Location
        /// </summary>
        /// <param name="location"></param>
        /// <returns>NoContent</returns>
        [HttpPost]
        [ProducesResponseType<App.DTO.v1_0.Location>((int)HttpStatusCode.Created)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<App.DTO.v1_0.Location>> PostLocation(App.DTO.v1_0.Location location)
        {
            var mappedLocation = _mapper.Map(location);
            mappedLocation!.Id = Guid.NewGuid();
            _bll.LocationService.Add(mappedLocation);
            await _bll.SaveChangesAsync();

            return CreatedAtAction("GetLocation", new
            {
                version = HttpContext.GetRequestedApiVersion()?.ToString(),
                id = mappedLocation.Id
            }, mappedLocation);
        }


        /// <summary>
        /// Delete Location by ID
        /// </summary>
        /// <param name="id">Location ID</param>
        /// <returns>NoContent</returns>
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpDelete("{id}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<IActionResult> DeleteLocation(Guid id)
        {
            var location = await _bll.LocationService.FirstOrDefaultAsync(id);
            
            if (location == null)
            {
                return NotFound();
            }

            await _bll.LocationService.RemoveAsync(id);

            return NoContent();
        }
    }
}
