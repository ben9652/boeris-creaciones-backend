using System.ComponentModel.DataAnnotations;

namespace BoerisCreaciones.Core.Models
{
    public class Usuario
    {
        public Usuario()
        {

        }
        public Usuario(int id_usuario, string username, string apellidos, string nombres, string email, string password, string rol)
        {
            this.id_usuario = id_usuario;
            this.username = username;
            this.apellidos = apellidos;
            this.nombres = nombres;
            this.email = email;
            this.password = password;
            this.rol = rol;
        }

        [Key]
        public int id_usuario { get; set; }

        [Required]
        [MaxLength(100)]
        public string username { get; set; }

        [MaxLength(100)]
        public string? password { get; set; }

        [Required]
        [MaxLength(50)]
        public string apellidos { get; set; }

        [Required]
        [MaxLength(50)]
        public string nombres { get; set; }

        [Required]
        [MaxLength(100)]
        public string email { get; set; }

        [Required]
        [MaxLength(50)]
        public string rol { get; set; }
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

        [Required]
        [MaxLength(100)]
        public string username { get; set; }

        [Required]
        [MaxLength(100)]
        public string password { get; set; }
    }

    public class UsuarioRegistro
    {
        public UsuarioRegistro()
        {

        }

        public UsuarioRegistro(string username, string apellidos, string nombres, string email, string password)
        {
            this.username = username;
            this.apellidos = apellidos;
            this.nombres = nombres;
            this.email = email;
            this.password = password;
        }

        [Required]
        [MaxLength(100)]
        public string username { get; set; }

        [Required]
        [MaxLength(50)]
        public string apellidos { get; set; }

        [Required]
        [MaxLength(50)]
        public string nombres { get; set; }

        [Required]
        [MaxLength(100)]
        public string email { get; set; }

        [Required]
        [MaxLength(100)]
        public string password { get; set; }
    }
}
