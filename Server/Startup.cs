using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Server.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            var assembly = typeof(Program).Assembly.GetName().Name;

            //SeedData.EnsureSeedData(_configuration.GetConnectionString("DefaultConnection"));

            services.AddDbContext<AspNetIdentityDbContext>(option =>
            option.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"),
            b => b.MigrationsAssembly(assembly)));
           

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<AspNetIdentityDbContext>();



            services.AddIdentityServer()
                .AddAspNetIdentity<IdentityUser>() 
                .AddConfigurationStore(option =>
                {
                    option.ConfigureDbContext = b => b.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"),
                         option => option.MigrationsAssembly(assembly));
                }).AddOperationalStore(option =>
                {
                    option.ConfigureDbContext = b => b.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"),
                          option => option.MigrationsAssembly(assembly));

                })
                .AddDeveloperSigningCredential();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            app.UseRouting();
            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
