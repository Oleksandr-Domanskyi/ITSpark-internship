using ApplicationCore.Domain.Entity;
using ApplicationCore.Domain.Entity.ItemProfile;
using Applications.CQRS.Command.Create;
using Applications.CQRS.Command.Update;
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
    public class ApplicationMapperProfile:Profile
    {
        public ApplicationMapperProfile()
        {
            CreateMap<ItemProfile,ItemProfileDto>().ReverseMap();

            CreateMap<CreateCommand<ItemProfile, ItemProfileRequest>, ItemProfile>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.request.Name))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.request.Price))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.request.Description))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.request.Category.ToString()))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.request.CreatedBy));
                
            //.ForMember(dest => dest.images, opt => opt.Ignore());

            CreateMap<UpdateCommand<ItemProfile, ItemProfileRequest>, ItemProfile>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.request.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.request.Name))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.request.Price))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.request.Description))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.request.Category.ToString()))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.request.CreatedBy));
        }

    }
}
