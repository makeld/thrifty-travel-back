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
    public class TripCategoriesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TripCategoriesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/TripCategoryService
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TripCategory>>> GetTripCategories()
        {
            return await _context.TripCategories.ToListAsync();
        }

        // GET: api/TripCategoryService/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TripCategory>> GetTripCategory(Guid id)
        {
            var tripCategory = await _context.TripCategories.FindAsync(id);

            if (tripCategory == null)
            {
                return NotFound();
            }

            return tripCategory;
        }

        // PUT: api/TripCategoryService/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTripCategory(Guid id, TripCategory tripCategory)
        {
            if (id != tripCategory.Id)
            {
                return BadRequest();
            }

            _context.Entry(tripCategory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TripCategoryExists(id))
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

        // POST: api/TripCategoryService
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TripCategory>> PostTripCategory(TripCategory tripCategory)
        {
            _context.TripCategories.Add(tripCategory);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTripCategory", new { id = tripCategory.Id }, tripCategory);
        }

        // DELETE: api/TripCategoryService/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTripCategory(Guid id)
        {
            var tripCategory = await _context.TripCategories.FindAsync(id);
            if (tripCategory == null)
            {
                return NotFound();
            }

            _context.TripCategories.Remove(tripCategory);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TripCategoryExists(Guid id)
        {
            return _context.TripCategories.Any(e => e.Id == id);
        }
    }
}
