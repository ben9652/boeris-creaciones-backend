using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoerisCreaciones.Core.Models
{
    public class Usuario
    {
        public Usuario()
        {

        }
        public Usuario(int id_usuario, string username, string apellidos, string nombres, string password, string rol)
        {
            this.id_usuario = id_usuario;
            this.username = username;
            this.apellidos = apellidos;
            this.nombres = nombres;
            this.password = password;
            this.rol = rol;
        }
        [Key]
        public int id_usuario { get; set; }

        [Required]
        [MaxLength(100)]
        public string username { get; set; }

        [Required]
        [MaxLength(100)]
        public string password { get; set; }

        [Required]
        [MaxLength(50)]
        public string apellidos { get; set; }

        [Required]
        [MaxLength(50)]
        public string nombres { get; set; }

        [Required]
        [MaxLength(50)]
        public string email { get; set; }

        [Required]
        [MaxLength(50)]
        public string rol { get; set; }
    }
}
