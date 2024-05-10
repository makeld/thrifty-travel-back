using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App.DAL.EF;
using Domain.Entities;

namespace WebApp.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripLocationsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TripLocationsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/TripLocationService
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TripLocation>>> GetTripLocations()
        {
            return await _context.TripLocations.ToListAsync();
        }

        // GET: api/TripLocationService/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TripLocation>> GetTripLocation(Guid id)
        {
            var tripLocation = await _context.TripLocations.FindAsync(id);

            if (tripLocation == null)
            {
                return NotFound();
            }

            return tripLocation;
        }

        // PUT: api/TripLocationService/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTripLocation(Guid id, TripLocation tripLocation)
        {
            if (id != tripLocation.Id)
            {
                return BadRequest();
            }

            _context.Entry(tripLocation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TripLocationExists(id))
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

        // POST: api/TripLocationService
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TripLocation>> PostTripLocation(TripLocation tripLocation)
        {
            _context.TripLocations.Add(tripLocation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTripLocation", new { id = tripLocation.Id }, tripLocation);
        }

        // DELETE: api/TripLocationService/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTripLocation(Guid id)
        {
            var tripLocation = await _context.TripLocations.FindAsync(id);
            if (tripLocation == null)
            {
                return NotFound();
            }

            _context.TripLocations.Remove(tripLocation);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TripLocationExists(Guid id)
        {
            return _context.TripLocations.Any(e => e.Id == id);
        }
    }
}
