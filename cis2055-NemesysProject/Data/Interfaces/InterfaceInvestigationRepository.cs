using cis2055_NemesysProject.Models;
using cis2055_NemesysProject.ViewModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cis2055_NemesysProject.Data.Interfaces
{
    public interface InterfaceInvestigationRepository
    {
      
        IEnumerable<Investigation> GetAllInvestigations();
        Investigation GetInvestigationById(int id);
       
        IEnumerable<LogInvestigation> GetLogsOfInvestigation(int id);
        Investigation GetInvestigationByReportId(int id);

        public void AddInvestigation(CreateInvestigationViewModel investigationModel);
    }
}
