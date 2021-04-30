using System;
using System.Collections.Generic;

#nullable disable

namespace cis2055_NemesysProject.Models
{
    public partial class User
    {
        public User()
        {
            Investigations = new HashSet<Investigation>();
            Reports = new HashSet<Report>();
        }

        public int UserId { get; set; }
        public int RoleId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public int? Telephone { get; set; }
        public string Password { get; set; }

        public virtual Role Role { get; set; }
        public virtual ICollection<Investigation> Investigations { get; set; }
        public virtual ICollection<Report> Reports { get; set; }
    }
}
