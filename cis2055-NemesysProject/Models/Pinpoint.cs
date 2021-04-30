using System;
using System.Collections.Generic;

#nullable disable

namespace cis2055_NemesysProject.Models
{
    public partial class Pinpoint
    {
        public Pinpoint()
        {
            Reports = new HashSet<Report>();
        }

        public int PinpointId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public virtual ICollection<Report> Reports { get; set; }
    }
}
