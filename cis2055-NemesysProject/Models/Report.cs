using System;
using System.Collections.Generic;

#nullable disable

namespace cis2055_NemesysProject.Models
{
    public partial class Report
    {
        public Report()
        {
            //Hazards = new HashSet<Hazard>();
            Investigations = new HashSet<Investigation>();
            //ReportHazards = new HashSet<ReportHazard>();
        }

        public int ReportId { get; set; }
        public int UserId { get; set; }
        public DateTime DateOfReport { get; set; }
        public DateTime DateTimeHazard { get; set; }
        public string Description { get; set; }
        public int Upvotes { get; set; }
        public string Image { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int StatusId { get; set; }
        public int HazardId { get; set; }
        public Hazard Hazard { get; set; }
        public Investigation Investigation { get; set; }
        public virtual StatusCategory Status { get; set; }

        //public virtual Pinpoint Pinpoint { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Investigation> Investigations { get; set; }
        //public virtual ICollection<Hazard> Hazards { get; set; }
        public virtual ICollection<ReportHazard> ReportHazards { get; set; }
    }
}
