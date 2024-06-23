using BoerisCreaciones.Core.Models;

namespace BoerisCreaciones.Repository.Interfaces
{
    public interface IUsuariosRepository
    {
        public UsuarioVM GetUserById(int id);
        public UsuarioVM Authenticate(UsuarioLogin userObj);
        public void RegisterUser(UsuarioVM userObj);
        public void UpdateUser(UsuarioVM userObj);
        public void DeleteUser(int id);
    }
}
