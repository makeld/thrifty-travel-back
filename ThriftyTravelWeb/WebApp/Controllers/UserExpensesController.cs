using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.DAL.EF;
using Domain.Entities;

namespace WebApp.Controllers
{
    public class UserExpensesController : Controller
    {
        private readonly AppDbContext _context;

        public UserExpensesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: UserExpenses
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.UserExpenses.Include(u => u.Expense).Include(u => u.TripUser);
            return View(await appDbContext.ToListAsync());
        }

        // GET: UserExpenses/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userExpense = await _context.UserExpenses
                .Include(u => u.Expense)
                .Include(u => u.TripUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userExpense == null)
            {
                return NotFound();
            }

            return View(userExpense);
        }

        // GET: UserExpenses/Create
        public IActionResult Create()
        {
            ViewData["ExpenseId"] = new SelectList(_context.Expenses, "Id", "CurrencyCode");
            ViewData["TripUserId"] = new SelectList(_context.TripUsers, "Id", "Id");
            return View();
        }

        // POST: UserExpenses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ExpenseId,TripUserId,Id")] UserExpense userExpense)
        {
            if (ModelState.IsValid)
            {
                userExpense.Id = Guid.NewGuid();
                _context.Add(userExpense);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ExpenseId"] = new SelectList(_context.Expenses, "Id", "CurrencyCode", userExpense.ExpenseId);
            ViewData["TripUserId"] = new SelectList(_context.TripUsers, "Id", "Id", userExpense.TripUserId);
            return View(userExpense);
        }

        // GET: UserExpenses/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userExpense = await _context.UserExpenses.FindAsync(id);
            if (userExpense == null)
            {
                return NotFound();
            }
            ViewData["ExpenseId"] = new SelectList(_context.Expenses, "Id", "CurrencyCode", userExpense.ExpenseId);
            ViewData["TripUserId"] = new SelectList(_context.TripUsers, "Id", "Id", userExpense.TripUserId);
            return View(userExpense);
        }

        // POST: UserExpenses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ExpenseId,TripUserId,Id")] UserExpense userExpense)
        {
            if (id != userExpense.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userExpense);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExpenseExists(userExpense.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ExpenseId"] = new SelectList(_context.Expenses, "Id", "CurrencyCode", userExpense.ExpenseId);
            ViewData["TripUserId"] = new SelectList(_context.TripUsers, "Id", "Id", userExpense.TripUserId);
            return View(userExpense);
        }

        // GET: UserExpenses/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userExpense = await _context.UserExpenses
                .Include(u => u.Expense)
                .Include(u => u.TripUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userExpense == null)
            {
                return NotFound();
            }

            return View(userExpense);
        }

        // POST: UserExpenses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var userExpense = await _context.UserExpenses.FindAsync(id);
            if (userExpense != null)
            {
                _context.UserExpenses.Remove(userExpense);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExpenseExists(Guid id)
        {
            return _context.UserExpenses.Any(e => e.Id == id);
        }
    }
}
