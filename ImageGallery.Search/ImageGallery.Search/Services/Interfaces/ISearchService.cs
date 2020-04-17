using ImageGallery.Search.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ImageGallery.Search.Services.Interfaces
{
    public interface ISearchService
    {
        Task<IEnumerable<ImageLocalModel>> SearchImages(string searchTerm);
    }
}
