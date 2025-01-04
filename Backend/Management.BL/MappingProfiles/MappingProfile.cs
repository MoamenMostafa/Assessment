using AutoMapper;
using Management.BL.DTOs;
using Management.DL.Models;
namespace Management.BL.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap();

            CreateMap<Category, CategoryDto>().ReverseMap();
        }
    }
}
