using BoerisCreaciones.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace BoerisCreaciones.Repository
{
    public class BoerisCreacionesContext : DbContext
    {
        public BoerisCreacionesContext(DbContextOptions<BoerisCreacionesContext> opt) : base(opt) { }

        public DbSet<Usuario> Usuarios { get; set; }
    }
}
