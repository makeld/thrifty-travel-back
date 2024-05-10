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
    public class TripCategoriesController : Controller
    {
        private readonly AppDbContext _context;

        public TripCategoriesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: TripCategoryService
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.TripCategories.Include(t => t.Category).Include(t => t.Trip);
            return View(await appDbContext.ToListAsync());
        }

        // GET: TripCategoryService/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tripCategory = await _context.TripCategories
                .Include(t => t.Category)
                .Include(t => t.Trip)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tripCategory == null)
            {
                return NotFound();
            }

            return View(tripCategory);
        }

        // GET: TripCategoryService/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            ViewData["TripId"] = new SelectList(_context.Trips, "Id", "Title");
            return View();
        }

        // POST: TripCategoryService/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TripId,CategoryId,Id")] TripCategory tripCategory)
        {
            if (ModelState.IsValid)
            {
                tripCategory.Id = Guid.NewGuid();
                _context.Add(tripCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", tripCategory.CategoryId);
            ViewData["TripId"] = new SelectList(_context.Trips, "Id", "Title", tripCategory.TripId);
            return View(tripCategory);
        }

        // GET: TripCategoryService/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tripCategory = await _context.TripCategories.FindAsync(id);
            if (tripCategory == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", tripCategory.CategoryId);
            ViewData["TripId"] = new SelectList(_context.Trips, "Id", "Title", tripCategory.TripId);
            return View(tripCategory);
        }

        // POST: TripCategoryService/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("TripId,CategoryId,Id")] TripCategory tripCategory)
        {
            if (id != tripCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tripCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TripCategoryExists(tripCategory.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", tripCategory.CategoryId);
            ViewData["TripId"] = new SelectList(_context.Trips, "Id", "Title", tripCategory.TripId);
            return View(tripCategory);
        }

        // GET: TripCategoryService/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tripCategory = await _context.TripCategories
                .Include(t => t.Category)
                .Include(t => t.Trip)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tripCategory == null)
            {
                return NotFound();
            }

            return View(tripCategory);
        }

        // POST: TripCategoryService/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var tripCategory = await _context.TripCategories.FindAsync(id);
            if (tripCategory != null)
            {
                _context.TripCategories.Remove(tripCategory);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TripCategoryExists(Guid id)
        {
            return _context.TripCategories.Any(e => e.Id == id);
        }
    }
}
