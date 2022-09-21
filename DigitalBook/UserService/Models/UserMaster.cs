using System;
using System.Collections.Generic;

namespace UserService.Models
{
    public partial class UserMaster
    {
        public UserMaster()
        {
            Books = new HashSet<Book>();
        }

        public int UserId { get; set; }
        public string UserName { get; set; } = null!;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string EmailId { get; set; } = null!;
        public string? UserPassword { get; set; }
        public int? RoleId { get; set; }
        public bool? Active { get; set; }

        public virtual RoleMaster? Role { get; set; }
        public virtual ICollection<Book> Books { get; set; }
    }
}
