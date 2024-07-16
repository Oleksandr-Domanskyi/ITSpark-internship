using ApplicationCore.Domain.Entity.Filters;
using System.Reflection;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore.Metadata;



namespace Applications.Services.FilterService
{
    public class FiltersService<TDto> : IFiltersService<TDto>
        where TDto : class
    {

        public async Task<Filters<TDto>> SortBy(Filters<TDto> domain)
        {
            if (domain.SortDirection == null)
            {
                domain.entity = domain.entity!.Reverse().ToList();
                return domain;
            }
            PropertyInfo[] properties = typeof(TDto).GetProperties();


            if (domain.SortDirection == "ascending")
            {
                domain.entity = domain.entity!.OrderBy(x => properties[1].GetValue(x)).ToList();
            }
            else
            {
                domain.SortDirection = "descending";
                domain.entity = domain.entity!.OrderByDescending(x => properties[1].GetValue(x)).ToList();
            }
           
            return domain;;
        }






    }
}
