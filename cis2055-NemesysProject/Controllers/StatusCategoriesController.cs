using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using cis2055_NemesysProject.Data;
using cis2055_NemesysProject.Models;

namespace cis2055_NemesysProject.Controllers
{
    public class StatusCategoriesController : Controller
    {
        private readonly cis2055nemesysContext _context;

        public StatusCategoriesController(cis2055nemesysContext context)
        {
            _context = context;
        }

        // GET: StatusCategories
        public async Task<IActionResult> Index()
        {
            return View(await _context.StatusCategories.ToListAsync());
        }

        // GET: StatusCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var statusCategory = await _context.StatusCategories
                .FirstOrDefaultAsync(m => m.StatusId == id);
            if (statusCategory == null)
            {
                return NotFound();
            }

            return View(statusCategory);
        }

        // GET: StatusCategories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: StatusCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StatusId,StatusType")] StatusCategory statusCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(statusCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(statusCategory);
        }

        // GET: StatusCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var statusCategory = await _context.StatusCategories.FindAsync(id);
            if (statusCategory == null)
            {
                return NotFound();
            }
            return View(statusCategory);
        }

        // POST: StatusCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StatusId,StatusType")] StatusCategory statusCategory)
        {
            if (id != statusCategory.StatusId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(statusCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StatusCategoryExists(statusCategory.StatusId))
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
            return View(statusCategory);
        }

        // GET: StatusCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var statusCategory = await _context.StatusCategories
                .FirstOrDefaultAsync(m => m.StatusId == id);
            if (statusCategory == null)
            {
                return NotFound();
            }

            return View(statusCategory);
        }

        // POST: StatusCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var statusCategory = await _context.StatusCategories.FindAsync(id);
            _context.StatusCategories.Remove(statusCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StatusCategoryExists(int id)
        {
            return _context.StatusCategories.Any(e => e.StatusId == id);
        }
    }
}
