using BoerisCreaciones.Core.Models;

namespace BoerisCreaciones.Service.Interfaces
{
    public interface IUsuariosService
    {
        public Usuario Authenticate(UsuarioLogin userObj);
        public void RegisterUser(UsuarioRegistro userObj);
    }
}
