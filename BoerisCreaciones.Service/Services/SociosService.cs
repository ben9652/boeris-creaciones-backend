using AutoMapper;
using BoerisCreaciones.Core.Models;
using BoerisCreaciones.Core.Models.Socio;
using BoerisCreaciones.Repository.Interfaces;
using BoerisCreaciones.Service.Helpers;
using BoerisCreaciones.Service.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Globalization;
using System.Text;

namespace BoerisCreaciones.Service.Services
{
    public class SociosService : UsuariosService, ISociosService
    {
        private new readonly ISociosRepository _repository;
        private readonly IRolesSociosService _rolesService;
        private readonly IMapper _mapper;

        public SociosService(IUsuariosRepository usuariosRepository, ISociosRepository sociosRepository, IRolesSociosService rolesService, IConfiguration configuration, IMapper mapper)
            : base(usuariosRepository, configuration)
        {
            _repository = sociosRepository;
            _rolesService = rolesService;
            _mapper = mapper;
        }

        public List<SocioDTO> GetPartners()
        {
            List<UsuarioVM> partnersListDatabase = _repository.GetPartners();

            List<SocioDTO> partnersList = new List<SocioDTO>();
            foreach(UsuarioVM partnerDB in partnersListDatabase)
            {
                SocioDTO partner = _mapper.Map<SocioDTO>(partnerDB);
                partner.partnerRoles = _rolesService.GetPartnerRolesCompleteNames(partner.id_user);

                if (!isAdminPartner(partner))
                    partnersList.Add(partner);
            }

            return partnersList;
        }

        private static bool isAdminPartner(SocioDTO partner)
        {
            foreach(TipoSocioDTO role in partner.partnerRoles)
            {
                if (role.role.Contains("Administrador"))
                    return true;
            }
            return false;
        }

        private static string RemoveDiacritics(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        private static string FormatString(string input, bool capitalizeFirst)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            input = RemoveDiacritics(input.ToLower());

            input = input.Replace(" ", "");

            if (capitalizeFirst)
                return char.ToUpper(input[0]) + input.Substring(1);
            else
                return input;
        }

        public SocioDTO RegisterPartner(SocioRegistro user)
        {
            string nombreRegistro = FormatString(user.nombre, false);
            string apellidoRegistro = FormatString(user.apellido, true);

            string username = nombreRegistro + apellidoRegistro;
            string password = PasswordHasher.HashPassword(username);

            string nombre = user.nombre + "," + (user.apellido == "" || user.apellido == null ? "-" : user.apellido);
            UsuarioVM userDB = new UsuarioVM(
                0,
                nombre,
                user.email,
                username,
                password,
                new DateTime(),
                's',
                '0',
                null,
                null,
                null
            );

            userDB = _repository.RegisterPartner(userDB);

            SocioDTO createdPartner = _mapper.Map<SocioDTO>(userDB);

            return createdPartner;
        }

        public void DeletePartner(int id)
        {
            _repository.DeletePartner(id);
        }
    }
}
