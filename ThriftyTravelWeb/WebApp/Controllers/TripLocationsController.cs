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
    public class TripLocationsController : Controller
    {
        private readonly AppDbContext _context;

        public TripLocationsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: TripLocationService
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.TripLocations.Include(t => t.Location).Include(t => t.Trip);
            return View(await appDbContext.ToListAsync());
        }

        // GET: TripLocationService/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tripLocation = await _context.TripLocations
                .Include(t => t.Location)
                .Include(t => t.Trip)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tripLocation == null)
            {
                return NotFound();
            }

            return View(tripLocation);
        }

        // GET: TripLocationService/Create
        public IActionResult Create()
        {
            ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "LocationName");
            ViewData["TripId"] = new SelectList(_context.Trips, "Id", "Title");
            return View();
        }

        // POST: TripLocationService/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TripId,LocationId,Id")] TripLocation tripLocation)
        {
            if (ModelState.IsValid)
            {
                tripLocation.Id = Guid.NewGuid();
                _context.Add(tripLocation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "LocationName", tripLocation.LocationId);
            ViewData["TripId"] = new SelectList(_context.Trips, "Id", "Title", tripLocation.TripId);
            return View(tripLocation);
        }

        // GET: TripLocationService/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tripLocation = await _context.TripLocations.FindAsync(id);
            if (tripLocation == null)
            {
                return NotFound();
            }
            ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "LocationName", tripLocation.LocationId);
            ViewData["TripId"] = new SelectList(_context.Trips, "Id", "Title", tripLocation.TripId);
            return View(tripLocation);
        }

        // POST: TripLocationService/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("TripId,LocationId,Id")] TripLocation tripLocation)
        {
            if (id != tripLocation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tripLocation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TripLocationExists(tripLocation.Id))
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
            ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "LocationName", tripLocation.LocationId);
            ViewData["TripId"] = new SelectList(_context.Trips, "Id", "Title", tripLocation.TripId);
            return View(tripLocation);
        }

        // GET: TripLocationService/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tripLocation = await _context.TripLocations
                .Include(t => t.Location)
                .Include(t => t.Trip)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tripLocation == null)
            {
                return NotFound();
            }

            return View(tripLocation);
        }

        // POST: TripLocationService/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var tripLocation = await _context.TripLocations.FindAsync(id);
            if (tripLocation != null)
            {
                _context.TripLocations.Remove(tripLocation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TripLocationExists(Guid id)
        {
            return _context.TripLocations.Any(e => e.Id == id);
        }
    }
}
