using BoerisCreaciones.Core.Models;

namespace BoerisCreaciones.Service.Interfaces
{
    public interface IUsuariosService
    {
        public UsuarioVM Authenticate(UsuarioLogin userObj);
        public void RegisterUser(UsuarioRegistro userObj);
    }
}
