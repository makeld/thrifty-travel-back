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
    public class PhotosController : Controller
    {
        private readonly AppDbContext _context;

        public PhotosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: PhotoService
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Photos.Include(p => p.Expense).Include(p => p.Trip);
            return View(await appDbContext.ToListAsync());
        }

        // GET: PhotoService/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var photo = await _context.Photos
                .Include(p => p.Expense)
                .Include(p => p.Trip)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (photo == null)
            {
                return NotFound();
            }

            return View(photo);
        }

        // GET: PhotoService/Create
        public IActionResult Create()
        {
            ViewData["ExpenseId"] = new SelectList(_context.Expenses, "Id", "CurrencyCode");
            ViewData["TripId"] = new SelectList(_context.Trips, "Id", "Title");
            return View();
        }

        // POST: PhotoService/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TripId,ExpenseId,ImageUrl,Description,Id")] Photo photo)
        {
            if (ModelState.IsValid)
            {
                photo.Id = Guid.NewGuid();
                _context.Add(photo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ExpenseId"] = new SelectList(_context.Expenses, "Id", "CurrencyCode", photo.ExpenseId);
            ViewData["TripId"] = new SelectList(_context.Trips, "Id", "Title", photo.TripId);
            return View(photo);
        }

        // GET: PhotoService/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var photo = await _context.Photos.FindAsync(id);
            if (photo == null)
            {
                return NotFound();
            }
            ViewData["ExpenseId"] = new SelectList(_context.Expenses, "Id", "CurrencyCode", photo.ExpenseId);
            ViewData["TripId"] = new SelectList(_context.Trips, "Id", "Title", photo.TripId);
            return View(photo);
        }

        // POST: PhotoService/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("TripId,ExpenseId,ImageUrl,Description,Id")] Photo photo)
        {
            if (id != photo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(photo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PhotoExists(photo.Id))
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
            ViewData["ExpenseId"] = new SelectList(_context.Expenses, "Id", "CurrencyCode", photo.ExpenseId);
            ViewData["TripId"] = new SelectList(_context.Trips, "Id", "Title", photo.TripId);
            return View(photo);
        }

        // GET: PhotoService/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var photo = await _context.Photos
                .Include(p => p.Expense)
                .Include(p => p.Trip)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (photo == null)
            {
                return NotFound();
            }

            return View(photo);
        }

        // POST: PhotoService/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var photo = await _context.Photos.FindAsync(id);
            if (photo != null)
            {
                _context.Photos.Remove(photo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PhotoExists(Guid id)
        {
            return _context.Photos.Any(e => e.Id == id);
        }
    }
}
