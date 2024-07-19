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
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;

namespace BoerisCreaciones.Service.Services
{
    public class UsuariosService : IUsuariosService
    {
        protected readonly IUsuariosRepository _repository;
        protected readonly IConfiguration _config;

        public UsuariosService(IUsuariosRepository usuariosRepository, IConfiguration config)
        {
            _repository = usuariosRepository;
            _config = config;
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

        public bool CheckPassword(int id, string password)
        {
            UsuarioVM user = _repository.GetUserById(id);
            PasswordHasher.VerifyPassword(user.password, password);
            return true;
        }

        public string GenerateToken(UsuarioDTO userObj, List<string>? additional_roles)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.SerialNumber, userObj.id_user.ToString()),
                new Claim(ClaimTypes.NameIdentifier, userObj.username),
                new Claim(ClaimTypes.Email, userObj.email),
                new Claim(ClaimTypes.GivenName, userObj.firstName),
                new Claim(ClaimTypes.Surname, userObj.lastName != null ? userObj.lastName : "-"),
                new Claim(ClaimTypes.Role, userObj.role.ToString())
            };

            if (additional_roles != null && additional_roles.Count != 0)
                claims.AddRange(additional_roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public void RegisterUser(UsuarioRegistro user)
        {
            user.password = PasswordHasher.HashPassword(user.password);

            string nombre = user.nombre + "," + (user.apellido == "" || user.apellido == null ? "-" : user.apellido);
            UsuarioVM userDB = new(
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

        public void UpdateUser(UsuarioVM userObj, List<string> attributesToChange)
        {
            if (attributesToChange.Find(attr => attr == "password") != null)
                userObj.password = PasswordHasher.HashPassword(userObj.password);

            _repository.UpdateUser(userObj, attributesToChange);
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
