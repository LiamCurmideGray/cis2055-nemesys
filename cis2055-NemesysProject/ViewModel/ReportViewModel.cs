using cis2055_NemesysProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cis2055_NemesysProject.ViewModel
{
    public class ReportViewModel
    {
        public int ReportId { get; set; }
        public int UserId { get; set; }
        public int StatusId { get; set; }
        public DateTime DateOfReport { get; set; }
        public DateTime DateTimeHazard { get; set; }
        public string Description { get; set; }
        public int Upvotes { get; set; }
        public HazardViewModel Hazard { get; set; }
        public StatusCategory Status { get; set; }
    }
}
