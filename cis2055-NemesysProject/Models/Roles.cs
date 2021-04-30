using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace cis2055_NemesysProject.Models
{
    public class Roles
    {
        [Key]
        public int Role_ID { get; set; }
        public string RoleType { get; set; }
    }
}
