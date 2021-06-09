using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using cis2055_NemesysProject.Data;
using cis2055_NemesysProject.Models;
using cis2055_NemesysProject.ViewModel;
using System.IO;

namespace cis2055_NemesysProject.Controllers
{
    public class ReportsController : Controller
    {
        private readonly cis2055nemesysContext _context;

        public ReportsController(cis2055nemesysContext context)
        {
            _context = context;
        }

        // GET: Reports
        public async Task<IActionResult> Index()
        {
            var cis2055nemesysContext = _context.Reports.Include(r => r.User);
            return View(await cis2055nemesysContext.ToListAsync());
        }

        // GET: Reports/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var report = await _context.Reports
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.ReportId == id);
            if (report == null)
            {
                return NotFound();
            }

            return View(report);
        }

        // GET: Reports/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email");
            ViewData["HazardId"] = new SelectList(_context.Hazards, "HazardId", "HazardType");
            var hazards = _context.Hazards.ToList();

            var model = new CreateReportViewModel()
            {
                HazardList = hazards
            };
            return View(model);
        }

        // POST: Reports/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,HazardId,DateTimeHazard,Description,ImageToUpload,Latitude,Longitude")] CreateReportViewModel report)
        {
            if (ModelState.IsValid)
            {
                string fileName = "";
                if (report.ImageToUpload != null)
                {
                    
                    var extension = "." + report.ImageToUpload.FileName.Split('.')[report.ImageToUpload.FileName.Split('.').Length - 1];
                    fileName = Guid.NewGuid().ToString() + extension;
                    var path = Directory.GetCurrentDirectory() + "\\wwwroot\\images\\reports\\" + fileName;
                    using (var bits = new FileStream(path, FileMode.Create))
                    {
                        report.ImageToUpload.CopyTo(bits);
                    }
                }

                Report newReport = new Report()
                {
                    UserId = report.UserId,
                    DateOfReport = DateTime.UtcNow,
                    DateTimeHazard = report.DateTimeHazard,
                    Description = report.Description,
                    Upvotes = 0,
                    Image = "/images/reports/" + fileName,
                    Longitude = report.Longitude,
                    Latitude = report.Latitude,
                    StatusId = 1
                };
                _context.Add(newReport);
                await _context.SaveChangesAsync();

                // can be modified to support many to many like in database
                Report reportObj = _context.Reports.OrderBy(r => r.ReportId).Last();
                ReportHazard reportHazard = new ReportHazard()
                {
                    HazardId = (int)report.HazardId,
                    ReportId = reportObj.ReportId
                };
                _context.Add(reportHazard);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email");
                var hazards = _context.Hazards.ToList();

                var model = new CreateReportViewModel()
                {
                    HazardList = hazards
                };
                return View(model);
            }
        }

        // GET: Reports/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var report = await _context.Reports.FindAsync(id);
            if (report == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email", report.UserId);
            return View(report);
        }

        // POST: Reports/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReportId,UserId,DateOfReport,DateTimeHazard,Description,Upvotes,Image,Latitude,Longitude")] Report report)
        {
            if (id != report.ReportId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(report);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReportExists(report.ReportId))
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
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email", report.UserId);
            return View(report);
        }

        // GET: Reports/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var report = await _context.Reports
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.ReportId == id);
            if (report == null)
            {
                return NotFound();
            }

            return View(report);
        }

        // POST: Reports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            List<ReportHazard> reportHazards = _context.ReportHazards.Where(e => e.ReportId == id).ToList();

            foreach (var item in reportHazards)
            {
                _context.Remove(item);
            }

            var report = await _context.Reports.FindAsync(id);
            _context.Reports.Remove(report);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReportExists(int id)
        {
            return _context.Reports.Any(e => e.ReportId == id);
        }
    }
}
