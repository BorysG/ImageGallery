using ImageGallery.Search.DAL.Context;
using ImageGallery.Search.DAL.Entities;
using ImageGallery.Search.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageGallery.Search.DAL.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly ImageGalleryContext imageGalleryContext;

        public ImageRepository(ImageGalleryContext imageGalleryContext)
        {
            this.imageGalleryContext = imageGalleryContext;
        }

        public async Task AddOrUpdate(ImageEntity imageEntity)
        {
            var existingEntity = imageGalleryContext.Image
                                                    .Include(image => image.ImageTags)
                                                    .FirstOrDefault(image => image.ExternalId.Equals(imageEntity.ExternalId));
            if (existingEntity == null)
                imageGalleryContext.Image.Add(imageEntity);
            else 
            {
                existingEntity.Author = imageEntity.Author;
                existingEntity.Camera = imageEntity.Camera;
                existingEntity.CroppedPicture = imageEntity.CroppedPicture;
                existingEntity.FullPicture = imageEntity.FullPicture;
                foreach (var imageTag in imageEntity.ImageTags)
                    imageTag.ImageId = existingEntity.Id;
                existingEntity.ImageTags = imageEntity.ImageTags;
            }

            await imageGalleryContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<ImageEntity>> GetAsync(string searchTerm)
        {
            return await imageGalleryContext.Image
                                            .Include(image => image.ImageTags)
                                            .Include(image => image.ImageTags).ThenInclude(imageTag => imageTag.Tag)
                                            .Where(image => image.Author.Contains(searchTerm) || 
                                                            image.Camera.Contains(searchTerm) ||
                                                            image.ImageTags.Select(imageTag => imageTag.Tag.Name).Any(name => name.Contains(searchTerm)))
                                            .ToListAsync();
        }
    }
}
