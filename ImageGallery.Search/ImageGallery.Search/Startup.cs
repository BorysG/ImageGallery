using System;
using Hangfire;
using Hangfire.SqlServer;
using ImageGallery.Search.Container;
using ImageGallery.Search.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ImageGallery.Search
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddDbContext();
            services.AddRepositories();
            services.AddServices();
            services.AddControllers();

            services.AddHangfire(config => config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                                                 .UseSimpleAssemblyNameTypeSerializer()
                                                 .UseRecommendedSerializerSettings()
                                                 .UseSqlServerStorage(configuration.GetConnectionString("ImageGalleryConnection"), new SqlServerStorageOptions
                                                 {
                                                     CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                                                     SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                                                     QueuePollInterval = TimeSpan.Zero,
                                                     UseRecommendedIsolationLevel = true,
                                                     UsePageLocksOnDequeue = true,
                                                     DisableGlobalLocks = true
                                                 }));

            services.AddHangfireServer();
        }

        public void Configure(IApplicationBuilder app, 
                              IWebHostEnvironment env, 
                              IImageService imageService)
        {
            imageService.DownloadImages().GetAwaiter().GetResult();
            RecurringJob.AddOrUpdate("DownloadImagesJob", () => imageService.DownloadImages(), configuration.GetSection("Jobs")["DownloadImagesCron"]);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Use '/search/{searchTerm}' to search images");
                });
            });
        }
    }
}
