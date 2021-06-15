using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cis2055_NemesysProject.Data.Interfaces;
using cis2055_NemesysProject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace cis2055_NemesysProject.Data.Repositories
{
    public class NemesysRepository : INemesysRepository
    {
        private readonly cis2055nemesysContext _context;
        private readonly ILogger _logger;

        public NemesysRepository(cis2055nemesysContext context, ILogger<NemesysRepository> logger)
        {
            try
            {
                _context = context;
                _logger = logger;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public IEnumerable<Report> GetAllReports()
        {
            try{
                return _context.Reports.Include(r => r.Status).Include(r => r.Hazard).Include(r => r.User).OrderBy(r => r.DateOfReport);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public IEnumerable<Investigation> GetAllInvestigations()
        {
            try
            {
                return _context.Investigations.Include(r => r.Report).Include(r => r.User).OrderBy(r => r.InvestigationId);
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
                return _context.Reports.Include(r => r.Status).Include(r => r.User).Include(r => r.Hazard).FirstOrDefault(p => p.ReportId == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
        
        public IEnumerable<Report> GetReportByUserId(string id)
        {
            try
            {
                return _context.Reports.Include(r => r.Status).Include(r => r.User).Include(r => r.Hazard).Where(p => p.UserId == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
        
        public NemesysUser GetUserByReportId(int id)
        {
            var report = GetReportById(id);
            var userId = report.UserId;
            return _context.Users.FirstOrDefault(u => u.Id == userId);
        }

        public Investigation GetInvestigationById(int id)
        {
            try
            {
                return _context.Investigations.Include(r => r.Report).Include(r => r.User).Include(r => r.LogInvestigations).FirstOrDefault(p => p.InvestigationId == id);
             }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
        

        public NemesysUser GetUserById(string id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == id);
        }

        public IEnumerable<int> GetUpvotesReportByUserId(string id)
        {
            try
            {
                return _context.Reports.Include(r => r.Status).Include(r => r.User).Include(r => r.Hazard).Where(p => p.UserId == id).Select(r => r.Upvotes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public Report UpdateReportUpVote (int reportId)
        {
            try
            {
            Report report = GetReportById(reportId);
            report.Upvotes++;

            _context.Update(report);
            _context.SaveChanges();

            } catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }

            return null;
        }

    }
}
