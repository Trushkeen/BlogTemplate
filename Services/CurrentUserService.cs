using BlogTemplate.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogTemplate.Services
{
    public class CurrentUserService
    {
        private User currentUser;
        private BlogContext db;
        private IHttpContextAccessor httpContext;

        public CurrentUserService(BlogContext db, IHttpContextAccessor httpContext)
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
    }
}
