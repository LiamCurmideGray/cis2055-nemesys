using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace cis2055_NemesysProject.Models
{
    public partial class Hazard
    {
        public Hazard()
        {
            ReportHazards = new HashSet<ReportHazard>();
        }

        [Key]
        public int HazardId { get; set; }
        public string HazardType { get; set; }

        public virtual ICollection<ReportHazard> ReportHazards { get; set; }
    }
}
