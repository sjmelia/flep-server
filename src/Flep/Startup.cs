namespace Flep
{
    using System;
    using ArrayOfBytes.BirdBranch;
    using ArrayOfBytes.OAuth.Client;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    public class Startup
    {
        public static readonly string MessagingCategory = "Messaging";

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
                loggerFactory.AddTwitterDirectMessage(oAuthInfo, oAuthInfoSection["recipient"], (c, ll) => c == MessagingCategory);
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
