using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BlogTemplate.Core.Models
{
    public class Post
    {
        public int Id { get; set; }
        public User Author { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int Likes { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
