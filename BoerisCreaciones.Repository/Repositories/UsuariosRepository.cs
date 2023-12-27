using BoerisCreaciones.Core;
using BoerisCreaciones.Core.Models;
using BoerisCreaciones.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
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

        private Usuario GetUser(int id)
        {
            Usuario user;
            try
            {
                user = ctx.LoadStoredProcedure("ObtenerUsuario", _applicationConfig)
                    .WithSqlParam("p_id_usuario", id)
                    .ExecuteSingleResultStoredProcedure<Usuario>();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return user;
        }

        public Usuario Authenticate(Usuario userObj)
        {
            Usuario user;
            try
            {
                user = ctx.LoadStoredProcedure("ComprobarExistenciaUsuario", _applicationConfig)
                    .WithSqlParam("p_user", userObj.username)
                    .ExecuteSingleResultStoredProcedure<Usuario>();
            }
            catch(Exception ex)
            {
                throw ex;
            }

            return user;
        }

        public void RegisterUser(UsuarioRegistro userObj)
        {
            try
            {
                ctx.LoadStoredProcedure("CrearUsuario", _applicationConfig)
                    .WithSqlParam("p_user", userObj.username)
                    .WithSqlParam("p_pass", userObj.password)
                    .WithSqlParam("p_nombres", userObj.nombres)
                    .WithSqlParam("p_apellidos", userObj.apellidos)
                    .WithSqlParam("p_email", userObj.email)
                    .WithSqlParam("p_rol", 'S')
                    .ExecuteVoidStoredProcedure();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        private dynamic GetMailParams()
        {
            try
            {
                return ctx.LoadStoredProcedure("ObtenerParametrosMail", _applicationConfig)
                    .ExecuteSingleResultStoredProcedure<dynamic>();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
