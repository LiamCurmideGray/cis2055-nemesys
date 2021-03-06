using cis2055_NemesysProject.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace cis2055_NemesysProject.ViewModel
{
    public class CreateReportViewModel
    {
        public int ReportId { get; set; }
        public string UserId { get; set; }
        [Display(Name = "Hazard Type")]
        [Required(ErrorMessage = "Hazard type must be selected")]
        public int? HazardId { get; set; }

        [Required(ErrorMessage = "Please select date and time hazard was spotted")]
        [Range(typeof(DateTime), "01/01/1900", "06/06/2079", ErrorMessage = "Date must be between 01/01/1900 and 06/06/2079")]
        [RestrictedDate(ErrorMessage = "Hazard spotted date and time cannot be in the future!")]
        public DateTime DateTimeHazard { get; set; } = System.DateTime.Today;

        public string Image { get; set; }
        [Required(ErrorMessage = "Report Title cannot be empty.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Report description cannot be empty.")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        [DataType(DataType.Upload)]
        [Display(Name = "Hazard Image")]
        public IFormFile ImageToUpload { get; set; }
        [Required (ErrorMessage = "Please place a marker on the map")]
        [Range (-90, 90, ErrorMessage = "Latitude has to be between -90 and 90")]
        public double? Latitude { get; set; }
        [Range(-180, 180, ErrorMessage = "Longitude has to be between -180 and 180")]
        public double? Longitude { get; set; }

        public IEnumerable<Hazard> HazardList { get; set; }
    }

    public class RestrictedDate : ValidationAttribute
    {
        public override bool IsValid(object date)
        {
            DateTime newDate = (DateTime)date;
            return newDate < DateTime.Now;
        }
    }
}
