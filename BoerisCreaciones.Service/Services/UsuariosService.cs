using BoerisCreaciones.Core.Models;
using BoerisCreaciones.Repository.Interfaces;
using BoerisCreaciones.Service.Helpers;
using BoerisCreaciones.Service.Interfaces;
using System.Net.Mail;
using System.Net;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using BoerisCreaciones.Core.Helpers;
using AutoMapper;
using BoerisCreaciones.Core.Profiles;
using System.Reflection;

namespace BoerisCreaciones.Service.Services
{
    public class UsuariosService : IUsuariosService
    {
        protected readonly IUsuariosRepository _repository;
        protected readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public UsuariosService(IUsuariosRepository usuariosRepository, IConfiguration config, IMapper mapper)
        {
            _repository = usuariosRepository;
            _config = config;
            _mapper = mapper;
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

        public void UpdateUser(int id, UsuarioDTOComplete user, List<PatchUpdate> attributesToChange)
        {
            UsuarioVM userVM = _mapper.Map<UsuarioVM>(user);
            Type vmType = userVM.GetType();
            PropertyInfo[] propiedades = vmType.GetProperties();
            List<string> actualAttributesString = new();
            foreach (PropertyInfo prop in propiedades)
                actualAttributesString.Add(prop.Name);
            List<PatchUpdate> actualAttributes = new List<PatchUpdate>();
            for(int i = 0; i < attributesToChange.Count; i++)
            {
                actualAttributes.Add(
                    new PatchUpdate(
                        GetVMAttribute(attributesToChange[i].path),
                        attributesToChange[i].value
                    )
                );
            }

            PatchUpdate? password = attributesToChange.Find(attr => attr.path == "password");
            if (password != null)
                password.value = PasswordHasher.HashPassword(password.value);

            _repository.UpdateUser(id, attributesToChange);
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

        private static string GetVMAttribute(string name)
        {
            switch (name)
            {
                case "id_user":
                    return "id_usuario";
                case "name":
                    return "nombre";
                default:
                    return "Invalid";
            }
        }

        private static Dictionary<List<string>, List<string>> GetVMAttributes(List<string> attrsDTO)
        {
            List<string> attrsVM = new();

            bool nameAdded = false;
            foreach(string attr in attrsDTO)
            {
                if(!nameAdded && (attr == "lastName" || attr == "firstName"))
                {
                    attrsVM.Add("name");
                    nameAdded = true;
                }


            }

            List<string> name = new();
            string? firstName = attrsDTO.Find(attr => attr == "firstName");
            string? lastName = attrsDTO.Find(attr => attr == "lastName");
            if(firstName != null) name.Add(firstName);
            if(lastName != null) name.Add(lastName);

            List<string> id_usuario = new List<string> { "id_usuario" };
            List<string> nombre = new List<string> { "nombre" };
            List<string> email = new List<string> { "email" };
            List<string> username = new List<string> { "username" };
            List<string> password = new List<string> { "password" };
            List<string> rol = new List<string> { "rol" };
            List<string> estado = new List<string> { "estado" };
            List<string> domicilio = new List<string> { "domicilio" };
            List<string> telefono = new List<string> { "telefono" };
            List<string> observaciones = new List<string> { "observaciones" };

            List<string> id_user = new List<string> { "id_user" };
            List<string> name = new List<string> { "lastName", "firstName" };
            List<string> role = new List<string> { "role" };
            List<string> state = new List<string> { "state" };
            List<string> domicile = new List<string> { "domicile" };
            List<string> phone = new List<string> { "phone" };
            List<string> observations = new List<string> { "observations" };

            Dictionary<List<string>, List<string>> dictionary = new Dictionary<List<string>, List<string>>();

            return dictionary;
        }
    }
}
