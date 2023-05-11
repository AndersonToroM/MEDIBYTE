using CodeGenerator.Data;
using DevExpress.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SignalRChat.Hubs;
using System;
using System.Collections.Generic;
using WebEssentials.AspNetCore.Pwa;

namespace CodeGenerator
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
            services.AddMvc().AddNewtonsoftJson(options =>

            {
                options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                options.SerializerSettings.TypeNameHandling = TypeNameHandling.Objects;
                options.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.Objects;
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
                options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                options.SerializerSettings.DateFormatString = "yyyy-MM-ddTHH:mm:ss.fff";
            });

            services.Configure<CookiePolicyOptions>(options =>
            {
                    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                    options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddSignalR(hubOptions =>
            {
                hubOptions.EnableDetailedErrors = true;
                hubOptions.MaximumReceiveMessageSize = long.MaxValue;
                    //hubOptions.KeepAliveInterval = TimeSpan.FromMinutes(1);
                });
            services.AddProgressiveWebApp(new PwaOptions
            {
                AllowHttp = true
            });

            services.AddDevExpressControls();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            string prefix = (Environment.GetEnvironmentVariable("NETWIZARD_PREFIX") ?? "/Wizard").Trim();
            if (!string.IsNullOrEmpty(prefix))
            {
                if (!prefix.StartsWith("/"))
                {
                    prefix = "/" + prefix;
                }
                if (prefix.EndsWith("/"))
                {
                    prefix = prefix.Substring(0, prefix.Length);
                }
                Console.WriteLine("Using Prefix: " + prefix);
                app.UsePathBase(new PathString(prefix));
            }

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
            app.UseDevExpressControls();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
                //endpoints.MapBlazorHub();

                endpoints.MapHub<GenerateHub>("/GenerateHub", (config) =>
                {
                    config.ApplicationMaxBufferSize = long.MaxValue;
                    config.TransportMaxBufferSize = long.MaxValue;
                    config.Transports = HttpTransportType.WebSockets;
                });

            });

            //app.UseSignalR(routes =>
            //{
            //    routes.MapHub<GenerateHub>("/GenerateHub", (config) =>
            //         {
            //        config.ApplicationMaxBufferSize = long.MaxValue;
            //        config.TransportMaxBufferSize = long.MaxValue;
            //        config.Transports = HttpTransportType.WebSockets;
            //    });

            //});

            //GCUtil.LoadDBData();
            GCUtil.ListDBSettings = Configuration.GetSection("DatabaseConfig").Get<List<DBSettings>>();
            foreach (var dbSettings in GCUtil.ListDBSettings)
            {
                Scheme scheme = new Scheme(dbSettings);

                //scheme.tableModels.Add(new Models.TableModel { Name = "Sedes" });
                //scheme.tableModels.Add(new Models.TableModel { Name = "Convenios" });
                //scheme.tableModels.Add(new Models.TableModel { Name = "Sedes" });
                //scheme.tableModels.Add(new Models.TableModel { Name = "Languages" });
                //scheme.tableModels.Add(new Models.TableModel { Name = "LanguageResources" });
                //scheme.tableModels.Add(new Models.TableModel { Name = "UserOffices" });

                GCUtil.Framework = 1;
                GCUtil.DataBaseInfo.Add(scheme.GetDataBaseInfo());
            }
        }
    }


}
