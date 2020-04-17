using ImageGallery.Search.DAL.Entities;
using ImageGallery.Search.DAL.Repositories.Interfaces;
using ImageGallery.Search.Models;
using ImageGallery.Search.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ImageGallery.Search.Services
{
    public class ImageService : IImageService
    {
        private readonly HttpClient httpClient;
        private readonly ILogger<ImageService> logger;
        private readonly ITagRepository tagRepository;
        private readonly IImageRepository imageRepository;

        private readonly string apiKey;

        public ImageService(IHttpClientFactory clientFactory,
            IConfiguration configuration,
            ILogger<ImageService> logger,
            IImageRepository imageRepository,
            ITagRepository tagRepository)
        {
            this.logger = logger;
            this.imageRepository = imageRepository;
            this.tagRepository = tagRepository;

            httpClient = clientFactory.CreateClient();

            IConfigurationSection gallerySection = configuration.GetSection("Gallery");
            httpClient.BaseAddress = new Uri(gallerySection["RemoteAPI"]);
            apiKey = gallerySection["ApiKey"];
        }

        public async Task DownloadImages()
        {
            logger.LogInformation($"Image download has been started...");
            string token = await GetTokenAsync();
            if (string.IsNullOrEmpty(token))
                return;

            await ProcessPagesAsync(token);
            logger.LogInformation($"Image download has been finished...");
        }

        private async Task<string> GetTokenAsync()
        {
            var token = string.Empty;

            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, "/auth");
                var content = new
                {
                    apiKey
                };
                var json = JsonConvert.SerializeObject(content);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                var result = await httpClient.SendAsync(request);
                string resultContent = await result.Content.ReadAsStringAsync();
                dynamic responseData = JsonConvert.DeserializeObject(resultContent);
                token = responseData.token;
            }
            catch (Exception ex)
            {
                logger.LogError($"Unable to get authentication token: {ex}");
            }

            return token;
        }

        private async Task ProcessPagesAsync(string token)
        {
            var pageIsNotEmpty = true;
            for (int i = 1; pageIsNotEmpty; i++)
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"/images?page={i}");
                request.Headers.Add("Authorization", $"Bearer {token}");
                var result = await httpClient.SendAsync(request);
                string resultContent = await result.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeObject<ImagesPageModel>(resultContent);
                pageIsNotEmpty = responseData.Pictures.Any();
                await ProcessImagesPageAsync(responseData.Pictures, token);
            }
        }

        private async Task ProcessImagesPageAsync(ImageModel[] images, string token)
        {
            foreach (var image in images)
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"/images/{image.Id}");
                request.Headers.Add("Authorization", $"Bearer {token}");
                var result = await httpClient.SendAsync(request);
                string resultContent = await result.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeObject<ImageFullModel>(resultContent);
                await SaveImageAsync(responseData);
            }
        }

        private async Task SaveImageAsync(ImageFullModel imageFullModel)
        {
            try
            {
                List<ImageTagEntity> savedTags = await SaveTagsAsync(imageFullModel.Tags);
                var imageEntity = new ImageEntity
                {
                    ExternalId = imageFullModel.Id,
                    Author = imageFullModel.Author,
                    Camera = imageFullModel.Camera,
                    CroppedPicture = imageFullModel.Cropped_picture,
                    FullPicture = imageFullModel.Full_picture,
                    ImageTags = savedTags
                };
                await imageRepository.AddOrUpdate(imageEntity);
            }
            catch (Exception ex)
            {
                logger.LogError($"Image with Id={imageFullModel.Id} could not be saved. {ex}");
            }
        }

        private async Task<List<ImageTagEntity>> SaveTagsAsync(string tagsString)
        {
            var savedTags = new List<ImageTagEntity>();
            if (string.IsNullOrEmpty(tagsString))
                return savedTags;

            var tags = tagsString.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            foreach (var tag in tags)
            {
                var tagEntity = new TagEntity
                {
                    Name = tag
                };

                TagEntity savedTag = await tagRepository.AddAsync(tagEntity);
                savedTags.Add(new ImageTagEntity
                {
                    TagId = savedTag.Id
                });
            }

            return savedTags;
        }
    }
}
