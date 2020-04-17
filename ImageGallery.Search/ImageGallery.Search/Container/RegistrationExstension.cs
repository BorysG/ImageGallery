using ImageGallery.Search.DAL.Context;
using ImageGallery.Search.DAL.Repositories;
using ImageGallery.Search.DAL.Repositories.Interfaces;
using ImageGallery.Search.Services;
using ImageGallery.Search.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ImageGallery.Search.Container
{
    public static class RegistrationExstension
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddTransient<IImageService, ImageService>();
            services.AddTransient<ISearchService, SearchService>();
        }

        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IImageRepository, ImageRepository>();
            services.AddScoped<ITagRepository, TagRepository>();
        }

        public static void AddDbContext(this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var connectionString = configuration.GetConnectionString("ImageGalleryConnection");
            services.AddDbContext<ImageGalleryContext>(options => options.UseSqlServer(connectionString));
        }
    }
}
