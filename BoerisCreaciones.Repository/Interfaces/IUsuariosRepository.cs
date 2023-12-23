using BoerisCreaciones.Core.Models;

namespace BoerisCreaciones.Repository.Interfaces
{
    public interface IUsuariosRepository
    {
        public Usuario Authenticate(Usuario userObj);
        public void UserRegister(Usuario userObj);
    }
}
