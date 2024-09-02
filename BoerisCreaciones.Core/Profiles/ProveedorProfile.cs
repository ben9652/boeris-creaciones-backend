using AutoMapper;
using BoerisCreaciones.Core.Models.Proveedores;
using BoerisCreaciones.Core.Models.Rubros;

namespace BoerisCreaciones.Core.Profiles
{
    public class ProveedorProfileVMtoDTO : Profile
    {
        public ProveedorProfileVMtoDTO()
        {
            CreateMap<ProveedorVM, ProveedorDTO>()
                .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.id))
                .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.nombre))
                .ForMember(dest => dest.category, opt => opt.MapFrom(src => new RubroMateriaPrimaDTO(src.id_rubro, src.rubroAsociado)))
                .ForMember(dest => dest.residence, opt => opt.MapFrom(src => src.domicilio))
                .ForMember(dest => dest.phone, opt => opt.MapFrom(src => src.telefono))
                .ForMember(dest => dest.cvu_or_alias, opt => opt.MapFrom(src => src.cvu.Length != 0 ? src.cvu : src.alias))
                ;
        }
    }

    public class ProveedorProfileDTOtoVM : Profile
    {
        public ProveedorProfileDTOtoVM()
        {
            CreateMap<ProveedorDTO, ProveedorVM>()
                .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.id))
                .ForMember(dest => dest.nombre, opt => opt.MapFrom(src => src.name))
                .ForMember(dest => dest.id_rubro, opt => opt.MapFrom(src => src.category.id))
                .ForMember(dest => dest.rubroAsociado, opt => opt.MapFrom(src => src.category.name))
                .ForMember(dest => dest.domicilio, opt => opt.MapFrom(src => src.residence))
                .ForMember(dest => dest.telefono, opt => opt.MapFrom(src => src.phone))
                .ForMember(dest => dest.cvu, opt => opt.MapFrom(src => IsDigitsOnly(src.cvu_or_alias) ? src.cvu_or_alias : null))
                .ForMember(dest => dest.alias, opt => opt.MapFrom(src => !IsDigitsOnly(src.cvu_or_alias) ? src.cvu_or_alias : null))
                ;
        }

        private bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }
            return true;
        }
    }
}
