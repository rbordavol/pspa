using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using WebAPI.Models;
using WebAPI.Dtos;

namespace WebAPI.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<City, CityDto>().ReverseMap();
            CreateMap<City, CityUpdateDto>().ReverseMap();

            CreateMap<Property, PropertyListDto>()
                .ForMember(d => d.City, opt=> opt.MapFrom(src =>src.City!.CityName))
                .ForMember(d => d.Country, opt=> opt.MapFrom(src =>src.City!.Country))
                .ForMember(d => d.FurnishingType, opt=> opt.MapFrom(src =>src.FurnishingType!.FurnishingTypeName))
                .ForMember(d => d.PropertyType, opt=> opt.MapFrom(src =>src.PropertyType!.PropertyTypeName))
                .ForMember(d => d.Photo, opt=> opt.MapFrom(src => src.Photos!
                                                    .FirstOrDefault(p=> p.IsPrimary)!.PresignedUrl));
                
            CreateMap<Property, PropertyDetailDto>()
                .ForMember(d => d.City, opt=> opt.MapFrom(src =>src.City!.CityName))
                .ForMember(d => d.Country, opt=> opt.MapFrom(src =>src.City!.Country))
                .ForMember(d => d.FurnishingType, opt=> opt.MapFrom(src =>src.FurnishingType!.FurnishingTypeName))
                .ForMember(d => d.PropertyType, opt=> opt.MapFrom(src =>src.PropertyType!.PropertyTypeName));
            
            CreateMap<PropertyType, PropertyTypeDto>().ReverseMap();   
            CreateMap<FurnishingType, FurnishingTypeDto>().ReverseMap();   
            CreateMap<Property, PropertyDto>().ReverseMap();     
            CreateMap<Photo, PhotoDto>().ReverseMap();   
        }
    }
}