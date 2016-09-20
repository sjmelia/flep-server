namespace Flep
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using ArrayOfBytes.BirdBranch;
    using ArrayOfBytes.OAuth.Client;
    using Flep.Models;

    public class Startup
    {
        public static readonly string StatisticsCategory = "Statistics";

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            builder.AddEnvironmentVariables();
            this.Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddCors(options =>
                    {
                        options.AddPolicy(
                            "default",
                            builder => builder.WithOrigins("*")
                                .WithMethods("*")
                                .WithHeaders("*")
                                .AllowCredentials());
                    });

            services.AddSingleton<IDataService, MongoDataService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(this.Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            var oAuthInfoSection = this.Configuration.GetSection("TwitterOAuth");
            if (oAuthInfoSection != null && !string.IsNullOrEmpty(oAuthInfoSection["ConsumerKey"]))
            {
                var oAuthInfo = new OAuthConfig(
                    oAuthInfoSection["ConsumerKey"],
                    oAuthInfoSection["ConsumerSecret"],
                    oAuthInfoSection["AccessToken"],
                    oAuthInfoSection["AccessSecret"]);
                Console.WriteLine("Found oauth section e.g. " + oAuthInfoSection["ConsumerKey"]);
                loggerFactory.AddTwitterDirectMessage(oAuthInfo, "thesjmelia", (c, ll) => c == StatisticsCategory);
            }
            else
            {
                Console.WriteLine("No oauth found");
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }

            app.UseCors("default");

            app.UseMvc(routes =>
                    {
                    routes.MapRoute(
                            name: "default",
                            template: "{controller=Home}/{action=Index}/{id?}");
                    });
        }
    }
}
