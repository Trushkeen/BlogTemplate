using BlogTemplate.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogTemplate.ViewModels
{
    public class ShowUserVM
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public int Rating { get; set; }
        public bool IsAdmin { get; set; }

        public ShowUserVM(User model)
        {
            Id = model.Id;
            Login = model.Login;
            Rating = model.Rating;
            IsAdmin = model.IsAdmin;
        }
    }
}
