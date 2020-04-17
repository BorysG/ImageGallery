using System.Collections.Generic;
using System.Threading.Tasks;
using ImageGallery.Search.Models;
using ImageGallery.Search.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ImageGallery.Search.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService searchService;

        public SearchController(ISearchService searchService)
        {
            this.searchService = searchService;
        }

        [HttpGet("")]
        [ProducesResponseType(typeof(IEnumerable<ImageLocalModel>), StatusCodes.Status200OK)]
        public IActionResult EmptySearchTerm(string searchTerm)
        {
            return NotFound("Input searchTerm in adress line");
        }

        [HttpGet("{searchTerm}")]
        [ProducesResponseType(typeof(IEnumerable<ImageLocalModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SearchImages(string searchTerm)
        {
            var images = await searchService.SearchImages(searchTerm);
            return Ok(images);
        }
    }
}