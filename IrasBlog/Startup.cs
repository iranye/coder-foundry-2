using IrasBlog.Data;
using IrasBlog.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace IrasBlog
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>(cfg =>
            {
                cfg.User.RequireUniqueEmail = true;
                cfg.Password.RequiredLength = 6;
                cfg.Password.RequireNonAlphanumeric = false;
            })
                .AddEntityFrameworkStores<BlogContext>();

            services.AddDbContext<BlogContext>(cfg =>
            {
                cfg.UseSqlServer();
            });
            services.AddTransient<DbSeeder>();

            services.AddScoped<IBlogRepository, BlogRepository>();

            services.AddControllersWithViews()
                .AddRazorRuntimeCompilation();

            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStaticFiles();            

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(cfg =>
            {
                cfg.MapRazorPages();
                cfg.MapControllerRoute("Default",
                    "{controller}/{action}/{slug?}",
                    new { controller = "Home", action = "Index" });
            });
        }
    }
}
