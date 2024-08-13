using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Domain.Entity;
using ApplicationCore.Domain.Entity.Image;
using Microsoft.AspNetCore.Http;

namespace ApplicationInfrastructure.Services.ImageService
{
    public interface IImageAzureService<Entity, TDto>
    where Entity : Entity<Guid>
    where TDto : class
    {
        public Task<IEnumerable<Stream>> LoadImagesAsStreamAsync(IEnumerable<Image>? images);
        public Task<List<Image>> UploadImagesToAzure(List<IFormFile> images);
        public Task DeleteRangeOldImageFromAzure(Entity entity);

    }
}