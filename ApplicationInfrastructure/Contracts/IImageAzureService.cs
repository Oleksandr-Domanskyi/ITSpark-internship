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
        public Task<List<Image>> UploadImagesToAzure(List<IFormFile> images);
        public bool HaveImages(TDto entity, out List<IFormFile> images);
        public Entity SetImagePath(Entity entity, List<Image> Path);

        public Task DeleteRangeOldImageFromAzure(Entity entity);

        public List<Image> SetImageItemProfileId(List<Image> images, Guid itemProfileId);
    }
}