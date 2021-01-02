using BlogTemplate.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogTemplate.ViewModels
{
    public class ShowPostVM
    {
        public Post Post { get; set; }
        public User Author { get; set; }
    }
}
