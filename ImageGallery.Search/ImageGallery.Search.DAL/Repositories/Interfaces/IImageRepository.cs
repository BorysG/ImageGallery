using ImageGallery.Search.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ImageGallery.Search.DAL.Repositories.Interfaces
{
    public interface IImageRepository
    {
        Task AddOrUpdate(ImageEntity imageEntity);
        Task<IEnumerable<ImageEntity>> GetAsync(string searchTerm);
    }
}
