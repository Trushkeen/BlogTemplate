using BlogTemplate.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogTemplate.Models
{
    public class LikedUser
    {
        public int Id { get; set; }
        public string Login { get; set; }

        public LikedUser(User user)
        {
            this.Id = user.Id;
            this.Login = user.Login;
        }

        public LikedUser() { }
    }
}
