using BlogTemplate.Core.Models;
using BlogTemplate.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogTemplate.Controllers
{
    public class PostController : Controller
    {
        private BlogContext db;

        public PostController(BlogContext dbUsers)
        {
            db = dbUsers;
        }

        [HttpGet]
        public async Task<IActionResult> LatestPost()
        {
            ShowPostVM model = new ShowPostVM();
            var latestPost = await db.Posts.FirstOrDefaultAsync(p => p.CreatedDate == db.Posts.Max(d => d.CreatedDate));
            if (latestPost != null)
            {
                model.Post = latestPost;
                model.Author = latestPost.Author;
                return View("Post", model);
            }
            else return new NotFoundResult();
        }

        [HttpGet]
        public IActionResult LatestPosts(int count = 10)
        {
            var latestPosts = db.Posts.OrderByDescending(d=>d.CreatedDate).Take(count);
            if (latestPosts != null)
            {
                return View(latestPosts);
            }
            else return null;
        }

        [HttpGet]
        public IActionResult CreatePost()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreatePost(CreatePostVM model)
        {
            var post = new Post()
            {
                Title = model.Title,
                Content = model.Content,
                Author = await db.Users.FirstOrDefaultAsync(u => u.Login == User.Identity.Name),
                CreatedDate = DateTime.Now,
                Likes = 0
            };
            await db.Posts.AddAsync(post);
            await db.SaveChangesAsync();

            return RedirectToAction("LatestPost", "Post");
        }
    }
}
