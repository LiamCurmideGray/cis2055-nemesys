using System;
using System.Collections.Generic;

#nullable disable

namespace cis2055_NemesysProject.Models
{
    public partial class Hazard
    {
        public Hazard()
        {
            Reports = new HashSet<Report>();
            ReportHazards = new HashSet<ReportHazard>();
        }

        public int HazardId { get; set; }
        public string HazardType { get; set; }
        public virtual ICollection<Report> Reports { get; set; }
        public virtual ICollection<ReportHazard> ReportHazards { get; set; }
    }
}
