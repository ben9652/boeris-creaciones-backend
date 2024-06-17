using BoerisCreaciones.Core.Models;

namespace BoerisCreaciones.Repository.Interfaces
{
    public interface IUsuariosRepository
    {
        public UsuarioVM Authenticate(UsuarioLogin userObj);
        public void RegisterUser(UsuarioRegistro userObj);
    }
}
