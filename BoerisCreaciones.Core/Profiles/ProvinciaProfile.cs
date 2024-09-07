using AutoMapper;
using BoerisCreaciones.Core.Models.Provincias;

namespace BoerisCreaciones.Core.Profiles
{
    public class ProvinciaVMtoDTOProfile : Profile
    {
        public ProvinciaVMtoDTOProfile()
        {
            CreateMap<ProvinciaVM, ProvinciaDTO>()
                .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.id_provincia))
                .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.nombre))
                ;
        }
    }

    public class ProvinciaVMtoExpandedDTOProfile : Profile
    {
        public ProvinciaVMtoExpandedDTOProfile()
        {
            CreateMap<ProvinciaVM, ProvinciaExpandedDTO>()
                .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.id_provincia))
                .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.nombre))
                ;
        }
    }

    public class ProvinciaDTOtoVMProfile : Profile
    {
        public ProvinciaDTOtoVMProfile()
        {
            CreateMap<ProvinciaDTO, ProvinciaVM>()
                .ForMember(dest => dest.id_provincia, opt => opt.MapFrom(src => src.id))
                .ForMember(dest => dest.nombre, opt => opt.MapFrom(src => src.name))
                ;
        }
    }
}
