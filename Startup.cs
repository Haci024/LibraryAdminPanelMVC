using LibraryAdminPanel.DAL;
using LibraryAdminPanel.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAdminPanel
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
            services.AddControllersWithViews();
            services.AddIdentity<AppUser, IdentityRole>(IdentityOption =>
            {

                IdentityOption.Password.RequireUppercase = true;//Sifrede boyuk herf olsun!
                IdentityOption.Password.RequireDigit = true;//reqemlerden istifade!
                IdentityOption.Password.RequireLowercase = true;//kicik herfde olsun!
                IdentityOption.Password.RequireNonAlphanumeric = false;//Serti isarelerden istifader ele!
                IdentityOption.User.RequireUniqueEmail = true;//User maili unique olsun!(tekrarlanmasin)
                IdentityOption.Lockout.AllowedForNewUsers = true;//website daxil olan istifadecilere qeydiyyatdan kecmeye imkan verir!
                IdentityOption.Lockout.MaxFailedAccessAttempts = 5;// sifreni daxil eden zaman ne qeder sehv cehd etmek olar!
                IdentityOption.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);//Sifreni sehv daxiletme limitini kecdikden sonra bloklanma muddeti!
                IdentityOption.SignIn.RequireConfirmedEmail = false;//Qeydiyyatdan kecende Email tesdiqi!(False=Deaktiv)
                IdentityOption.SignIn.RequireConfirmedPhoneNumber = false;//Qeydiyyatdan kecen zaman Telefon nomresi tesdiqi!(false=Deaktiv)
                IdentityOption.SignIn.RequireConfirmedAccount = false;//




               




            })
              .AddDefaultTokenProviders().AddEntityFrameworkStores<AppDbContext>();
            services.AddDbContext<AppDbContext>(option =>
            {
                option.UseSqlServer(Configuration.GetConnectionString("MyPC"));
            });
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
                app.UseExceptionHandler("/Home/Error");
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
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Dashboard}/{action=Index}/{id?}");
            });
        }
    }
}
