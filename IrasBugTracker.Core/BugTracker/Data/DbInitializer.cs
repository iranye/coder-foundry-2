using BugTracker.Models;
using Microsoft.AspNetCore.Identity;
using System.IO.Pipelines;
using System.Xml.Linq;

namespace BugTracker.Data
{
    public class SeededUser
    {
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? UserName { get; set; }
        public string? Role { get; set; }
        public bool IsDemoUser { get; set; }
    }

    public static class DbInitializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            ApplicationDbContext context = applicationBuilder.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<ApplicationDbContext>();
            UserManager<ApplicationUser> userManager = applicationBuilder.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            RoleManager<IdentityRole> roleManager = applicationBuilder.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            if (!context.Projects.Any())
            {
                context.AddRange
                (
                    new Project { Name = "Bug Tracker", Description = "The One App to Rule Them All" }
                );
            }

            var seededUsers = new List<SeededUser>()
            {
                new SeededUser{FirstName="Ira", LastName=  "Nye", Email="ira@foo.com", UserName="ira@foo.com", Role="Admin"},
                new SeededUser{FirstName="Sally", LastName="Nye", Email="sub@foo.com", UserName="sub@foo.com", Role="Submitter"},
                new SeededUser{FirstName="Peter", LastName="Nye", Email="pm@foo.com", UserName="pm@foo.com", Role="ProjectManager"},
                new SeededUser{FirstName="Andy", LastName= "Nye", Email="admin@foo.com", UserName="admin@foo.com", Role="Admin"},
                new SeededUser{FirstName="Marton", LastName= "Nye", Email="dev@foo.com", UserName="dev@foo.com", Role="Developer"},

                new SeededUser{FirstName="Dennis", LastName="Love", Email="DemoAdmin@foo.com", UserName="DemoAdmin@foo.com", Role="DemoAdmin", IsDemoUser=true},
                new SeededUser{FirstName="Dedra", LastName= "Love", Email="DemoPM@foo.com", UserName="DemoProjectManager@foo.com", Role="DemoProjectManager", IsDemoUser=true},
                new SeededUser{FirstName="Donna", LastName= "Love", Email="DemoSub@foo.com", UserName="DemoSub@foo.com", Role="DemoSubmitter", IsDemoUser=true},
                new SeededUser{FirstName="Dave", LastName= "Love", Email="DemoDev@foo.com", UserName="DemoDev@foo.com", Role="Developer", IsDemoUser=true},
            };
            SeedRoles(roleManager, seededUsers.Select(u => u.Role).ToList());
            SeedUsers(userManager, seededUsers);
            context.SaveChanges();
            SeedTickets(context, userManager);
        }

        private static async void SeedRoles(RoleManager<IdentityRole> roleManager, List<string> roles)
        {
            IdentityResult seededRole = null;
            foreach (var role in roles)
            {
                if (!roleManager.Roles.Any(r => r.Name == role))
                {
                    seededRole = await roleManager.CreateAsync(new IdentityRole { Name = role });
                }
            }
        }

        private static async void SeedUsers(UserManager<ApplicationUser> userManager, List<SeededUser> seededUsers)
        {
            foreach (var user in seededUsers)
            {
                var userLookup = await userManager.FindByEmailAsync(user.Email);
                if (userLookup == null)
                {
                    string userIdGuid = Guid.NewGuid().ToString().ToLower();
                    userLookup = new ApplicationUser
                    {
                        Id = userIdGuid,
                        Email = user.Email,
                        FirstName = user.FirstName ?? String.Empty,
                        LastName = user.LastName ?? String.Empty,
                        IsDemoUser = user.IsDemoUser,
                        UserName = user.UserName
                    };
                    var result = await userManager.CreateAsync(userLookup, "Test1234!");
                    if (result != IdentityResult.Success)
                    {
                        throw new InvalidOperationException($"Failed to seed user: {user.Email}");
                    }

                    await userManager.AddToRoleAsync(userLookup, user.Role);
                }
            }
        }

        private static async void SeedTickets(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            if (!context.Tickets.Any(t => t.Title == "Ticket One"))
            {
                var seededTicket = new Ticket { Title = "Ticket One", Description = "First Ticket!", Created = DateTime.Now, TicketPriority = Priority.High };
                var project = context.Projects.FirstOrDefault(p => p.Name.Contains("Bug Tracker"));
                if (project is not null)
                {
                    seededTicket.ProjectId = project.Id;

                    var ticketOwner = await userManager.FindByEmailAsync("ira@foo.com");
                    if (ticketOwner is not null)
                    {
                        seededTicket.OwnerId = ticketOwner.Id;
                    }

                    var ticketAssignee = await userManager.FindByEmailAsync("dev@foo.com");
                    if (ticketAssignee is not null)
                    {
                        seededTicket.AssignedToId = ticketAssignee.Id;
                    }
                    context.Tickets.Add(seededTicket);
                    context.SaveChanges();
                }
            }
        }
    }
}
