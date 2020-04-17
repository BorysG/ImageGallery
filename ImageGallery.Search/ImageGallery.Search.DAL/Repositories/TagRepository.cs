using ImageGallery.Search.DAL.Context;
using ImageGallery.Search.DAL.Entities;
using ImageGallery.Search.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageGallery.Search.DAL.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly ImageGalleryContext imageGalleryContext;

        public TagRepository(ImageGalleryContext imageGalleryContext)
        {
            this.imageGalleryContext = imageGalleryContext;
        }

        public async Task<TagEntity> AddAsync(TagEntity tagEntity)
        {
            var existingEntity = imageGalleryContext.Tag.FirstOrDefault(tag => tag.Name.Equals(tagEntity.Name));
            if (existingEntity == null)
            { 
                imageGalleryContext.Tag.Add(tagEntity);
                await imageGalleryContext.SaveChangesAsync();
            }
            else
            {
                tagEntity.Id = existingEntity.Id;
            }

            return tagEntity;
        }

        public async Task<IEnumerable<TagEntity>> GetAsync(string searchTerm)
        {
            return await imageGalleryContext.Tag.Where(tag => tag.Name.Contains(searchTerm)).ToListAsync();
        }
    }
}
