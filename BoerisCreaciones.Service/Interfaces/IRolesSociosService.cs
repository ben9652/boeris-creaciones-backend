using BoerisCreaciones.Core.Models.Socio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoerisCreaciones.Service.Interfaces
{
    public interface IRolesSociosService
    {
        public List<TipoSocioDTO> GetPartnerRolesCompleteNames(int id);
        public List<string> GetPartnerRoles(int id);
        public List<TipoSocioDTO> GetPossibleRoles();
        public void AssignRoles(int id, List<int> roles);
        public void UpdateRoles(int id, List<int> roles);
        public void DeleteRoles(int id);
    }
}
