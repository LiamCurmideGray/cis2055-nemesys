using System;
using System.Collections.Generic;

#nullable disable

namespace cis2055_NemesysProject.Models
{
    public partial class Hazard
    {
        public Hazard()
        {
            ReportHazards = new HashSet<ReportHazard>();
        }

        public int HarzardId { get; set; }
        public string HazardType { get; set; }

        public virtual ICollection<ReportHazard> ReportHazards { get; set; }
    }
}
