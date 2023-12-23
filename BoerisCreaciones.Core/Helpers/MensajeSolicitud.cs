namespace BoerisCreaciones.Core.Models
{
    public class MensajeSolicitud
    {
        public MensajeSolicitud(dynamic mensaje, bool error)
        {
            this.mensaje = mensaje;
            this.error = error;
        }
        public dynamic mensaje { get; set; }
        public bool error { get; set; }
    }
}
