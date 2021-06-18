using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace cis2055_NemesysProject.Models
{
    public partial class Report
    {
        public Report()
        {
        }

        [Key]
        [Display(Name = "Report ID")]
        public int ReportId { get; set; }
        [Display(Name = "Date of Report")]
        public DateTime DateOfReport { get; set; }
        [Display(Name = "Hazard Spotted")]
        public DateTime DateTimeHazard { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
        public int Upvotes { get; set; }
        public string Image { get; set; }
        public string Title { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int StatusId { get; set; }
        public int HazardId { get; set; }
        public Hazard Hazard { get; set; }
        public virtual StatusCategory Status { get; set; }
        public string UserId { get; set; }
        [Display(Name = "Reporter")]
        public virtual NemesysUser User { get; set; }
        public virtual Investigation Investigation { get; set; }
    }
}
