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
    public class TripUsersController : Controller
    {
        private readonly AppDbContext _context;

        public TripUsersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: TripUsers
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.TripUsers.Include(t => t.AppUser).Include(t => t.Trip);
            return View(await appDbContext.ToListAsync());
        }

        // GET: TripUsers/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tripUser = await _context.TripUsers
                .Include(t => t.AppUser)
                .Include(t => t.Trip)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tripUser == null)
            {
                return NotFound();
            }

            return View(tripUser);
        }

        // GET: TripUsers/Create
        public IActionResult Create()
        {
            ViewData["AppUserId"] = new SelectList(_context.AppUsers, "Id", "FirstName");
            ViewData["TripId"] = new SelectList(_context.Trips, "Id", "Title");
            return View();
        }

        // POST: TripUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AppUserId,TripId,Id")] TripUser tripUser)
        {
            if (ModelState.IsValid)
            {
                tripUser.Id = Guid.NewGuid();
                _context.Add(tripUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AppUserId"] = new SelectList(_context.AppUsers, "Id", "FirstName", tripUser.AppUserId);
            ViewData["TripId"] = new SelectList(_context.Trips, "Id", "Title", tripUser.TripId);
            return View(tripUser);
        }

        // GET: TripUsers/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tripUser = await _context.TripUsers.FindAsync(id);
            if (tripUser == null)
            {
                return NotFound();
            }
            ViewData["AppUserId"] = new SelectList(_context.AppUsers, "Id", "FirstName", tripUser.AppUserId);
            ViewData["TripId"] = new SelectList(_context.Trips, "Id", "Title", tripUser.TripId);
            return View(tripUser);
        }

        // POST: TripUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("AppUserId,TripId,Id")] TripUser tripUser)
        {
            if (id != tripUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tripUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TripUserExists(tripUser.Id))
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
            ViewData["AppUserId"] = new SelectList(_context.AppUsers, "Id", "FirstName", tripUser.AppUserId);
            ViewData["TripId"] = new SelectList(_context.Trips, "Id", "Title", tripUser.TripId);
            return View(tripUser);
        }

        // GET: TripUsers/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tripUser = await _context.TripUsers
                .Include(t => t.AppUser)
                .Include(t => t.Trip)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tripUser == null)
            {
                return NotFound();
            }

            return View(tripUser);
        }

        // POST: TripUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var tripUser = await _context.TripUsers.FindAsync(id);
            if (tripUser != null)
            {
                _context.TripUsers.Remove(tripUser);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TripUserExists(Guid id)
        {
            return _context.TripUsers.Any(e => e.Id == id);
        }
    }
}
