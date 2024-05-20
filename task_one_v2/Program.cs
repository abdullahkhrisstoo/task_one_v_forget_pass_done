using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Globalization;
using task_one_v2.Models;
using task_one_v2.App_Core.StateMangement;
using task_one_v2.App_Core.ErrorHandler;
using Microsoft.AspNetCore.Identity;
using task_one_v2.App_Core.FileHelper;
using System.Configuration;
using task_one_v2.App_Core.Mail;
using static task_one_v2.App_Core.StateMangement.IEmailService;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Options;

namespace task_one_v2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<ModelContext>(x => x.UseOracle(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("Smtp"));
            builder.Services.AddRazorPages().AddRazorRuntimeCompilation();


            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

                options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            })
         .AddCookie(options =>
            {
                options.Cookie.IsEssential = true;
            })
          .AddGoogle(googleOptions =>
          {
              googleOptions.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

              googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
              googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
              //googleOptions.CallbackPath = "/signin-google";
          });
            builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(@"./keys"))
    .SetApplicationName("task_one_v2")
    .SetDefaultKeyLifetime(TimeSpan.FromDays(14));

            //todo: Start Session config
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            //todo: End Session config


            //todo: START LOCALIZATION CONFIG
            builder.Services.AddLocalization();
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>();

            builder.Services.AddMvc()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization(options =>
                {
                    options.DataAnnotationLocalizerProvider = (type, factory) =>
                        factory.Create(typeof(JsonStringLocalizerFactory));
                });

            builder.Logging.AddDebug();
            builder.Logging.SetMinimumLevel(LogLevel.Debug);
            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                   new CultureInfo("en-US"),
                   new CultureInfo("ar-JO")
                };

                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            //todo: END LOCALIZATION CONFIG


            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });



            //todo:File Helper
            builder.Services.AddScoped<FileHelper>();
            builder.Services.AddScoped<ICurrentChefManager, CurrentChefManager>();
            builder.Services.AddScoped<IAdminManager, CurrentAdminManager>();
            builder.Services.AddScoped<IAuthManager, AuthManager>();
            builder.Services.AddScoped<IUserManager, CurrentUserManger>();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddHttpContextAccessor();

            //todo:End File Helper

            var app = builder.Build();
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }






            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();


            //TODO: START CONFIG LANGUAGE
            var supportedCultures = new[] { "en-US", "ar-JO" };
            var localizationOptions = new RequestLocalizationOptions()
                //.SetDefaultCulture(supportedCultures[0])
                .AddSupportedCultures(supportedCultures)
                .AddSupportedUICultures(supportedCultures);

            app.UseRequestLocalization(localizationOptions);

            //TODO; END CONFIG FOR LANGUAGE
            app.UseSession(); 
            app.UseAuthentication(); 
            app.UseAuthorization(); 


            //todo:error handdler
            //app.UseStatusCodePages(async context => {
            //    if (context.HttpContext.Response.StatusCode == StatusCodes.Status404NotFound)
            //    {
            //        context.HttpContext.Response.ContentType = "text/html; charset=utf-8";
            //        await context.HttpContext.Response.WriteAsync(_404Error.code);
            //    }
            //});

            //todo: end error handler

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Home}/{id?}");
            app.Run();
        }
    }
}

