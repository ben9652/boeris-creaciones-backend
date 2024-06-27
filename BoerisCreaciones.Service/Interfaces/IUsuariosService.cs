using BoerisCreaciones.Core.Models;
using Org.BouncyCastle.Asn1.Mozilla;

namespace BoerisCreaciones.Service.Interfaces
{
    public interface IUsuariosService
    {
        public UsuarioVM GetUserById(int id);
        public UsuarioDTO Authenticate(UsuarioLogin userObj);
        public bool CheckPassword(int id, string password);
        public string GenerateToken(UsuarioDTO userObj);
        public void RegisterUser(UsuarioRegistro userObj);
        public void UpdateUser(UsuarioVM userObj, bool passwordUpdated);
        public void DeleteUser(int id);
    }
}
