using ApplicationCore.Domain.Entity.Filters;
using System.Reflection;
using System.Threading.Tasks;
using System.Linq;

namespace Applications.Services
{
    public class FiltersService<TDto> : IFiltersService<TDto>
        where TDto : class
    {
        public Task<Filters<TDto>> AddFilters(Filters<TDto> domain)
        {
            if (domain.SortDirection == null)
            {
                domain.entity = domain.entity.Reverse<TDto>().ToList();
                return Task.FromResult(Pager(domain));
            }
            PropertyInfo[] properties = typeof(TDto).GetProperties();


            domain = Search(domain);
            domain = SearchBetween(domain, properties);

            if (domain.SortDirection == "ascending")
            {
                domain.entity = domain.entity.OrderBy(x => properties[1].GetValue(x)).ToList();
            }
            else
            {
                domain.SortDirection = "descending";
                domain.entity = domain.entity.OrderByDescending(x => properties[1].GetValue(x)).ToList();
            }

            return Task.FromResult(Pager(Search(domain)));
        }

        private Filters<TDto> Pager(Filters<TDto> domain)
        {
            if(domain.CurrentPage <= 0)
            {
                return new Filters<TDto>();
            }
            int reSkip = (domain.CurrentPage - 1) * domain.perPage;

            domain.GetTotalPages(domain.entity, domain.perPage);

            domain.StartPage = domain.CurrentPage - 5;
            domain.LastPage = domain.CurrentPage + 4;

            if (domain.StartPage <= 0)
            {
                domain.LastPage = domain.LastPage - (domain.StartPage - 1);
                domain.StartPage = 1;
            }
            if (domain.LastPage > domain.TotalPages)
            {
                domain.LastPage = domain.TotalPages;
                if (domain.LastPage > 10)
                {
                    domain.StartPage = domain.LastPage - 9;
                }
            }
            domain.entity = domain.entity.Skip(reSkip).Take(domain.perPage).ToList();

            return domain;
        }
        private Filters<TDto> Search(Filters<TDto> domain)
        {
            if (domain.SearchBy == null)
            {
                return domain;
            }

            var filteredEntities = domain.entity.Where(e =>
            {
                var properties = typeof(TDto).GetProperties();
                foreach (var property in properties)
                {
                    if (property.PropertyType == typeof(string))
                    {
                        var propertyValue = property.GetValue(e)?.ToString();
                        if (propertyValue != null && propertyValue.Contains(domain.SearchBy, StringComparison.OrdinalIgnoreCase))
                        {
                            return true;
                        }
                    }
                }
                return false;
            }).ToList();

            domain.entity = filteredEntities;
            return domain;
        }

        private Filters<TDto> SearchBetween(Filters<TDto> domain, PropertyInfo[] properties)
        {
            if ((domain.ToStartSearch == null && domain.ToEndSearch == null) || !properties.Any(p => p.PropertyType == typeof(double)))
            {
                return domain;
            }

            var filteredEntities = domain.entity.Where(entity =>
            {
                foreach (var property in properties.Where(p => p.PropertyType == typeof(double)))
                {
                    var propertyValue = property.GetValue(entity);
                    if (propertyValue == null) continue;

                    double value = (double)propertyValue;
                    bool startCondition = domain.ToStartSearch == null || value >= domain.ToStartSearch;
                    bool endCondition = domain.ToEndSearch == null || value <= domain.ToEndSearch;

                    if (startCondition && endCondition)
                    {
                        return true;
                    }
                }
                return false;
            }).ToList();

            domain.entity = filteredEntities;
            return domain;
        }



    }
}
