using cis2055_NemesysProject.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace cis2055_NemesysProject.ViewModel
{
    public class CreateInvestigationViewModel
    {
        public int InvestigationId { get; set; }
        public int ReportId { get; set; }
        [Display (Name = "Report Title")]
        public string ReportTitle { get; set; }
        [Required(ErrorMessage = "Investigation description cannot be empty.")]
        public string Description { get; set; }
        public string UserId { get; set; }
        [Display(Name = "Status")]
        [Required(ErrorMessage = "Status must be selected")]
        public int StatusId { get; set; }

        public IEnumerable<StatusCategory> StatusList { get; set; }
        public NemesysUser User { get; set; }
        [Required(ErrorMessage = "Log description is required")]
        public string LogDescription { get; set; }
        public IEnumerable<LogInvestigation> LogInvestigation { get; set; }
    }
}
