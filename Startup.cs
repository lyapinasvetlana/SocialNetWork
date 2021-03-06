using System;
using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SocialNetWork.Models;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;

namespace SocialNetWork
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddControllersWithViews(options =>
            {
                options.EnableEndpointRouting = false;
            });
           
            services.AddEntityFrameworkNpgsql().AddDbContext<ApplicationContext>(options =>
            {
                options.UseNpgsql(Config.Config.SetConfig());
            });
            
            /*//добавили роли
            services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true) 
                .AddEntityFrameworkStores<ApplicationContext>();;*/
           
            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    //options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;

                })
                .AddCookie(options =>
                {
                    options.LoginPath = "/Home/";
                    options.AccessDeniedPath = "/Home/";
                })
                .AddGoogle(options =>
                {
                    options.ClientId = Environment.GetEnvironmentVariable("IDGOOGLE");
                    options.ClientSecret = Environment.GetEnvironmentVariable("SECRETGOOGLE");
                })
                .AddYahoo(yahooOptions=>
                {
                    yahooOptions.ClientId = Environment.GetEnvironmentVariable("IDYAHOO");
                    yahooOptions.ClientSecret = Environment.GetEnvironmentVariable("SECRETYAHOO");
                })
                .AddGitHub(options =>
                {
                    options.ClientId = Environment.GetEnvironmentVariable("IDGITHUB");
                    options.ClientSecret = Environment.GetEnvironmentVariable("SECRETGITHUB");
                });
            services.AddControllersWithViews();
            
            /*services.Configure<SecurityStampValidatorOptions>(options =>
            {
                options.ValidationInterval = TimeSpan.Zero;   
            });*/
            services.ConfigureApplicationCookie(options => options.LoginPath = "/Home");
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseForwardedHeaders();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                
                //app.UseHsts();
            }

           
            
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}