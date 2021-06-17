using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cis2055_NemesysProject.Models;
using cis2055_NemesysProject.ViewModel;
using Microsoft.AspNetCore.Http;

namespace cis2055_NemesysProject.Data.Interfaces
{
    public interface IReportRepository
    {
        public IEnumerable<Report> GetAllReports();
        public Report GetReportById(int id);
        IEnumerable<Report> GetReportByUserId(string id);
        public void AddReport(CreateReportViewModel report);
        public bool UpdateReport(CreateReportViewModel report, string currentUserId);
        public bool DeleteReport(int reportId, string userId);
        Report UpdateReportUpVote(int reportId);
        string UploadImage(IFormFile image);
        public IEnumerable<Hazard> GetAllHazard();
        public IEnumerable<StatusCategory> GetAllStatusCategories();

    }
}
