using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthorizationAuthentication.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AuthorizationAuthentication
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
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddControllersWithViews();

            services.AddAuthentication("Cookie").AddCookie("Cookie", config =>
            {
                config.LoginPath = "/Admin/Login";
                config.AccessDeniedPath = "/Home/AccessDenied";
            });

            services.AddAuthorization(options=>
            {
                options.AddPolicy("Administrator", builder =>
                {
                    builder.RequireAssertion(x=>x.User.HasClaim(ClaimTypes.Role, "Administrator"));
                });
                //options.AddPolicy("Manager", builder =>
                //{
                //    builder.RequireClaim(ClaimTypes.Role, "Manager");
                //});
                options.AddPolicy("Manager", builder =>
                {
                    builder.RequireAssertion(x => x.User.HasClaim(ClaimTypes.Role, "Manager")
                                                  || x.User.HasClaim(ClaimTypes.Role, "Administrator"));
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
