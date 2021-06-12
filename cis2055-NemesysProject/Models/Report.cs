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
            Investigations = new HashSet<Investigation>();
            ReportHazards = new HashSet<ReportHazard>();
        }

        [Key]
        public int ReportId { get; set; }
        public DateTime DateOfReport { get; set; }
        public DateTime DateTimeHazard { get; set; }
        public string Description { get; set; }
        public int Upvotes { get; set; }
        public string Image { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int StatusId { get; set; }
        public virtual StatusCategory Status { get; set; }
        public string UserId { get; set; }
        public virtual NemesysUser User { get; set; }
        public virtual ICollection<Investigation> Investigations { get; set; }
        public virtual ICollection<ReportHazard> ReportHazards { get; set; }
    }
}
