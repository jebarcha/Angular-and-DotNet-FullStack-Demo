using System.Collections.Generic;

#nullable disable

namespace EShopping.Models
{
    public class Profile
    {
        public Profile()
        {
            Users = new HashSet<User>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
