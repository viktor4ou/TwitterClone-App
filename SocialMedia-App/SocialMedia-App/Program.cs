using Microsoft.EntityFrameworkCore;
using SocialMedia.Data.Data;
using Microsoft.AspNetCore.Identity;
using SocialMedia.Data.Interfaces;
using SocialMedia.Data.Repository;
using SocialMedia.Models.Hubs;
using SocialMedia.Models.Models;

namespace SocialMedia_App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
           
            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection1")));
            builder.Services.AddScoped<IPostRepository, PostRepository>();
            builder.Services.AddScoped<ICommentRepository, CommentRepository>();
            builder.Services.AddScoped<ILIkeRepository, LikeRepository>();
            builder.Services.AddRazorPages();
            builder.Services.AddSignalR();
            builder.Services.AddDefaultIdentity<IdentityUser>(options =>
                {
                    options.Lockout.AllowedForNewUsers = false; // Disable account lockout
                    options.User.RequireUniqueEmail = true;

                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddUserManager<CustomUserManager>();

            var app = builder.Build();
                
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.MapRazorPages();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseWebSockets();
            app.MapControllerRoute(
                name: "default",    
                pattern: "{area=User}/{controller=Home}/{action=Index}/{id?}");
            app.MapHub<ChatHub>("/chat");
            app.Run();
        }
    }
}
