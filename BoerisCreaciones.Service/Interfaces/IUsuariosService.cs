using BoerisCreaciones.Core.Models;

namespace BoerisCreaciones.Service.Interfaces
{
    public interface IUsuariosService
    {
        public Usuario Authenticate(Usuario userObj);
        public void RegisterUser(UsuarioRegistro userObj);
    }
}
