using GleamTech.AspNet.Core;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebTools.Authorization;
using WebTools.Context;
using WebTools.Services;
using WebTools.Services.Interface;

namespace WebTools
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
            services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
            services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

            services.AddControllersWithViews();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => {
                    options.LoginPath = "/User/Login";
                    options.AccessDeniedPath = "/User/Denied";
                    options.Events = new CookieAuthenticationEvents()
                    {
                        OnSigningIn = async context =>
                        {
                            //var principal = context.Principal;
                            //if (principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value == "Admin")
                            //{
                            //    var claimsIdentity = principal.Identity as ClaimsIdentity;
                            //    claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, "Admin"));
                            //}
                            //else
                            //{
                            //    var claimsIdentity = principal.Identity as ClaimsIdentity;
                            //    claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, "User"));
                            //} 
                            var principal = context.Principal;
                            if (principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value == "")
                            {                                
                                var claimsIdentity = principal.Identity as ClaimsIdentity;
                                claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, "User"));
                            }

                            await Task.CompletedTask;
                        },
                        OnSignedIn = async context =>
                        {
                            await Task.CompletedTask;
                        },
                        OnValidatePrincipal = async context =>
                        {
                            await Task.CompletedTask;
                        }
                    };
                });
            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("Admin", policy => {
            //        policy.RequireAuthenticatedUser();                   
            //        policy.RequireRole("Admin");
            //        //policy.RequireClaim("Permission","");
            //    });
            //    options.AddPolicy("ITOnly", policy => policy.RequireClaim("Permission", "IT"));
            //    options.AddPolicy("SuperIT",policy => policy.RequireClaim("Permission", "IT")
            //                                                .RequireClaim("IT"));
            //});
            services.AddScoped<IReportListServices, ReportListServices>();
            services.AddScoped<IReportVersionServices, ReportVersionServices>();
            services.AddScoped<IReportSoftServices, ReportSoftServices>();
            services.AddScoped<IReportDetailServices, ReportDetailServices>();
            services.AddScoped<IReportURDServices, ReportURDServices>();
            services.AddScoped<IDepts, DeptsServices>();
            services.AddScoped<IRolesServices, RolesServices>();
            services.AddScoped<IUserServices, UserServices>();
            services.AddScoped<IModuleControllerServices, ModuleControllerServices>();
            services.AddScoped<IModuleActionServices, ModuleActionServices>();
            services.AddScoped<IGoogleDriveAPI, GoogleDriveAPI>();
            services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ToolsDB")));
            //Add GleamTech to the ASP.NET Core services container.
            //----------------------
            services.AddGleamTech();
            //----------------------

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
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            //Register GleamTech to the ASP.NET Core HTTP request pipeline.
            //----------------------
            app.UseGleamTech();
            //----------------------
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });


        }
    }
}
