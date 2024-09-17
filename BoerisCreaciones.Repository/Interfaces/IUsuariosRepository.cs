using BoerisCreaciones.Core.Helpers;
using BoerisCreaciones.Core.Models;

namespace BoerisCreaciones.Repository.Interfaces
{
    public interface IUsuariosRepository
    {
        public UsuarioVM GetUserById(int id);
        public UsuarioVM Authenticate(UsuarioLogin userObj);
        public void RegisterUser(UsuarioVM userObj);
        public void UpdateUser(int id, List<PatchUpdate> attributesToChange);
        public void DeleteUser(int id);
    }
}
