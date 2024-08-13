using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Domain.Entity;
using ApplicationCore.Domain.Entity.Image;
using Microsoft.AspNetCore.Http;

namespace ApplicationInfrastructure.Contracts
{
    public interface IImageManagerService<Entity, EntityDto>
    where Entity : Entity<Guid>
    where EntityDto : class
    {
        public Entity SetImagesPathToEntity(Entity entity, List<Image> Path);
        public List<Image> SetImageItemProfileId(List<Image> images, Guid itemProfileId);
        public bool TryGetImages(EntityDto entity, out List<IFormFile> images);
        public Task<Entity> HandleImageUpdateAsync(Entity entity, EntityDto dto, Guid id);
        public Task<Entity> HandleImageUploadAsync(Entity entity, EntityDto entityDto);
    }
}