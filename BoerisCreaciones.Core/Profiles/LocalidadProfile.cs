using AutoMapper;
using BoerisCreaciones.Core.Models.Localidades;
using BoerisCreaciones.Core.Models.Provincias;

namespace BoerisCreaciones.Core.Profiles
{
    public class LocalidadVMtoDTOProfile : Profile
    {
        public LocalidadVMtoDTOProfile()
        {
            CreateMap<LocalidadVM, LocalidadDTO>()
                .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.id_provincia))
                .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.nombre))
                .ForMember(dest => dest.province, opt => opt.MapFrom(src => new ProvinciaDTO(src.id_provincia, src.provincia)))
                ;
        }
    }

    public class LocalidadVMtoExpandedDTOProfile : Profile
    {
        public LocalidadVMtoExpandedDTOProfile()
        {
            CreateMap<LocalidadVM, LocalidadExpandedDTO>()
                .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.id_provincia))
                .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.nombre))
                ;
        }
    }

    public class LocalidadDTOtoVMProfile : Profile
    {
        public LocalidadDTOtoVMProfile()
        {
            CreateMap<LocalidadDTO, LocalidadVM>()
                .ForMember(dest => dest.id_localidad, opt => opt.MapFrom(src => src.id))
                .ForMember(dest => dest.nombre, opt => opt.MapFrom(src => src.name))
                .ForMember(dest => dest.id_provincia, opt => opt.MapFrom(src => src.province.id))
                .ForMember(dest => dest.provincia, opt => opt.MapFrom(src => src.province.name))
                ;
        }
    }
}
