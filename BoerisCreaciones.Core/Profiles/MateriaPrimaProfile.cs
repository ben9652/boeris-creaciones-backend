using AutoMapper;
using BoerisCreaciones.Core.Models.MateriasPrimas;
using BoerisCreaciones.Core.Models.Rubros;
using BoerisCreaciones.Core.Models.Unidades;

namespace BoerisCreaciones.Core.Profiles
{
    public class MateriaPrimaVMtoDTOProfile : Profile
    {
        public MateriaPrimaVMtoDTOProfile()
        {
            CreateMap<MateriaPrimaVM, MateriaPrimaDTO>()
                .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.id_matP))
                .ForMember(dest => dest.category, opt => opt.MapFrom(src => new RubroMateriaPrimaDTO(src.id_rubroMP, src.rubro)))
                .ForMember(dest => dest.unit, opt => opt.MapFrom(src => new UnidadDTO(src.id_unidad, src.unidad)))
                .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.nombre))
                .ForMember(dest => dest.source, opt => opt.MapFrom(src => src.origen))
                .ForMember(dest => dest.stock, opt => opt.MapFrom(src => src.cantidad_restante))
                .ForMember(dest => dest.picture, opt => opt.MapFrom(src => src.imagen))
                .ForMember(dest => dest.comment, opt => opt.MapFrom(src => src.comentario))
                ;
        }
    }

    public class MateriaPrimaVMtoDTOBaseProfile : Profile
    {
        public MateriaPrimaVMtoDTOBaseProfile()
        {
            CreateMap<MateriaPrimaVM, MateriaPrimaDTOBase>()
                .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.id_matP))
                .ForMember(dest => dest.unit, opt => opt.MapFrom(src => new UnidadDTO(src.id_unidad, src.unidad)))
                .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.nombre))
                .ForMember(dest => dest.source, opt => opt.MapFrom(src => src.origen))
                .ForMember(dest => dest.stock, opt => opt.MapFrom(src => src.cantidad_restante))
                .ForMember(dest => dest.picture, opt => opt.MapFrom(src => src.imagen))
                .ForMember(dest => dest.comment, opt => opt.MapFrom(src => src.comentario))
                ;
        }
    }

    public class MateriaPrimaDTOtoVMProfile : Profile
    {
        public MateriaPrimaDTOtoVMProfile()
        {
            CreateMap<MateriaPrimaDTO, MateriaPrimaVM>()
                .ForMember(dest => dest.id_matP, opt => opt.MapFrom(src => src.id))
                .ForMember(dest => dest.id_rubroMP, opt => opt.MapFrom(src => src.category.id))
                .ForMember(dest => dest.rubro, opt => opt.MapFrom(src => src.category.name))
                .ForMember(dest => dest.id_unidad, opt => opt.MapFrom(src => src.unit.id))
                .ForMember(dest => dest.unidad, opt => opt.MapFrom(src => src.unit.name))
                .ForMember(dest => dest.nombre, opt => opt.MapFrom(src => src.name))
                .ForMember(dest => dest.origen, opt => opt.MapFrom(src => src.source))
                .ForMember(dest => dest.cantidad_restante, opt => opt.MapFrom(src => src.stock))
                .ForMember(dest => dest.cantidad_descartada, opt => opt.MapFrom(src => 0))
                .ForMember(dest => dest.imagen, opt => opt.MapFrom(src => src.picture))
                .ForMember(dest => dest.comentario, opt => opt.MapFrom(src => src.comment))
                ;
        }
    }
}
