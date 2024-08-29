using AutoMapper;
using BoerisCreaciones.Core.Models.Productos;
using BoerisCreaciones.Core.Models.Rubros;

namespace BoerisCreaciones.Core.Profiles
{
    public class ProductoVMtoDTOProfile : Profile
    {
        public ProductoVMtoDTOProfile()
        {
            CreateMap<ProductosItemVM, ProductosItemDTO>()
                .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.id_producto))
                .ForMember(dest => dest.category, opt => opt.MapFrom(src => new RubroProductoDTO(src.id_rubroP, src.nombre)))
                .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.nombre))
                .ForMember(dest => dest.price, opt => opt.MapFrom(src => src.precio))
                .ForMember(dest => dest.stock, opt => opt.MapFrom(src => src.cantidad_restante))
                .ForMember(dest => dest.picture, opt => opt.MapFrom(src => src.imagen))
                .ForMember(dest => dest.comment, opt => opt.MapFrom(src => src.comentario))
                ;
        }
    }

    public class ProductoDTOtoVMProfile : Profile
    {
        public ProductoDTOtoVMProfile()
        {
            CreateMap<ProductosItemDTO, ProductosItemVM>()
                .ForMember(dest => dest.id_producto, opt => opt.MapFrom(src => src.id))
                .ForMember(dest => dest.id_rubroP, opt => opt.MapFrom(src => src.category.id))
                .ForMember(dest => dest.rubro, opt => opt.MapFrom(src => src.category.name))
                .ForMember(dest => dest.nombre, opt => opt.MapFrom(src => src.name))
                .ForMember(dest => dest.precio, opt => opt.MapFrom(src => src.price))
                .ForMember(dest => dest.cantidad_restante, opt => opt.MapFrom(src => src.stock))
                .ForMember(dest => dest.imagen, opt => opt.MapFrom(src => src.picture))
                .ForMember(dest => dest.comentario, opt => opt.MapFrom(src => src.comment))
                ;
        }
    }
}
