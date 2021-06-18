using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using cis2055_NemesysProject.Models;
using cis2055_NemesysProject.Data.Interfaces;
using cis2055_NemesysProject.ViewModel;

namespace cis2055_NemesysProject.Data.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly cis2055nemesysContext _context;
        private readonly ILogger _logger;

        public ReportRepository(cis2055nemesysContext context, ILogger<InvestigationRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IEnumerable<Report> GetAllReports()
        {
            try
            {
                return _context.Reports.Include(r => r.Status).Include(r => r.Hazard).Include(r => r.User).Include(r => r.Investigation).OrderBy(r => r.DateOfReport);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public Report GetReportById(int id)
        {
            try
            {
                return _context.Reports.Include(r => r.Status).Include(r => r.User).Include(r => r.Hazard).Include(r => r.Investigation).Include(r => r.Investigation.LogInvestigations).FirstOrDefault(p => p.ReportId == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public void AddReport(CreateReportViewModel report)
        {
            try
            {
                Report newReport = new Report()
                {
                    UserId = report.UserId,
                    DateOfReport = DateTime.UtcNow,
                    DateTimeHazard = report.DateTimeHazard,
                    Title = report.Title,
                    Description = report.Description,
                    Upvotes = 0,
                    Image = UploadImage(report.ImageToUpload),
                    Longitude = (double)report.Longitude,
                    Latitude = (double)report.Latitude,
                    StatusId = 1,
                    HazardId = (int)report.HazardId,
                };
                _context.Add(newReport);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }

        }

        public bool UpdateReport(CreateReportViewModel report, string currentUserId)
        {
            Report updateReport = GetReportById(report.ReportId);
            if (updateReport.User.Id == currentUserId)
            {
                try
                {
                    string imageUrl = "";
                    if (report.ImageToUpload != null)
                    {
                        if(updateReport.Image != null)
                        {
                            System.IO.File.Delete("wwwroot" + updateReport.Image);
                        }
                        imageUrl = UploadImage(report.ImageToUpload);
                    }
                    else
                    {
                        imageUrl = updateReport.Image;
                    }
                    updateReport.DateTimeHazard = report.DateTimeHazard;
                    updateReport.Title = report.Title;
                    updateReport.Description = report.Description;
                    updateReport.Image = imageUrl;
                    updateReport.Latitude = (double)report.Latitude;
                    updateReport.Longitude = (double)report.Longitude;
                    updateReport.HazardId = (int)report.HazardId;
                    _context.Update(updateReport);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    _logger.LogError(ex.Message);
                    throw;

                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DeleteReport(int reportId, string userId)
        {
            Report report = GetReportById(reportId);

            if (report.User.Id == userId)
            {
                if (report.Image != "")
                {
                    System.IO.File.Delete("wwwroot" + report.Image);
                }
                if(report.Investigation != null)
                {
                    if(report.Investigation.LogInvestigations != null)
                    {
                        foreach (var log in report.Investigation.LogInvestigations)
                        {
                            _context.LogInvestigations.Remove(log);
                        }
                    }
                    _context.Investigations.Remove(report.Investigation);
                }
                _context.Reports.Remove(report);
                _context.SaveChanges();
                return true;

            }
            else
            {
                return false;
            }
        }

        public IEnumerable<Report> GetReportByUserId(string id)
        {
            try
            {
                return _context.Reports.Include(r => r.Status).Include(r => r.User).Include(r => r.Hazard).Include(r => r.Investigation).Where(p => p.UserId == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public Report UpdateReportUpVote(int reportId)
        {
            try
            {
                Report report = GetReportById(reportId);
                report.Upvotes++;

                _context.Update(report);
                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }

            return null;
        }

        public string UploadImage(IFormFile image)
        {
            try
            {
                string imageUrl = "";
                string fileName = "";
                if (image != null)
                {

                    var extension = "." + image.FileName.Split('.')[image.FileName.Split('.').Length - 1];
                    fileName = Guid.NewGuid().ToString() + extension;
                    var path = Directory.GetCurrentDirectory() + "\\wwwroot\\images\\reports\\" + fileName;
                    using (var bits = new FileStream(path, FileMode.Create))
                    {
                        image.CopyTo(bits);
                    }
                    imageUrl = "/images/reports/" + fileName;
                }

                return imageUrl;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }

        }

       public IEnumerable<Hazard> GetAllHazard()
        {
            try
            {
                return _context.Hazards;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public IEnumerable<StatusCategory> GetAllStatusCategories()
        {
            try
            {
                return _context.StatusCategories;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
    }
}
