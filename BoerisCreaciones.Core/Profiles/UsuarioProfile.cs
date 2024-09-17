using AutoMapper;
using BoerisCreaciones.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoerisCreaciones.Core.Profiles
{
    public class UsuarioVMtoDTOProfile : Profile
    {
        public UsuarioVMtoDTOProfile()
        {
            CreateMap<UsuarioVM, UsuarioDTO>()
                .ForMember(dest => dest.id_user, opt => opt.MapFrom(src => src.id_usuario))
                .ForMember(dest => dest.firstName, opt => opt.MapFrom(src => SplitNombre(src.nombre).Item1))
                .ForMember(dest => dest.lastName, opt => opt.MapFrom(src => SplitNombre(src.nombre).Item2))
                .ForMember(dest => dest.email, opt => opt.MapFrom(src => src.email))
                .ForMember(dest => dest.username, opt => opt.MapFrom(src => src.username))
                .ForMember(dest => dest.role, opt => opt.MapFrom(src => src.rol));
        }

        private static (string, string?) SplitNombre(string nombre)
        {
            string[] nombresApellidos = nombre.Split(',');
            string firstName = nombresApellidos[0];
            string? lastName = nombresApellidos.Length > 1 && nombresApellidos[1] != "-" ? nombresApellidos[1] : null;
            return (firstName, lastName);
        }
    }

    public class UsuarioDTOtoVMProfile : Profile
    {
        public UsuarioDTOtoVMProfile()
        {
            CreateMap<UsuarioDTO, UsuarioVM>()
                .ForMember(dest => dest.id_usuario, opt => opt.MapFrom(src => src.id_user))
                .ForMember(dest => dest.nombre, opt => opt.MapFrom(src => src.firstName + "," + src.lastName))
                .ForMember(dest => dest.email, opt => opt.MapFrom(src => src.email))
                .ForMember(dest => dest.username, opt => opt.MapFrom(src => src.username))
                .ForMember(dest => dest.rol, opt => opt.MapFrom(src => src.role))
                ;
        }
    }

    public class UsuarioVMtoDTOCompleteProfile : Profile
    {
        public UsuarioVMtoDTOCompleteProfile()
        {
            CreateMap<UsuarioVM, UsuarioDTOComplete>()
                .ForMember(dest => dest.id_user, opt => opt.MapFrom(src => src.id_usuario))
                .ForMember(dest => dest.firstName, opt => opt.MapFrom(src => SplitNombre(src.nombre).Item1))
                .ForMember(dest => dest.lastName, opt => opt.MapFrom(src => SplitNombre(src.nombre).Item2))
                .ForMember(dest => dest.email, opt => opt.MapFrom(src => src.email))
                .ForMember(dest => dest.username, opt => opt.MapFrom(src => src.username))
                .ForMember(dest => dest.password, opt => opt.MapFrom(src => src.password))
                .ForMember(dest => dest.role, opt => opt.MapFrom(src => src.rol))
                .ForMember(dest => dest.state, opt => opt.MapFrom(src => src.estado))
                .ForMember(dest => dest.domicile, opt => opt.MapFrom(src => src.domicilio))
                .ForMember(dest => dest.phone, opt => opt.MapFrom(src => src.telefono))
                .ForMember(dest => dest.observations, opt => opt.MapFrom(src => src.observaciones))
                ;

        }

        private static (string, string?) SplitNombre(string nombre)
        {
            string[] nombresApellidos = nombre.Split(',');
            string firstName = nombresApellidos[0];
            string? lastName = nombresApellidos.Length > 1 && nombresApellidos[1] != "-" ? nombresApellidos[1] : null;
            return (firstName, lastName);
        }
    }
}
