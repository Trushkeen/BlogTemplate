using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogTemplate.Core.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public int Rating { get; set; }
        public bool IsAdmin { get; set; }
        public List<Post> Posts { get; set; }
    }
}
