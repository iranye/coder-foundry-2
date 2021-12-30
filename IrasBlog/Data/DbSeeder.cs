using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;
using IrasBlog.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace IrasBlog.Data
{
    public class DbSeeder
    {
        private readonly BlogContext _context;
        private readonly IWebHostEnvironment _hostingEnv;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _config;

        public DbSeeder(BlogContext context, 
            IWebHostEnvironment hostingEnv,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration config)
        {
            _context = context;
            _hostingEnv = hostingEnv;
            _userManager = userManager;
            _roleManager = roleManager;
            _config = config;
        }

        public async Task SeedAsync()
        {
            _context.Database.EnsureCreated();

            if (!_context.BlogPosts.Any())
            {
                var filePath = Path.Combine(_hostingEnv.ContentRootPath, "Data/blogs.json");

                var json = File.ReadAllText(filePath);
                if (String.IsNullOrWhiteSpace(json))
                {
                    throw new InvalidDataException();
                }

                var blogPosts = JsonConvert.DeserializeObject<IEnumerable<BlogPost>>(json);

                if (blogPosts == null || blogPosts.Count() == 0)
                {
                    throw new InvalidDataException("Failed to read BlogPost Info from json file");
                }

                var blogsList = blogPosts.ToList();
                _context.AddRange(blogsList);
                _context.SaveChanges();
            }

            #region Normal User + Role

            var userRoleStr = "User";
            var userRole = new IdentityRole(userRoleStr);

            if (!_context.Roles.Any(r => r.Name == userRoleStr))
            {
                _roleManager.CreateAsync(userRole).GetAwaiter().GetResult();
            }

            var userEmail = _config["DefaultUser:Email"];
            var appUser = await _userManager.FindByEmailAsync(userEmail);
            if (appUser == null)
            {
                var firstName = _config["DefaultUser:FirstName"];
                var lastName = _config["DefaultUser:LastName"];
                var userName = _config["DefaultUser:UserName"];
                var password = _config["DefaultUser:Password"];

                appUser = new ApplicationUser()
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = userEmail,
                    UserName = userName,
                };

                var result = await _userManager.CreateAsync(appUser, password);
                if (result != IdentityResult.Success)
                    throw new InvalidOperationException("Could not create new user in Seeder");

                _userManager.AddToRoleAsync(appUser, userRole.Name).GetAwaiter().GetResult();
                _context.SaveChanges();
            }
            #endregion

            if (_context.BlogPosts.Any() && appUser != null)
            {
                var blogPost = _context.BlogPosts.FirstOrDefault();
                if (blogPost != null)
                {
                    var comment = new Comment
                    {
                        Author = appUser,
                        BlogPost = blogPost,
                        CommentBody = "Great Blog!",
                        Created = new DateTime(2021, 07, 05, 9, 1, 2)
                    };
                    _context.Comments.Add(comment);
                    _context.SaveChanges();
                }
            }

            #region Admin User + Role
            var adminRoleStr = "Admin";
            var adminRole = new IdentityRole(adminRoleStr);

            if (!_context.Roles.Any(r => r.Name == adminRoleStr))
            {
                _roleManager.CreateAsync(adminRole).GetAwaiter().GetResult();
            }

            var adminEmail = _config["AdminUser:Email"];

            var adminUser = await _userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                var firstName = _config["AdminUser:FirstName"];
                var lastName = _config["AdminUser:LastName"];
                var userName = _config["AdminUser:UserName"];
                var password = _config["AdminUser:Password"];

                adminUser = new ApplicationUser()
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = adminEmail,
                    UserName = userName,
                };

                var result = await _userManager.CreateAsync(adminUser, password);
                if (result != IdentityResult.Success)
                    throw new InvalidOperationException("Could not create new admin in Seeder");

                _userManager.AddToRoleAsync(adminUser, adminRole.Name).GetAwaiter().GetResult();
                _context.SaveChanges();
            }
            #endregion

            #region Moderator + Role

            var moderatorRoleStr = "Moderator";
            var moderatorRole = new IdentityRole(moderatorRoleStr);

            if (!_context.Roles.Any(r => r.Name == moderatorRoleStr))
            {
                _roleManager.CreateAsync(moderatorRole).GetAwaiter().GetResult();
            }

            var moderatorEmail = _config["ModeratorUser:Email"];
            var moderatorUser = await _userManager.FindByEmailAsync(moderatorEmail);

            if (moderatorUser == null)
            {
                var firstName = _config["ModeratorUser:FirstName"];
                var lastName = _config["ModeratorUser:LastName"];
                var userName = _config["ModeratorUser:UserName"];
                var password = _config["ModeratorUser:Password"];

                moderatorUser = new ApplicationUser()
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = moderatorEmail,
                    UserName = userName,
                };

                var result = await _userManager.CreateAsync(moderatorUser, password);
                if (result != IdentityResult.Success)
                    throw new InvalidOperationException("Could not create new moderator in Seeder");

                _userManager.AddToRoleAsync(moderatorUser, moderatorRole.Name).GetAwaiter().GetResult();
                _context.SaveChanges();
            }
            _context.SaveChanges();
            #endregion
        }
    }
}
