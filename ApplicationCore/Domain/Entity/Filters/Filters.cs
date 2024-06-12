using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Domain.Entity.Filters
{
    
    public class Filters<TDto>:FiltersOption where TDto : class
    {
        public IEnumerable<TDto>? entity { get; set; }
        public int? StartPage { get; set; } = 1;
        public int? LastPage { get; set; } 

        public void AddFilterOption(FiltersOption model, IEnumerable<TDto> normalentity)
        {

            entity = normalentity;
            SortDirection = model.SortDirection;
            perPage = model.perPage;
            ColumnName = model.ColumnName;
            CurrentPage = model.CurrentPage;
            SearchBy = model.SearchBy;
            ToEndSearch = model.ToEndSearch;
            ToStartSearch = model.ToStartSearch;
        }
        
        public int TotalPages => GetTotalPages(entity, perPage);

        public int GetTotalPages(IEnumerable<TDto>? entity, int perPage)
        {
            if (entity == null || entity.Count() == null )
            {
                return 0;
            }

            return (int)Math.Ceiling(entity.Count() / (decimal)perPage);
        }
    }
}
