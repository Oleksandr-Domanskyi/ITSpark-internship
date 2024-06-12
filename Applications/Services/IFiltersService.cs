using ApplicationCore.Domain.Entity.Filters;

namespace Applications.Services
{
    public interface IFiltersService<TEntity> where TEntity : class
    {
        Task<Filters<TEntity>> AddFilters(Filters<TEntity> filters);
    }
}