using IrasBlog.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IrasBlog.Data
{
    public class BlogContext : IdentityDbContext<ApplicationUser>
    {
        private readonly IConfiguration _config;

        public BlogContext(IConfiguration config)
        {
            _config = config;
        }

        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(_config["ConnectionStrings:BlogContextDb"]);
        }
    }
}
