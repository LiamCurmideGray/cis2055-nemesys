using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using cis2055_NemesysProject.Data.Interfaces;
using cis2055_NemesysProject.Models;
using cis2055_NemesysProject.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace cis2055_NemesysProject.Data.Repositories
{
    public class InvestigationRepository : InterfaceInvestigationRepository
    {
        private readonly cis2055nemesysContext _context;
        private readonly IReportRepository _reportRepository;
        private readonly ILogger _logger;

        public InvestigationRepository(cis2055nemesysContext context, ILogger<InvestigationRepository> logger, IReportRepository reportRepository)
        {
            try
            {
                _context = context;
                _logger = logger;
                _reportRepository = reportRepository;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

     

        public IEnumerable<Investigation> GetAllInvestigations()
        {
            try
            {
                return _context.Investigations.Include(r => r.Report).Include(r => r.User).Include(r => r.Report.User).OrderBy(r => r.InvestigationId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

       


        public Investigation GetInvestigationById(int id)
        {
            try
            {
                Investigation investigation = _context.Investigations.Include(r => r.Report).Include(r => r.User).Include(r => r.LogInvestigations).FirstOrDefault(p => p.InvestigationId == id);
                investigation.Report = _reportRepository.GetReportById(investigation.ReportId);
                return investigation;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }


        public Investigation GetInvestigationByReportId(int id)
        {
            try
            {
               return _context.Investigations.Include(r => r.Report).Include(r => r.User).Include(r => r.LogInvestigations).FirstOrDefault(p => p.ReportId == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public void AddInvestigation(CreateInvestigationViewModel investigationModel)
        {
            try
            {
                Investigation investigation = new Investigation()
                {
                    ReportId = investigationModel.ReportId,
                    Description = investigationModel.Description,
                    UserId = investigationModel.UserId
                };

                _context.Add(investigation);
                _context.SaveChanges();

                Investigation investigationId = GetInvestigationByReportId(investigationModel.ReportId);

                LogInvestigation logInvestigation = new LogInvestigation()
                {
                    InvestigationId = investigationId.InvestigationId,
                    Description = investigationModel.LogDescription,
                    DateOfAction = DateTime.UtcNow
                };

                _context.Add(logInvestigation);
                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }

        }



        public IEnumerable<LogInvestigation> GetLogsOfInvestigation(int id)
        {
            try
            {
                return _context.LogInvestigations.Include(r => r.Investigation).Where(p => p.InvestigationId == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
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

      
    }
}
