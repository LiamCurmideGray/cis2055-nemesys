using cis2055_NemesysProject.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace cis2055_NemesysProject.ViewModel
{
    public class ReportDetailsViewModel
    {
        public int ReportId { get; set; }
        public string UserId { get; set; }
        public int StatusId { get; set; }
        [Display(Name = "Date of Report")]
        public DateTime DateOfReport { get; set; }
        [Display(Name = "Hazard Spotted")]
        public DateTime DateTimeHazard { get; set; }
        public string Description { get; set; }
        public int Upvotes { get; set; }
        public string Image { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public HazardViewModel Hazard { get; set; }
        public StatusCategory Status { get; set; }
        public NemesysUser User { get; set; }
        //public Investigation Investigation { get; set; }
    }
}
