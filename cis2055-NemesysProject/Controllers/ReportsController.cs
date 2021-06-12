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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace cis2055_NemesysProject.Controllers
{
    public class ReportsController : Controller
    {
        private readonly cis2055nemesysContext _context;

        private readonly UserManager<NemesysUser> _userManager;

        public ReportsController(cis2055nemesysContext context, UserManager<NemesysUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Reports
        //[Authorize(Roles = "Admin")]
        //[Authorize(Roles = "Reporter")]
        [Authorize]
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
        [Authorize(Roles = "Reporter")]
        public IActionResult Create()
        {
            //ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email");
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
        [Authorize(Roles = "Reporter")]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("UserId,HazardId,DateTimeHazard,Description,ImageToUpload,Latitude,Longitude")] CreateReportViewModel report)
        public async Task<IActionResult> Create([Bind("HazardId,DateTimeHazard,Description,ImageToUpload,Latitude,Longitude")] CreateReportViewModel report)
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
                    UserId = _userManager.GetUserId(User),
                    DateOfReport = DateTime.UtcNow,
                    DateTimeHazard = report.DateTimeHazard,
                    Description = report.Description,
                    Upvotes = 0,
                    Image = imageUrl,
                    Longitude = (double)report.Longitude,
                    Latitude = (double)report.Latitude,
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
        [Authorize(Roles = "Reporter")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var report = await _context.Reports.FindAsync(id);
            var hazards = _context.Hazards.ToList();
            var hazardId = _context.ReportHazards.Where(r => r.ReportId == id).Select(i => i.HazardId).FirstOrDefault();
            if (report == null)
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (report.UserId == currentUser.Id)
            {
                CreateReportViewModel model = new CreateReportViewModel()
                {
                    ReportId = report.ReportId,
                    UserId = _userManager.GetUserId(User),
                    HazardId = hazardId,
                    DateTimeHazard = report.DateTimeHazard,
                    Description = report.Description,
                    Image = report.Image,
                    Longitude = report.Longitude,
                    Latitude = report.Latitude,
                    HazardList = hazards,
                };
                return View(model);
            }
            else
            {
                return Unauthorized();
            }

        }

        // POST: Reports/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Reporter")]
        public async Task<IActionResult> Edit(int id, [Bind("HazardId,DateTimeHazard,Description,ImageToUpload,Latitude,Longitude")] CreateReportViewModel report)
        {
            var modelToUpdate = await _context.Reports.FindAsync(id);
            var currentUser = await _userManager.GetUserAsync(User);
            if (modelToUpdate.User.Id == currentUser.Id)
            {

                if (ModelState.IsValid)
                {
                    try
                    {
                        var reportHazardId = _context.ReportHazards.Where(r => r.ReportId == id).Select(i => i.HazardId).FirstOrDefault();
                        var reportHazard = _context.ReportHazards.Find(reportHazardId, id);
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
                        _context.Remove(reportHazard);
                        await _context.SaveChangesAsync();

                        reportHazard.ReportId = id;
                        reportHazard.HazardId = (int)report.HazardId;

                        _context.Add(reportHazard);
                        await _context.SaveChangesAsync();
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
                    //ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email");
                    var hazards = _context.Hazards.ToList();

                    var model = new CreateReportViewModel()
                    {
                        HazardList = hazards
                    };
                    return View(model);
                }
                //ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email", report.UserId);
                //return View(report);
            }
            else
            {
                return Unauthorized();
            }
        }

        // GET: Reports/Delete/5
        [Authorize(Roles = "Reporter")]
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

            var currentUser = await _userManager.GetUserAsync(User);
            if (report.User.Id == currentUser.Id)
            {
            return View(report);

            } else
            {
                return Unauthorized();
            }

        }

        // POST: Reports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Reporter")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            List<ReportHazard> reportHazards = _context.ReportHazards.Where(e => e.ReportId == id).ToList();

            foreach (var item in reportHazards)
            {
                _context.Remove(item);
            }

            var report = await _context.Reports.FindAsync(id);

            var currentUser = await _userManager.GetUserAsync(User);
            if (report.User.Id == currentUser.Id)
            {
            _context.Reports.Remove(report);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

            } else
            {
                return Unauthorized();
            }
        }

        private bool ReportExists(int id)
        {
            return _context.Reports.Any(e => e.ReportId == id);
        }
    }
}
