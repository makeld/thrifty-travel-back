using System.Net;
using App.Contracts.BLL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App.DAL.EF;
using AutoMapper;
using Domain.Entities;
using Domain.Identity;
using Microsoft.AspNetCore.Identity;
using WebApp.Helpers;

namespace WebApp.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IAppBLL _bll;
        private readonly UserManager<AppUser> _userManager;
        private readonly PublicDTOBllMapper<App.DTO.v1_0.Trip, App.BLL.DTO.Trip> _mapper;

        public TripsController(AppDbContext context, IAppBLL bll, UserManager<AppUser> userManager, IMapper autoMapper)
        {
            _context = context;
            _bll = bll;
            _userManager = userManager;
            _mapper = new PublicDTOBllMapper<App.DTO.v1_0.Trip, App.BLL.DTO.Trip>(autoMapper);
        }

        // GET: api/Trips
        /// <summary>
        /// Return all trips visible to current user.
        /// </summary>
        /// <returns>IEnumerable<App.DTO.v1_0.Trip></returns>
        [HttpGet]
        [ProducesResponseType<IEnumerable<App.BLL.DTO.Trip>>((int) HttpStatusCode.OK)]
        // [ProducesResponseType((int) HttpStatusCode.Unauthorized)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<IEnumerable<App.DTO.v1_0.Trip>>> GetTrips()
        {
            var res = (await _bll.Trips.GetAllSortedAsync(
                Guid.Parse(_userManager.GetUserId(User))
                ))
                .Select(e => _mapper.Map(e));
            return Ok(res);
        }

        // GET: api/Trips/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Trip>> GetTrip(Guid id)
        {
            var trip = await _context.Trips.FindAsync(id);

            if (trip == null)
            {
                return NotFound();
            }

            return trip;
        }

        // PUT: api/Trips/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTrip(Guid id, Trip trip)
        {
            if (id != trip.Id)
            {
                return BadRequest();
            }

            _context.Entry(trip).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TripExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Trips
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Trip>> PostTrip(Trip trip)
        {
            _context.Trips.Add(trip);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTrip", new { id = trip.Id }, trip);
        }

        // DELETE: api/Trips/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrip(Guid id)
        {
            var trip = await _context.Trips.FindAsync(id);
            if (trip == null)
            {
                return NotFound();
            }

            _context.Trips.Remove(trip);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TripExists(Guid id)
        {
            return _context.Trips.Any(e => e.Id == id);
        }
    }
}
