using ImageGallery.Search.DAL.Repositories.Interfaces;
using ImageGallery.Search.Models;
using ImageGallery.Search.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageGallery.Search.Services
{
    public class SearchService : ISearchService
    {
        private readonly ITagRepository tagRepository;
        private readonly IImageRepository imageRepository;

        public SearchService(ITagRepository tagRepository,
                             IImageRepository imageRepository)
        {
            this.tagRepository = tagRepository;
            this.imageRepository = imageRepository;
        }

        public async Task<IEnumerable<ImageLocalModel>> SearchImages(string searchTerm)
        {
            var tags = await tagRepository.GetAsync(searchTerm);
            var iamges = await imageRepository.GetAsync(searchTerm);

            return iamges.Select(image => new ImageLocalModel 
            {
                Id = image.Id,
                Author = image.Author,
                Camera = image.Camera,
                CroppedPicture = image.CroppedPicture,
                FullPicture = image.FullPicture,
                Tags = string.Join(' ', image.ImageTags.Select(imageTag => imageTag.Tag.Name))
            });
        }
    }
}
