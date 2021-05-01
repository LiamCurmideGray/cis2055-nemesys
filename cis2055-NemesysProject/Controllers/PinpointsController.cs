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
    public class PinpointsController : Controller
    {
        private readonly cis2055nemesysContext _context;

        public PinpointsController(cis2055nemesysContext context)
        {
            _context = context;
        }

        // GET: Pinpoints
        public async Task<IActionResult> Index()
        {
            return View(await _context.Pinpoints.ToListAsync());
        }

        // GET: Pinpoints/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pinpoint = await _context.Pinpoints
                .FirstOrDefaultAsync(m => m.PinpointId == id);
            if (pinpoint == null)
            {
                return NotFound();
            }

            return View(pinpoint);
        }

        // GET: Pinpoints/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Pinpoints/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PinpointId,Latitude,Longitude")] Pinpoint pinpoint)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pinpoint);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pinpoint);
        }

        // GET: Pinpoints/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pinpoint = await _context.Pinpoints.FindAsync(id);
            if (pinpoint == null)
            {
                return NotFound();
            }
            return View(pinpoint);
        }

        // POST: Pinpoints/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PinpointId,Latitude,Longitude")] Pinpoint pinpoint)
        {
            if (id != pinpoint.PinpointId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pinpoint);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PinpointExists(pinpoint.PinpointId))
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
            return View(pinpoint);
        }

        // GET: Pinpoints/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pinpoint = await _context.Pinpoints
                .FirstOrDefaultAsync(m => m.PinpointId == id);
            if (pinpoint == null)
            {
                return NotFound();
            }

            return View(pinpoint);
        }

        // POST: Pinpoints/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pinpoint = await _context.Pinpoints.FindAsync(id);
            _context.Pinpoints.Remove(pinpoint);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PinpointExists(int id)
        {
            return _context.Pinpoints.Any(e => e.PinpointId == id);
        }
    }
}
