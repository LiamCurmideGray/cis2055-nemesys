using System;
using System.Collections.Generic;

#nullable disable

namespace cis2055_NemesysProject.Models
{
    public partial class Role
    {
        public Role()
        {
            Users = new HashSet<User>();
        }

        public int RoleId { get; set; }
        public string RoleType { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
