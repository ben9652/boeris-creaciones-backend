﻿using BoerisCreaciones.Core.Models;
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
                PasswordHasher.VerifyPassword(user.password, userObj.password);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
            return user;
        }

        public void RegisterUser(UsuarioRegistro user)
        {
            try
            {
                user.password = PasswordHasher.HashPassword(user.password);
                _repository.RegisterUser(user);
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
