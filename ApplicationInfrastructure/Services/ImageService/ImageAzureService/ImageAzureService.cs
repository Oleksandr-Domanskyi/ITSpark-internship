using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Domain.Azure;
using ApplicationCore.Domain.Entity;
using ApplicationCore.Domain.Entity.Image;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace ApplicationInfrastructure.Services.ImageService
{
    public class ImageAzureService<Entity, TDto> : IImageAzureService<Entity, TDto>
    where Entity : Entity<Guid>
    where TDto : class
    {
        private readonly AzureOptions _azureoptions;
        private readonly HttpClient _httpClient;

        public ImageAzureService(IOptions<AzureOptions> azureoptions)
        {
            _azureoptions = azureoptions.Value;
            _httpClient = new HttpClient();
        }

        public bool HaveImages(TDto entity, out List<IFormFile> images)
        {
            var properties = entity.GetType().GetProperties();
            var imageProps = properties.Where(p => p.PropertyType == typeof(List<IFormFile>));

            if (imageProps.Any())
            {
                var firstImageProp = imageProps.First();
                var propertyValue = firstImageProp.GetValue(entity);

                images = (List<IFormFile>)propertyValue!;
                return true;
            }
            images = default!;
            return false;
        }

        public Entity SetImagePath(Entity entity, List<Image> Path)
        {
            if (Path == null)
            {
                return entity;
            }
            var properties = typeof(Entity).GetProperties().Where(p => p.PropertyType == typeof(List<Image>));
            foreach (var property in properties)
            {
                property.SetValue(entity, Path);
            }
            return entity;
        }
        public List<Image> SetImageItemProfileId(List<Image> images, Guid itemProfileId)
        {
            foreach (var image in images)
            {
                image.ItemProfileId = itemProfileId;
            }
            return images;
        }
        public async Task DeleteRangeOldImageFromAzure(Entity entity)
        {
            var oldImagePaths = new List<Image>();
            var properties = typeof(Entity).GetProperties().Where(p => p.PropertyType == typeof(List<Image>));
            foreach (var property in properties)
            {
                if (property != null)
                {
                    oldImagePaths.AddRange((List<Image>)property.GetValue(entity)!);
                }

            }
            foreach (var oldImagePath in oldImagePaths)
            {
                await DeleteImageFromAzure(oldImagePath);

            }

        }
        private async Task DeleteImageFromAzure(Image image)
        {
            if (!string.IsNullOrEmpty(image.Path))
            {
                BlobContainerClient blobContainerClient = new BlobContainerClient(_azureoptions.ConnectionString, _azureoptions.Container);
                BlobClient blobClient = blobContainerClient.GetBlobClient(Path.GetFileName(image.Path));
                await blobClient.DeleteIfExistsAsync();
            }
        }


        public async Task<List<Image>> UploadImagesToAzure(List<IFormFile> images)
        {
            var uploadedImages = new List<Image>();

            if (images == null)
            {
                return uploadedImages;
            }

            foreach (var image in images)
            {
                var fileExtension = Path.GetExtension(image.FileName);
                var uniqueName = Guid.NewGuid().ToString() + fileExtension;

                using (MemoryStream imageUploadStream = new MemoryStream())
                {
                    image.CopyTo(imageUploadStream);
                    imageUploadStream.Position = 0;

                    BlobContainerClient blobContainerClient =
                        new BlobContainerClient(_azureoptions.ConnectionString, _azureoptions.Container);
                    BlobClient blobClient = blobContainerClient.GetBlobClient(uniqueName);

                    var repit = 0;
                    var IsSuccess = false;

                    while (!IsSuccess && repit < 3)
                    {
                        imageUploadStream.Position = 0;

                        var result = await blobClient.UploadAsync(imageUploadStream, new BlobUploadOptions()
                        {
                            HttpHeaders = new BlobHttpHeaders
                            {
                                ContentType = image.ContentType,
                            }
                        }, cancellationToken: default);

                        IsSuccess = !result.GetRawResponse().IsError;
                        repit++;
                    }

                    if (IsSuccess)
                    {
                        var uploadedImage = new Image
                        {
                            Path = $"{_azureoptions.BlobURL}/{uniqueName}",
                        };

                        uploadedImages.Add(uploadedImage);
                    }
                    else
                    {
                        throw new Exception("Upload images was Failed!!!");
                    }
                }
            }

            return uploadedImages;
        }

        public async Task<IEnumerable<Stream>> LoadImagesAsStreamAsync(IEnumerable<Image>? images)
        {
            if (images == null)
                return default!;
            var tasks = images
                .Where(image => !string.IsNullOrEmpty(image.Path))
                .Select(async image =>
                {
                    if (image == null)
                        return null;

                    var response = await _httpClient.GetAsync(image.Path);
                    response.EnsureSuccessStatusCode();
                    return await response.Content.ReadAsStreamAsync();

                });

            var imageStreams = await Task.WhenAll(tasks);
            return imageStreams.Where(stream => stream != null)!;

        }
    }
}