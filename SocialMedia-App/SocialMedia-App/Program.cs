using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NpgsqlTypes;
using Serilog;
using Serilog.Sinks.PostgreSQL;
using SocialMedia.Data.Data;
using SocialMedia.Data.Interfaces;
using SocialMedia.Data.Repository;
using SocialMedia.Models.Hubs;
using SocialMedia.Models.Models;
using SocialMedia_App.Services;
using SocialMedia_App.Services.Interfaces;

namespace SocialMedia_App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var logsConnectionString = builder.Configuration.GetConnectionString("LogsConnection")!;
            var columnWriters = new Dictionary<string, ColumnWriterBase>
            {
                { "timestamp",       new TimestampColumnWriter(NpgsqlDbType.TimestampTz) },
                { "level",           new LevelColumnWriter(true, NpgsqlDbType.Varchar) },
                { "message",         new RenderedMessageColumnWriter(NpgsqlDbType.Text) },
                { "message_template", new MessageTemplateColumnWriter(NpgsqlDbType.Text) },
                { "exception",       new ExceptionColumnWriter(NpgsqlDbType.Text) },
                { "properties",      new PropertiesColumnWriter(NpgsqlDbType.Jsonb) },
                { "machine_name",    new SinglePropertyColumnWriter("MachineName", PropertyWriteMethod.ToString, NpgsqlDbType.Text) }
            };

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .WriteTo.Console()
                .WriteTo.PostgreSQL(
                    connectionString: logsConnectionString,
                    tableName: "logs",
                    columnOptions: columnWriters,
                    needAutoCreateTable: true,
                    schemaName: "public")
                .CreateLogger();

            builder.Host.UseSerilog();

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection1")));
            builder.Services.AddScoped<IPostRepository, PostRepository>();
            builder.Services.AddScoped<ICommentRepository, CommentRepository>();
            builder.Services.AddScoped<ILIkeRepository, LikeRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IFollowerRepository, FollowerRepository>();
            builder.Services.AddScoped<CustomUserManager>();
            builder.Services.AddScoped<UserManager<ApplicationUser>, CustomUserManager>();
            builder.Services.AddScoped<IPostService, PostService>();
            builder.Services.AddScoped<IProfileService, ProfileService>();
            builder.Services.AddScoped<IChatService, ChatService>();
            builder.Services.AddTransient<Microsoft.AspNetCore.Identity.UI.Services.IEmailSender, EmailService>();
            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor();
            builder.Services.AddSignalR();

            builder.Services
                    .AddIdentity<ApplicationUser, IdentityRole>(options =>
                    {
                        options.Lockout.AllowedForNewUsers = false;
                        options.User.RequireUniqueEmail = true;
                        options.SignIn.RequireConfirmedAccount = true;
                    })
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddDefaultTokenProviders();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Identity/Account/Login";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.SlidingExpiration = true;
            });


            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseSerilogRequestLogging();
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
            app.MapBlazorHub();
            app.MapFallbackToPage("/blazor/{*path:nonfile}", "/_Host");
            app.MapHub<ChatHub>("/chat");

            try
            {
                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
