namespace BoerisCreaciones.Core.Models.Socio
{
    public class SocioDTO : UsuarioDTO
    {
        public SocioDTO()
        {
            partnerRoles = new List<TipoSocioDTO>();
        }

        public SocioDTO(int id_user, string username, string? lastName, string firstName, string email, char role)
            : base(id_user, username, lastName, firstName, email, role)
        {
            partnerRoles = new List<TipoSocioDTO>();
        }

        public List<TipoSocioDTO> partnerRoles;
    }

    public class SocioRegistro
    {
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string email { get; set; }
    }

    public class TipoSocioVM
    {
        public TipoSocioVM(int id_tipoSocio, char tipoSocio, string explicacion_rol)
        {
            this.id_tipoSocio = id_tipoSocio;
            this.tipoSocio = tipoSocio;
            this.explicacion_rol = explicacion_rol;
        }

        public int id_tipoSocio { get; set; }
        public char tipoSocio { get; set; }
        public string explicacion_rol { get; set; }
    }

    public class TipoSocioDTO
    {
        public TipoSocioDTO()
        {

        }
        public TipoSocioDTO(int id, string role, string explanation_role)
        {
            this.id = id;
            this.role = role;
            this.explanation_role = explanation_role;
        }

        public int id { get; set; }
        public string role { get; set; }
        public string explanation_role { get; set; }
    }

    public class RolSocioNuevo
    {
        public RolSocioNuevo(int id_usuario, int id_tipoSocio)
        {
            this.id_usuario = id_usuario;
            this.id_tipoSocio = id_tipoSocio;
        }

        public int id_usuario { get; set; }
        public int id_tipoSocio { get; set; }
    }
}
