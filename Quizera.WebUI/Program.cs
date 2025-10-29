using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Quizera.BLL;
using Quizera.DAL;
using Quizera.Domain;

namespace Quizera.WebUI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            #region App Builder 

            var builder = WebApplication.CreateBuilder(args);

            #region Built-In Services: Already dclared -> Need To Register 

            #region AddControllersWithViews
            builder.Services.AddControllersWithViews();
            //builder.Services.AddControllersWithViews(options =>
            //{
            //    var policy = new AuthorizationPolicyBuilder()
            //        .RequireAuthenticatedUser()
            //        .Build();
            //    options.Filters.Add(new AuthorizeFilter(policy));
            //});
            #endregion

            #region DbContext

            builder.Services.AddDbContext<QuizeraDB>(options =>
                        options.UseSqlServer(builder.Configuration.GetConnectionString("QuizeraDB")));
            #endregion

            #region All Needed Services
            
            #region Add AutoMapper
            builder.Services.AddAutoMapper(cfg =>
            {
                cfg.LicenseKey = builder.Configuration["AutoMapper:LicenseKey"];
            }, typeof(AccountProfile).Assembly);  // Scans Core assembly for profiles 
            #endregion

            #region Fluent Validators
            // Register all validators in this assembly
            //builder.Services.AddValidatorsFromAssemblyContaining<InstructorCreateVMValidator>();
            #endregion

            #region Session Regiteration
            // Add services for Session
            builder.Services.AddDistributedMemoryCache(); // Required for session storage
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Session timeout
                options.Cookie.HttpOnly = true;   // Prevent JS access
            });
            #endregion

            #region Identity Services
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                // -------------------
                // Password settings
                // -------------------
                options.Password.RequireDigit = false;                // Must contain a number
                options.Password.RequireLowercase = false;            // Must contain a lowercase letter
                options.Password.RequireUppercase = false;            // Must contain an uppercase letter
                options.Password.RequireNonAlphanumeric = false;     // Must contain a special character
                options.Password.RequiredLength = 4;                // Minimum length
                options.Password.RequiredUniqueChars = 0;           // Minimum unique characters


                // -------------------
                // User settings
                // -------------------
                options.User.RequireUniqueEmail = true;           // Email must be unique
                options.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+"; // Allowed username chars


                // -------------------
                // Lockout settings
                // -------------------
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); // Lockout duration
                options.Lockout.MaxFailedAccessAttempts = 5;                       // Max failed attempts
                options.Lockout.AllowedForNewUsers = true;                         // Lockout enabled for new users


                // -------------------
                // SignIn settings
                // -------------------
                options.SignIn.RequireConfirmedEmail = false;     // Require email confirmation
                options.SignIn.RequireConfirmedPhoneNumber = false; // Require phone confirmation
            })
            .AddEntityFrameworkStores<QuizeraDB>();
            #endregion

            #region Cookie Service
            builder.Services.AddAuthentication("Cookies")
                .AddCookie("Cookies", options =>
                {
                    // 🔹 Paths
                    options.LoginPath = "/Account/Login";   // Redirect here if not logged in
                    options.LogoutPath = "/Account/SignOut";     // Redirect here after logout
                    options.AccessDeniedPath = "/Account/AccessDenied"; // For [Authorize] failed

                    // 🔹 Cookie Settings
                    options.Cookie.Name = "Quizera";          // Cookie name
                    options.Cookie.HttpOnly = true;             // Can't be accessed by JS
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Only HTTPS
                    options.Cookie.SameSite = SameSiteMode.Strict; // Strict CSRF protection

                    // 🔹 Expiration
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(60); // Cookie lifetime
                    options.SlidingExpiration = true;  // Refresh expiration on activity

                    // 🔹 Return URL
                    options.ReturnUrlParameter = "returnUrl"; // Query string for redirect

                });
            #endregion

            #endregion

            #endregion

            #region Custom Service: -> Need To Register and Inject 

            //AddScoped → Object per request / HTTP (Recomended)
            //AddSingleton → Object for the whole application 
            //AddTransient → Object Per Injection

            #region Generic Repo & Service
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            builder.Services.AddScoped(typeof(IGenericService<>), typeof(GenericService<>));
            #endregion

            //
            builder.Services.AddScoped<IExamRepository, ExamRepository>();
            builder.Services.AddScoped<IExamService, ExamService>();

            builder.Services.AddScoped<IAttemptRepository, AttemptRepository>();
            builder.Services.AddScoped<IAttemptService, AttemptService>();


            #endregion


            //Build WebApplication Object
            var app = builder.Build();

            #endregion

            #region Middlewares
            // Handle the HTTP request/Response pipeline.

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }

            // Use Session Middleware
            app.UseSession();

            app.UseRouting(); //Matches incoming URL to endpoints.

            #region MyRegiWhat UseAuthentication() doeson
            //When a request comes in:

            //It reads the cookie specified by CookieAuthenticationOptions.Cookie.Name(e.g., "MyAppAuthCookie").

            //It decrypts and validates the cookie automatically using the Data Protection API, which was used to encrypt it when it was issued.

            //If the cookie is valid and not expired, it creates a ClaimsPrincipal and sets HttpContext.User to that user.

            //If the cookie is missing, tampered, or expired, the user is treated as unauthenticated.
            #endregion

            app.UseAuthentication(); //validate user credentials

            app.UseAuthorization(); //Check Permissions

            app.MapStaticAssets(); //Serves CSS/ JS / images from wwwroot using Ajax Calls


            app.MapControllerRoute(  //Directs requests to controllers/actions.
                name: "default",
                pattern: "{controller=Account}/{action=Login}/{id?}")
                .WithStaticAssets();

            #endregion

            #region App Run Middleware to Terminate Pipline and build App

            app.Run();

            #endregion
        }
    }
}
