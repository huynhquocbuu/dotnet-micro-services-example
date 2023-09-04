using AutoMapper;
using Infrastructure.Extensions;
using Shared.DTOs.Product;

namespace Product.API;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Entities.Product, ProductDto>();
        CreateMap<CreateProductDto, Entities.Product>();
        CreateMap<UpdateProductDto, Entities.Product>()
            .IgnoreAllNonExisting();
    }
}