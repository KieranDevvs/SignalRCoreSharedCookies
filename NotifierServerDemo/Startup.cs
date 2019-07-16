using System.IO;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace NotifierServerDemo
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
            services.AddSignalR();

            services.AddDataProtection()
            .PersistKeysToFileSystem(GetKeyRingDirInfo())
            .SetApplicationName("SharedCookieApp");

            services.ConfigureApplicationCookie(options => {
                options.Cookie.Name = ".AspNet.SharedCookie";
            });

            services.AddAuthentication(o => {
                o.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        private DirectoryInfo GetKeyRingDirInfo()
        {
            return new DirectoryInfo(Program.KeyRingPath);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseCors(builder =>
            {
                builder
                .WithOrigins("https://localhost:5001")
                .WithOrigins("http://localhost:5000")
                .AllowAnyHeader()
                .WithMethods("GET", "POST")
                .AllowCredentials();
            });

            app.UseSignalR(routes =>
            {
                routes.MapHub<TaskHub>("/taskHub");
            });

            app.UseAuthentication();
        }
    }
}
