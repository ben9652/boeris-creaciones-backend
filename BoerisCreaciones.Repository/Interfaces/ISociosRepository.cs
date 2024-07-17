using BoerisCreaciones.Core.Models;
using BoerisCreaciones.Core.Models.Socio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoerisCreaciones.Repository.Interfaces
{
    public interface ISociosRepository
    {
        public List<UsuarioVM> GetPartners();
        public UsuarioVM RegisterPartner(UsuarioVM userObj);
        public void DeletePartner(int id);
    }
}
