using AutoMapper;
using BoerisCreaciones.Core.Models.Rubros;

namespace BoerisCreaciones.Core.Profiles
{
    public class RubrosVMtoDTOProfile : Profile
    {
        public RubrosVMtoDTOProfile()
        {
            CreateMap<RubroMateriaPrimaVM, RubroMateriaPrimaDTO>()
                .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.id_rubroMP))
                .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.nombre))
                ;
        }
    }

    public class RubrosDTOtoVMProfile : Profile
    {
        public RubrosDTOtoVMProfile()
        {
            CreateMap<RubroMateriaPrimaDTO, RubroMateriaPrimaVM>()
                .ForMember(dest => dest.id_rubroMP, opt => opt.MapFrom(src => src.id))
                .ForMember(dest => dest.nombre, opt => opt.MapFrom(src => src.name))
                ;
        }
    }
}
