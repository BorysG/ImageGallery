using ImageGallery.Search.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ImageGallery.Search.DAL.Repositories.Interfaces
{
    public interface ITagRepository
    {
        Task<TagEntity> AddAsync(TagEntity tagEntity);
        Task<IEnumerable<TagEntity>> GetAsync(string searchTerm);
    }
}
