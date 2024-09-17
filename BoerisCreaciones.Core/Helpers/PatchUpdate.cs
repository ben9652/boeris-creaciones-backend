namespace BoerisCreaciones.Core.Helpers
{
    public class PatchUpdate
    {
        public PatchUpdate(string path, dynamic value)
        {
            this.path = path;
            this.value = value;
        }

        public string path { get; set; }
        public dynamic value { get; set; }
    }
}
