using BoerisCreaciones.Core.Models;
using BoerisCreaciones.Core.Models.Socio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BoerisCreaciones.Service.Interfaces
{
    public interface ISociosService
    {
        public List<SocioDTO> GetPartners();
        public SocioDTO GetPartner(int id);
        public SocioDTO RegisterPartner(SocioRegistro user);
        public void DeletePartner(int id);
    }
}
