using AutoMapper;
using DataAccessAPI.Dtos;
using DataAccessAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessAPI.Helpers
{
    public class AutomapperProfiles: Profile
    {
        public AutomapperProfiles()
        {
            CreateMap<Item, ItemDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.ItemCategory.CategoryName));

            CreateMap<CreateItemDto, Item>();

            CreateMap<UpdateItemDto, Item>();

            CreateMap<CreateCategoryDto, ItemCategory>();

            CreateMap<UpdateCategoryDto, ItemCategory>();
        }
    }
}
