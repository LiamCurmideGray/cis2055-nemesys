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
using cis2055_NemesysProject.Data.Interfaces;
using System.IO;
using Microsoft.Extensions.Logging;

namespace cis2055_NemesysProject.Controllers
{
    public class ReportsController : Controller
    {
        private readonly cis2055nemesysContext _context;
        private readonly INemesysRepository _nemesysRepository;
        private readonly ILogger<ReportsController> _logger;

        public ReportsController(cis2055nemesysContext context, INemesysRepository nemesysRepository, ILogger<ReportsController> logger)
        {
            _context = context;
            _nemesysRepository = nemesysRepository;
            _logger = logger;
        }

        // GET: Reports
        public IActionResult Index()
        {
            var model = new ReportListViewModel()
            {
                TotalReports = _nemesysRepository.GetAllReports().Count(),
                Reports = _nemesysRepository.GetAllReports().OrderByDescending(d => d.DateOfReport).Select(r => new ReportViewModel
                {
                    ReportId = r.ReportId,
                    UserId = r.UserId,
                    StatusId = r.StatusId,
                    DateOfReport = r.DateOfReport,
                    DateTimeHazard = r.DateTimeHazard,
                    Description = r.Description,
                    Upvotes = r.Upvotes,
                    Image = r.Image,
                    Hazard = new HazardViewModel()
                    {
                        HazardId = r.Hazard.HazardId,
                        HazardType = r.Hazard.HazardType
                    },
                    Status = new StatusCategory()
                    {
                        StatusId = r.Status.StatusId,
                        StatusType = r.Status.StatusType
                    }
                })
            };
            //var cis2055nemesysContext = _context.Reports.Include(r => r.User);
            return View(model);
        }

        // GET: Reports/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var report = _nemesysRepository.GetReportById(id);
            if (report == null)
            {
                return NotFound();
            }

            var model = new ReportDetailsViewModel()
            {
                ReportId = report.ReportId,
                UserId = report.UserId,
                StatusId = report.StatusId,
                DateOfReport = report.DateOfReport,
                DateTimeHazard = report.DateTimeHazard,
                Description = report.Description,
                Upvotes = report.Upvotes,
                Image = report.Image,
                Latitude = report.Latitude,
                Longitude = report.Longitude,
                Hazard = new HazardViewModel()
                {
                    HazardId = report.Hazard.HazardId,
                    HazardType = report.Hazard.HazardType,
                },
                Status = new StatusCategory()
                {
                    StatusId = report.Status.StatusId,
                    StatusType = report.Status.StatusType
                },
                User = new User()
                {
                    UserId = report.UserId,
                }
            };


            return View(model);
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
                string imageUrl = "";
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
                    imageUrl = "/images/reports/" + fileName;
                }

                Report newReport = new Report()
                {
                    UserId = report.UserId,
                    DateOfReport = DateTime.UtcNow,
                    DateTimeHazard = report.DateTimeHazard,
                    Description = report.Description,
                    Upvotes = 0,
                    Image = imageUrl,
                    Longitude = (double)report.Longitude,
                    Latitude = (double)report.Latitude,
                    StatusId = 1,
                    HazardId = (int)report.HazardId
                };
                _context.Add(newReport);
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
            var hazards = _context.Hazards.ToList();
            if (report == null)
            {
                return NotFound();
            }
            CreateReportViewModel model = new CreateReportViewModel()
            {
                ReportId = report.ReportId,
                UserId = report.UserId,
                HazardId = report.HazardId,
                DateTimeHazard = report.DateTimeHazard,
                Description = report.Description,
                Image = report.Image,
                Longitude = report.Longitude,
                Latitude = report.Latitude,
                HazardList = hazards,
            };
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email", report.UserId);
            return View(model);
        }

        // POST: Reports/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,HazardId,DateTimeHazard,Description,ImageToUpload,Latitude,Longitude")] CreateReportViewModel report)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var modelToUpdate = await _context.Reports.FindAsync(id);
                    if (modelToUpdate == null)
                    {
                        return NotFound();
                    }
                    string fileName = "";
                    string imageUrl = "";
                    if (report.ImageToUpload != null)
                    {

                        var extension = "." + report.ImageToUpload.FileName.Split('.')[report.ImageToUpload.FileName.Split('.').Length - 1];
                        fileName = Guid.NewGuid().ToString() + extension;
                        var path = Directory.GetCurrentDirectory() + "\\wwwroot\\images\\reports\\" + fileName;
                        using (var bits = new FileStream(path, FileMode.Create))
                        {
                            report.ImageToUpload.CopyTo(bits);
                        }
                        imageUrl = "/images/reports/" + fileName;
                    }
                    else
                    {
                        imageUrl = modelToUpdate.Image;
                    }
                    modelToUpdate.DateTimeHazard = report.DateTimeHazard;
                    modelToUpdate.Description = report.Description;
                    modelToUpdate.Image = imageUrl;
                    modelToUpdate.Latitude = (double)report.Latitude;
                    modelToUpdate.Longitude = (double)report.Longitude;
                    modelToUpdate.HazardId = (int)report.HazardId;

                    _context.Update(modelToUpdate);
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
            if (report.Image != "")
                System.IO.File.Delete("wwwroot" + report.Image);
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
