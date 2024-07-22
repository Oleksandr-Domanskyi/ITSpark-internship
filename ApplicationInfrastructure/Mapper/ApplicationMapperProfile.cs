using ApplicationCore.Domain.Entity;
using ApplicationCore.Domain.Entity.Filters;
using ApplicationCore.Domain.Entity.ItemProfile;
using Applications.Dto;
using Applications.Dto.Request;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Mapper
{
    public class ApplicationMapperProfile : Profile
    {
        public ApplicationMapperProfile()
        {
            CreateMap<ItemProfile, ItemProfileDto>().ReverseMap();

            CreateMap<ItemProfileRequest, ItemProfile>()
            .ForMember(x => x.images, dest => dest.Ignore());

            CreateMap<Filters, FiltersOption>().ReverseMap();

        }

    }
}
