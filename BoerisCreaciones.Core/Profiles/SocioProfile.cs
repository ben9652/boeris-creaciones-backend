using AutoMapper;
using BoerisCreaciones.Core.Models;
using BoerisCreaciones.Core.Models.Socio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoerisCreaciones.Core.Profiles
{
    public class SocioProfile : Profile
    {
        public SocioProfile()
        {
            CreateMap<UsuarioVM, SocioDTO>()
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

    public class TipoSocio : Profile
    {
        public TipoSocio()
        {
            CreateMap<TipoSocioVM, TipoSocioDTO>()
                .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.id_tipoSocio))
                .ForMember(dest => dest.role, opt => opt.MapFrom(src => GetRoleName(src.tipoSocio)))
                .ForMember(dest => dest.explanation_role, opt => opt.MapFrom(src => src.explicacion_rol));
        }

        private static string GetRoleName(char role)
        {
            switch (role)
            {
                case 'a':
                    return "Administrador";
                case 's':
                    return "Surtidor";
                case 'r':
                    return "Recepcionista";
                case 'e':
                    return "Elaborador";
                case 'l':
                    return "Logístico";
                case 'v':
                    return "Vendedor";
                default:
                    return "Desconocido";
            }
        }
    }
}
