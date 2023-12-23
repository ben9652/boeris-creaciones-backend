using BoerisCreaciones.Core.Models;
using BoerisCreaciones.Repository.Interfaces;
using BoerisCreaciones.Service.Excepciones;
using BoerisCreaciones.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BoerisCreaciones.Service.Services
{
    public class UsuariosService : IUsuariosService
    {
        private readonly IUsuariosRepository _repository;

        public UsuariosService(IUsuariosRepository usuariosRepository)
        {
            _repository = usuariosRepository;
        }

        public Usuario Authenticate(Usuario userObj)
        {
            Usuario user;

            try
            {
                user = _repository.Authenticate(userObj);
                VerifyPassword(user.password, userObj.password);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return user;
        }

        private static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = sha256.ComputeHash(bytes);

                StringBuilder builder = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private static void VerifyPassword(string hashedPasswordFromDatabase, string providedPassword)
        {
            string hashedProvidedPassword = HashPassword(providedPassword);
            if (hashedPasswordFromDatabase != hashedProvidedPassword)
                throw new InvalidPasswordException("Contraseña incorrecta");
        }

        public void RegisterUser(Usuario user)
        {
            try
            {
                user.password = HashPassword(user.password);
                _repository.UserRegister(user);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
