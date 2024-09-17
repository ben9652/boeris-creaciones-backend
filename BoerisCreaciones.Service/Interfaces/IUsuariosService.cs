using BoerisCreaciones.Core.Helpers;
using BoerisCreaciones.Core.Models;

namespace BoerisCreaciones.Service.Interfaces
{
    public interface IUsuariosService
    {
        public UsuarioVM GetUserById(int id);
        public UsuarioDTO Authenticate(UsuarioLogin userObj);
        public bool CheckPassword(int id, string password);
        public string GenerateToken(UsuarioDTO userObj, List<string>? additional_roles);
        public void RegisterUser(UsuarioRegistro userObj);
        public void UpdateUser(int id, UsuarioDTOComplete user, List<PatchUpdate> attributesToChange);
        public void DeleteUser(int id);
    }
}
