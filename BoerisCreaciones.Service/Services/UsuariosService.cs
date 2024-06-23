using BoerisCreaciones.Core.Models;
using BoerisCreaciones.Repository.Interfaces;
using BoerisCreaciones.Service.Excepciones;
using BoerisCreaciones.Service.Helpers;
using BoerisCreaciones.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Diagnostics;

namespace BoerisCreaciones.Service.Services
{
    public class UsuariosService : IUsuariosService
    {
        private readonly IUsuariosRepository _repository;

        public UsuariosService(IUsuariosRepository usuariosRepository)
        {
            _repository = usuariosRepository;
        }

        public UsuarioVM GetUserById(int id)
        {
            UsuarioVM userDatabase;

            userDatabase = _repository.GetUserById(id);

            return userDatabase;
        }

        public UsuarioDTO Authenticate(UsuarioLogin userObj)
        {
            UsuarioVM userDatabase;

            string nombre;
            string apellido;

            Console.WriteLine("Ejecución de Authenticate()");
            userDatabase = _repository.Authenticate(userObj);

            PasswordHasher.VerifyPassword(userDatabase.password, userObj.password);
            userDatabase.password = null;

            string[] nombres_apellidos = userDatabase.nombre.Split(',');
            nombre = nombres_apellidos[0];
            apellido = nombres_apellidos[1];

            UsuarioDTO user;
            user = new UsuarioDTO(
                userDatabase.id_usuario,
                userDatabase.username,
                apellido != "-" ? apellido : null,
                nombre,
                userDatabase.email,
                userDatabase.rol
            );
            
            return user;
        }

        public void RegisterUser(UsuarioRegistro user)
        {
            user.password = PasswordHasher.HashPassword(user.password);

            string nombre = user.nombre + "," + (user.apellido == "" || user.apellido == null ? "-" : user.apellido);
            UsuarioVM userDB = new UsuarioVM(
                0,
                nombre,
                user.email,
                user.username,
                user.password,
                new DateTime(),
                user.rol,
                '0',
                user.domicilio,
                user.telefono,
                user.observaciones
            );

            _repository.RegisterUser(userDB);
        }

        public void UpdateUser(UsuarioVM userObj, bool passwordUpdated)
        {
            if(passwordUpdated)
                userObj.password = PasswordHasher.HashPassword(userObj.password);

            _repository.UpdateUser(userObj);
        }

        public void DeleteUser(int id)
        {
            _repository.DeleteUser(id);
        }

        private void SendMail(string fromMail, string fromPassword, string destinationMail, string subject, string body)
        {
            MailMessage message = new MailMessage();
            message.From = new MailAddress(fromMail);
            message.Subject = "Asunto de testeo";
            message.To.Add(new MailAddress(destinationMail));
            message.Body = "<html><body>" + body + "</body></html>";
            message.IsBodyHtml = true;

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromMail, fromPassword),
                EnableSsl = true
            };
        }
    }
}
