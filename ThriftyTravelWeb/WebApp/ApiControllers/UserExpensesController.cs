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
    public class UserExpensesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserExpensesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/UserExpenses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserExpense>>> GetUserExpenses()
        {
            return await _context.UserExpenses.ToListAsync();
        }

        // GET: api/UserExpenses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserExpense>> GetUserExpense(Guid id)
        {
            var userExpense = await _context.UserExpenses.FindAsync(id);

            if (userExpense == null)
            {
                return NotFound();
            }

            return userExpense;
        }

        // PUT: api/UserExpenses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserExpense(Guid id, UserExpense userExpense)
        {
            if (id != userExpense.Id)
            {
                return BadRequest();
            }

            _context.Entry(userExpense).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExpenseExists(id))
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

        // POST: api/UserExpenses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserExpense>> PostUserExpense(UserExpense userExpense)
        {
            _context.UserExpenses.Add(userExpense);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserExpense", new { id = userExpense.Id }, userExpense);
        }

        // DELETE: api/UserExpenses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserExpense(Guid id)
        {
            var userExpense = await _context.UserExpenses.FindAsync(id);
            if (userExpense == null)
            {
                return NotFound();
            }

            _context.UserExpenses.Remove(userExpense);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExpenseExists(Guid id)
        {
            return _context.UserExpenses.Any(e => e.Id == id);
        }
    }
}
