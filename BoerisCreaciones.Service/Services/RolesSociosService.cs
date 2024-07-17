using AutoMapper;
using BoerisCreaciones.Core.Models.Socio;
using BoerisCreaciones.Repository.Interfaces;
using BoerisCreaciones.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoerisCreaciones.Service.Services
{
    public class RolesSociosService : IRolesSociosService
    {
        private readonly IRolesSociosRepository _repository;
        private readonly IMapper _mapper;

        public RolesSociosService(IRolesSociosRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public List<TipoSocioDTO> GetPartnerRolesCompleteNames(int id)
        {
            List<TipoSocioVM> rolesDatabase = _repository.GetPartnerRoles(id);
            List<TipoSocioDTO> rolesDTO = new List<TipoSocioDTO>();

            foreach (TipoSocioVM role in rolesDatabase)
                rolesDTO.Add(_mapper.Map<TipoSocioDTO>(role));

            return rolesDTO;
        }

        public List<string> GetPartnerRoles(int id)
        {
            List<TipoSocioVM> rolesDatabase = _repository.GetPartnerRoles(id);
            List<string> roles = new List<string>();

            foreach (TipoSocioVM role in rolesDatabase)
                roles.Add($"s{role.tipoSocio}");

            return roles;
        }

        public List<TipoSocioDTO> GetPossibleRoles()
        {
            List<TipoSocioVM> rolesDB = _repository.GetPossibleRoles();
            List<TipoSocioDTO> roles = new List<TipoSocioDTO>();

            foreach(TipoSocioVM role in rolesDB)
            {
                if(role.tipoSocio != 'a')
                {
                    TipoSocioDTO roleDTO = _mapper.Map<TipoSocioDTO>(role);
                    roles.Add(roleDTO);
                }
            }

            return roles;
        }

        public void AssignRoles(int id, List<int> roles)
        {
            _repository.AssignRoles(id, roles);
        }

        public void UpdateRoles(int id, List<int> roles)
        {
            _repository.UpdateRoles(id, roles);
        }

        public void DeleteRoles(int id)
        {
            _repository.DeleteRoles(id);
        }
    }
}
