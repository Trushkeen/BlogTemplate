using BlogTemplate.Core.Models;
using BlogTemplate.Services;
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
        private UserService currentUser;

        public PostController(BlogContext dbUsers, UserService currentUser)
        {
            db = dbUsers;
            this.currentUser = currentUser;
        }

        [HttpGet]
        public async Task<IActionResult> ShowPost(int id)
        {
            var post = await db.Posts.FirstOrDefaultAsync(p => p.Id == id);
            if (post != null)
            {
                return View("Post", post);
            }
            else return new NotFoundResult();
        }

        [HttpGet]
        public IActionResult LatestPosts(int count = 10)
        {
            var latestPosts = db.Posts.OrderByDescending(d => d.CreatedDate).Take(count);
            if (latestPosts != null)
            {
                return View(latestPosts);
            }
            else return null;
        }

        [HttpGet]
        [Authorize]
        public IActionResult CreatePost()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreatePost(CreatePostVM model)
        {
            var author = await currentUser.GetCurrentUserAsync();
            var post = new Post()
            {
                Title = model.Title,
                Content = model.Content,
                AuthorId = author.Id,
                CreatedDate = DateTime.Now,
                Likes = 0
            };
            await db.Posts.AddAsync(post);
            await db.SaveChangesAsync();

            return RedirectToAction("LatestPosts", "Post");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeletePost(int id)
        {
            var user = await currentUser.GetCurrentUserAsync();
            if (user.IsAdmin)
            {
                var obj = await db.Posts.FirstOrDefaultAsync(p => p.Id == id);
                db.Posts.Remove(obj);
                await db.SaveChangesAsync();
            }
            return RedirectToAction("LatestPosts", "Post");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> LikePost(int id)
        {
            var user = await currentUser.GetCurrentUserAsync();
            var obj = await db.Posts.FirstOrDefaultAsync(p => p.Id == id);
            if (obj.LikedUsers == null)
            {
                obj.LikedUsers = new List<User>();
            }
            obj.Likes++;
            obj.LikedUsers.Add(user);
            try
            {
                await db.SaveChangesAsync();
            }
            catch (Exception) { }
            return RedirectToAction("LatestPosts", "Post");
        }
    }
}
