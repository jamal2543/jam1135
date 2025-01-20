using System;
using System.Collections.Generic;

namespace kurs11135.Models
{
    public partial class UserPosition
    {
        public UserPosition()
        {
            Users = new HashSet<User>();
        }

        public int Id { get; set; }
        public string? Titile { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
