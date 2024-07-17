using BoerisCreaciones.Core.Models.Socio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoerisCreaciones.Repository.Interfaces
{
    public interface IRolesSociosRepository
    {
        public List<TipoSocioVM> GetPartnerRoles(int id);
        public List<TipoSocioVM> GetPossibleRoles();
        public void AssignRoles(int id, List<int> roles);
        public void UpdateRoles(int id, List<int> roles);
        public void DeleteRoles(int id);
    }
}
