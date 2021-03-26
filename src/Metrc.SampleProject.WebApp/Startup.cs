using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Data.Common;
using System.Data.SqlClient;
using Metrc.SampleProject.Services.Infrastructure;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Serialization;

namespace Metrc.SampleProject.WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            var connectionString = Configuration?.GetSection("ConnectionStrings:ViewStore:connectionString").Value;
            ProviderName = Configuration?.GetSection("ConnectionStrings:ViewStore:providerName").Value;
            if (connectionString == null)
            {
                throw new Exception("No ConnectionString found.");
            }

            ConnectionString = connectionString;
            if (String.IsNullOrWhiteSpace(connectionString) ||
                String.Equals(ProviderName, "System.Data.SqlClient"))
            {
                DbProviderFactories.RegisterFactory("System.Data.SqlClient", SqlClientFactory.Instance);
                Repositories.DbFactory = new RepositoryConnectionFactory("Metrc.SampleProject", connectionString);
            }
        }

        public IConfiguration Configuration { get; }
        public static String ConnectionString { get; set; }
        public static String ProviderName { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddHttpClient();
            services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());

            services.AddRazorPages().AddRazorRuntimeCompilation();
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
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
