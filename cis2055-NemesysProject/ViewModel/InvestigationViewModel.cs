using cis2055_NemesysProject.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace cis2055_NemesysProject.ViewModel
{
    public class InvestigationViewModel
    {
        [Display (Name = "Investigation ID")]
        public int InvestigationId { get; set; }
        public int ReportId { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        [Display (Name = "Investigator")]
        public NemesysUser User { get; set; }
        [Display (Name = "Report")]
        public Report Report { get; set; }
        public Hazard Hazard { get; set; }
    }
}
