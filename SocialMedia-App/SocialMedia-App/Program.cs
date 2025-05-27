using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Data.Data;
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
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IFollowerRepository, FollowerRepository>();
            builder.Services.AddRazorPages();
            builder.Services.AddSignalR();


            builder.Services
                    .AddIdentity<ApplicationUser, IdentityRole>(options =>
                    {
                        options.Lockout.AllowedForNewUsers = false;
                        options.User.RequireUniqueEmail = true;
                    })
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddDefaultTokenProviders();
            builder.Services.AddScoped<CustomUserManager>();
            builder.Services
                    .AddScoped<UserManager<ApplicationUser>, CustomUserManager>();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                // Use the Identity UI’s login page
                options.LoginPath = "/Identity/Account/Login";
                // Optional: if you want a custom access‐denied page
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.SlidingExpiration = true;
            });


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
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapRazorPages();
            app.UseWebSockets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{area=User}/{controller=Home}/{action=Index}/{id?}");
            app.MapHub<ChatHub>("/chat");
            app.Run();
        }
    }
}
