using BlogTemplate.Core.Models;
using BlogTemplate.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogTemplate.Services
{
    public class UserService
    {
        private User currentUser;
        private BlogContext db;
        private IHttpContextAccessor httpContext;

        public UserService(BlogContext db, IHttpContextAccessor httpContext)
        {
            this.db = db;
            this.httpContext = httpContext;
        }

        public async Task<User> GetCurrentUserAsync()
        {
            if (currentUser == null)
            {
                var username = httpContext.HttpContext.User.Identity.Name;
                currentUser = await db.Users.FirstOrDefaultAsync(u => u.Login == username);
            }
            return currentUser;
        }

        public User GetCurrentUser()
        {
            if (currentUser == null)
            {
                var username = httpContext.HttpContext.User.Identity.Name;
                currentUser = db.Users.FirstOrDefault(u => u.Login == username);
            }
            return currentUser;
        }

        public async Task<ShowUserVM> GetUserByIdAsync(int id)
        {
            return new ShowUserVM(await db.Users.FirstOrDefaultAsync(u => u.Id == id));
        }

        public ShowUserVM GetUserById(int id)
        {
            return new ShowUserVM(db.Users.FirstOrDefault(u => u.Id == id));
        }
    }
}
