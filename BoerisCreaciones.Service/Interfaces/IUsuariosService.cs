using BoerisCreaciones.Core.Models;

namespace BoerisCreaciones.Service.Interfaces
{
    public interface IUsuariosService
    {
        public UsuarioVM GetUserById(int id);
        public UsuarioDTO Authenticate(UsuarioLogin userObj);
        public void RegisterUser(UsuarioRegistro userObj);
        public void UpdateUser(UsuarioVM userObj, bool passwordUpdated);
    }
}
