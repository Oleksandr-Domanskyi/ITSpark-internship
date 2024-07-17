using ApplicationCore.Domain.Entity.Filters;

namespace Applications.Services.FilterService
{
    public interface IFiltersService<TEntity> where TEntity : class
    {
        Task<Filters<TEntity>> SortBy(Filters<TEntity> filters);
    }
}