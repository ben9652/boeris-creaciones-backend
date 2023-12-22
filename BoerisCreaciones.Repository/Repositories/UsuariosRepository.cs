using BoerisCreaciones.Core;
using BoerisCreaciones.Core.Models;
using BoerisCreaciones.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoerisCreaciones.Repository.Repositories
{
    public class UsuariosRepository : Repository<Usuario>, IUsuariosRepository
    {
        private readonly ApplicationConfig _applicationConfig;
        private readonly BoerisCreacionesContext ctx;

        public UsuariosRepository(ApplicationConfig applicationConfig, BoerisCreacionesContext context) : base(context)
        {
            _applicationConfig = applicationConfig;
            ctx = context;
        }
    }
}
