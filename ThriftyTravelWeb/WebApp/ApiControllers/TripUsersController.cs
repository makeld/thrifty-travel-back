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
    public class TripUsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TripUsersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/TripUsers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TripUser>>> GetTripUsers()
        {
            return await _context.TripUsers.ToListAsync();
        }

        // GET: api/TripUsers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TripUser>> GetTripUser(Guid id)
        {
            var tripUser = await _context.TripUsers.FindAsync(id);

            if (tripUser == null)
            {
                return NotFound();
            }

            return tripUser;
        }

        // PUT: api/TripUsers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTripUser(Guid id, TripUser tripUser)
        {
            if (id != tripUser.Id)
            {
                return BadRequest();
            }

            _context.Entry(tripUser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TripUserExists(id))
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

        // POST: api/TripUsers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TripUser>> PostTripUser(TripUser tripUser)
        {
            _context.TripUsers.Add(tripUser);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTripUser", new { id = tripUser.Id }, tripUser);
        }

        // DELETE: api/TripUsers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTripUser(Guid id)
        {
            var tripUser = await _context.TripUsers.FindAsync(id);
            if (tripUser == null)
            {
                return NotFound();
            }

            _context.TripUsers.Remove(tripUser);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TripUserExists(Guid id)
        {
            return _context.TripUsers.Any(e => e.Id == id);
        }
    }
}
