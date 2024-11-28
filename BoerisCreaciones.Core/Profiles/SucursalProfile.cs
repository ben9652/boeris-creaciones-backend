using AutoMapper;
using BoerisCreaciones.Core.Models.Localidades;
using BoerisCreaciones.Core.Models.Sucursales;

namespace BoerisCreaciones.Core.Profiles
{
    public class SucursalVMtoDTOProfile : Profile
    {
        public SucursalVMtoDTOProfile()
        {
            CreateMap<SucursalVM, SucursalDTO>()
                .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.id_sucursal))
                .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.nombre))
                .ForMember(dest => dest.domicile, opt => opt.MapFrom(src => src.domicilio))
                .ForMember(dest => dest.locality, opt => opt.MapFrom(src => new LocalidadDTO(src.id_localidad, src.localidad, null)))
                ;
        }
    }

    public class SucursalVMtoDTOBaseProfile : Profile
    {
        public SucursalVMtoDTOBaseProfile()
        {
            CreateMap<SucursalVM, SucursalDTOBase>()
                .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.id_sucursal))
                .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.nombre))
                .ForMember(dest => dest.domicile, opt => opt.MapFrom(src => src.domicilio))
                ;
        }
    }

    public class SucursalVMtoExpandedDTOProfile : Profile
    {
        public SucursalVMtoExpandedDTOProfile()
        {
            CreateMap<SucursalVM, SucursalExpandedDTO>()
                .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.id_sucursal))
                .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.nombre))
                .ForMember(dest => dest.domicile, opt => opt.MapFrom(src => src.domicilio))
                ;
        }
    }

    public class SucursalDTOtoVMProfile : Profile
    {
        public SucursalDTOtoVMProfile()
        {
            CreateMap<SucursalDTO, SucursalVM>()
                .ForMember(dest => dest.id_sucursal, opt => opt.MapFrom(src => src.id))
                .ForMember(dest => dest.nombre, opt => opt.MapFrom(src => src.name))
                .ForMember(dest => dest.domicilio, opt => opt.MapFrom(src => src.domicile))
                .ForMember(dest => dest.id_localidad, opt => opt.MapFrom(src => src.locality.id))
                .ForMember(dest => dest.localidad, opt => opt.MapFrom(src => src.locality.name))
                ;
        }
    }
}
