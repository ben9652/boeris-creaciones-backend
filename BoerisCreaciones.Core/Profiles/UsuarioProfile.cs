using AutoMapper;
using BoerisCreaciones.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoerisCreaciones.Core.Profiles
{
    public class UsuarioProfile : Profile
    {
        public UsuarioProfile()
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
}
