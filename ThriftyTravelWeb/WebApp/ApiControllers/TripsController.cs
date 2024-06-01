using System.Net;
using App.Contracts.BLL;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
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
    public class TripsController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly UserManager<AppUser> _userManager;
        private readonly PublicDTOBllMapper<App.DTO.v1_0.Trip, App.BLL.DTO.Trip> _mapper;
        private readonly PublicDTOBllMapper<App.DTO.v1_0.AddTrip, App.BLL.DTO.AddTrip> _addTripMapper;

        public TripsController(IAppBLL bll, UserManager<AppUser> userManager, IMapper autoMapper)
        {
            _bll = bll;
            _userManager = userManager;
            _mapper = new PublicDTOBllMapper<App.DTO.v1_0.Trip, App.BLL.DTO.Trip>(autoMapper);
            _addTripMapper = new PublicDTOBllMapper<App.DTO.v1_0.AddTrip, App.BLL.DTO.AddTrip>(autoMapper);
        }

        
        /// <summary>
        /// Return all trips
        /// </summary>
        /// <returns>List of Trips</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<App.DTO.v1_0.Trip>), (int)HttpStatusCode.OK)]
        // [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [Produces("application/json")]
        [Consumes("application/json")]
        [AllowAnonymous]
        public async Task<ActionResult<List<App.DTO.v1_0.Trip>>> GetTrips()
        {
            var bllTripsResult = await _bll.TripService.GetAllAsync();
            var bllTrips = bllTripsResult.Select(e => _mapper.Map(e)).ToList();
            return Ok(bllTrips);
        }

        /// <summary>
        /// Return Trip by its ID
        /// </summary>
        /// <param name="id">Trip ID</param>
        /// <returns>Trip</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(App.DTO.v1_0.Trip), (int)HttpStatusCode.OK)]
        // [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [Produces("application/json")]
        [Consumes("application/json")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<App.DTO.v1_0.Trip>> GetTrip(Guid id)
        {
            var trip = await _bll.TripService.FirstOrDefaultAsync(id);
            Console.WriteLine(id);

            if (trip == null)
            {
                return NotFound();
            }

            var res = _mapper.Map(trip);

            return Ok(res);
        }
        
        
        /// <summary>
        /// Update Trip
        /// </summary>
        /// <param name="id">Trip ID</param>
        /// <param name="trip">Trip</param>
        /// <returns>NoContent</returns>
        [HttpPut("{id}")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [Produces("application/json")]
        [Consumes("application/json")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PutTrip(Guid id, [FromBody] App.DTO.v1_0.Trip trip)
        {
            if (id != trip.Id)
            {
                return BadRequest("The ID in the URL does not match the ID in the trip data.");
            }

            if (!await _bll.TripService.ExistsAsync(id))
            {
                return NotFound("The trip with the specified ID does not exist.");
            }

            var res = _mapper.Map(trip);
            res!.UpdatedAt = DateTime.Now;

            _bll.TripService.Update(res);
            await _bll.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Create new Trip
        /// </summary>
        /// <param name="trip">Trip</param>
        /// <returns>Trip</returns>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [ProducesResponseType<App.DTO.v1_0.Trip>((int) HttpStatusCode.Created)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<App.DTO.v1_0.Trip>> PostTrip(App.DTO.v1_0.AddTrip tripData)
        {
            var userId = Guid.Parse(_userManager.GetUserId(User)!);

            var mappedData = _addTripMapper.Map(tripData);

            var res= await _bll.TripService.CreateTripWithData(mappedData!, userId);
            await _bll.SaveChangesAsync();

            var publicTrip = _mapper.Map(res);
            return CreatedAtAction("GetTrip", new
            {
                version = HttpContext.GetRequestedApiVersion()?.ToString(),
                id = publicTrip!.Id
            }, publicTrip);
        }

        /// <summary>
        /// Delete Trip by ID
        /// </summary>
        /// <param name="id">Trip ID</param>
        /// <returns>NoContent</returns>
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [HttpDelete("{id}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteTrip(Guid id)
        {
            var trip = await _bll.TripService.FirstOrDefaultAsync(id);
            if (trip == null)
            {
                return NotFound();
            }
            
            var tripUsers = await _bll.TripUserService.GetAllTripUsersByTripId(id);
            foreach (var tripUser in tripUsers)
            {
                await _bll.TripUserService.RemoveAsync(tripUser!.Id);
            }
            
            var expenses = await _bll.ExpenseService.GetExpenseByTripId(id);
            foreach (var expense in expenses)
            {
                await _bll.ExpenseService.RemoveAsync(expense!.Id);
            }

            var photos = await _bll.PhotoService.GetAllPhotosByTripId(id);
            foreach (var photo in photos)
            {
                await _bll.PhotoService.RemoveAsync(photo!.Id);
            }
            
            var tripCategories = await _bll.TripCategoryService.GetAllTripCategoriesByTripId(id);
            foreach (var tripCategory in tripCategories)
            {
                await _bll.TripCategoryService.RemoveAsync(tripCategory!.Id);
            }
            
            var tripLocations = await _bll.TripLocationService.GetAllTripLocationsByTripId(id);
            foreach (var tripLocation in tripLocations)
            {
                await _bll.TripCategoryService.RemoveAsync(tripLocation!.Id);
            }

            await _bll.TripService.RemoveAsync(id);
            await _bll.SaveChangesAsync();

            return NoContent();
        }

        
        /// <summary>
        /// Check if Trip exists
        /// </summary>
        /// <param name="id">Trip ID</param>
        /// <returns>bool</returns>
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [HttpDelete("{id}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        private bool TripExists(Guid id)
        {
            return _bll.TripService.Exists(id);
        }
    }
}
