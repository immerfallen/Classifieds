using Classifieds.Data;
using Classifieds.Data.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Classifieds.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DatabaseConnection"))
            );
            services.AddDefaultIdentity<User>(opts => {
                opts.Password.RequireDigit = true;                
                opts.Password.RequiredLength = 8;
                opts.Password.RequireNonAlphanumeric = true;
                opts.Password.RequireUppercase = true;
                opts.SignIn.RequireConfirmedAccount = true;

            }).AddEntityFrameworkStores<ApplicationDbContext>();


            //o filter.add adiciona o filtro [Authorize] para todas as pages 
            services.AddRazorPages().AddMvcOptions(q=>q.Filters.Add(new AuthorizeFilter()));

            //para usar com mv, corpiar a linha abaixo
            //services.AddControllersWithViews(q => q.Filters.Add(new AuthorizeFilter()));

            services.AddAuthentication(o =>
            {
                o.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                //o.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
                //o.DefaultChallengeScheme = FacebookDefaults.AuthenticationScheme;
                //o.DefaultChallengeScheme = MicrosoftAccountDefaults.AuthenticationScheme;
            })
                .AddCookie(q => q.LoginPath = "/Auth/Login");
                //.AddGoogle(o => { o.ClientId = Configuration["Google:ClientId"]; o.ClientSecret = Configuration["Google:ClientSecret"]; });
        //        .AddMicrosoftAccount(o => { o.ClientId = Configuration["Google:ClientId"]; o.ClientSecret = Configuration["Google:ClientSecret"]; })
        //        .AddFacebook(o => { o.ClientId = Configuration["Google:ClientId"]; o.ClientSecret = Configuration["Google:ClientSecret"]; });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
