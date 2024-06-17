using System.ComponentModel.DataAnnotations;

namespace BoerisCreaciones.Core.Models
{
    public class UsuarioVM
    {
        public UsuarioVM()
        {

        }

        public UsuarioVM(
            int id_usuario,
            string nombre,
            string email,
            string username,
            string password,
            DateTime fecha_alta,
            char rol,
            char estado,
            string? domicilio,
            UInt64? telefono,
            string? observaciones
            )
        {
            this.id_usuario = id_usuario;
            this.nombre = nombre;
            this.email = email;
            this.username = username;
            this.password = password;
            this.fecha_alta = fecha_alta;
            this.rol = rol;
            this.estado = estado;
            this.domicilio = domicilio;
            this.telefono = telefono;
            this.observaciones = observaciones;
        }

        public int id_usuario { get; set; }
        public string nombre { get; set; }
        public string email { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public DateTime fecha_alta { get; set; }
        public char rol { get; set; }
        public char estado { get; set; }
        public string? domicilio { get; set; }
        public UInt64? telefono { get; set; }
        public string? observaciones { get; set; }
    }

    public class UsuarioDTO
    {
        public UsuarioDTO()
        {

        }

        public UsuarioDTO(int id_user, string username, string lastName, string firstName, string email, string role)
        {
            this.id_user = id_user;
            this.username = username;
            this.lastName = lastName;
            this.firstName = firstName;
            this.email = email;
            this.role = role;
        }

        public int id_user { get; set; }
        public string username { get; set; }
        public string lastName { get; set; }
        public string firstName { get; set; }
        public string email { get; set; }
        public string role { get; set; }
    }

    public class UsuarioLogin
    {
        public UsuarioLogin()
        {

        }

        public UsuarioLogin(string username, string password)
        {
            this.username = username;
            this.password = password;
        }

        public string username { get; set; }
        public string password { get; set; }
    }
}

public class UsuarioRegistro
{
    public UsuarioRegistro(
        string nombre,
        string email,
        string username,
        string password,
        char rol,
        string? domicilio,
        UInt64? telefono,
        string? observaciones
        )
    {
        this.nombre = nombre;
        this.email = email;
        this.username = username;
        this.password = password;
        this.rol = rol;
        this.domicilio = domicilio;
        this.telefono = telefono;
        this.observaciones = observaciones;
    }

    public string nombre { get; set; }
    public string email { get; set; }
    public string username { get; set; }
    public string password { get; set; }
    public char rol { get; set; }
    public string? domicilio { get; set; }
    public UInt64? telefono { get; set; }
    public string? observaciones { get; set; }
}
