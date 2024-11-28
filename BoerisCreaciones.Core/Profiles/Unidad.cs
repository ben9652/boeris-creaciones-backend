using AutoMapper;
using BoerisCreaciones.Core.Models.Unidades;

namespace BoerisCreaciones.Core.Profiles
{
    public class Unidad : Profile
    {
        public Unidad()
        {
            CreateMap<UnidadVM, UnidadDTO>()
                .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.id_unidad))
                .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.nombre))
                ;
        }
    }

    public class UnidadDTOtoVMProfile : Profile
    {
        public UnidadDTOtoVMProfile()
        {
            CreateMap<UnidadDTO, UnidadVM>()
                .ForMember(dest => dest.id_unidad, opt => opt.MapFrom(src => src.id))
                .ForMember(dest => dest.nombre, opt => opt.MapFrom(src => src.name))
                ;
        }
    }
}
