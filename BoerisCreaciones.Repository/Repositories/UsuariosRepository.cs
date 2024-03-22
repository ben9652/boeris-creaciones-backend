using BoerisCreaciones.Core;
using BoerisCreaciones.Core.Models;
using BoerisCreaciones.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Bcpg.OpenPgp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
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

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Console.WriteLine("Ejecución de Authenticate()");

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
            stopwatch.Stop();

            TimeSpan elapsedTime = stopwatch.Elapsed;
            Console.WriteLine("Tiempo transcurrido: " + elapsedTime.TotalMilliseconds.ToString("0.00") + "ms");
            Console.WriteLine("");
            Console.WriteLine("");

            return user;
        }
        
        public Usuario Authenticate(UsuarioLogin userObj)
        {
            Usuario user = null;

            using (MySqlConnection conn = new MySqlConnection(_applicationConfig.ConnectionStrings.BoerisCreacionesConnection))
            {
                conn.Open();
                //MySqlCommand cmd = new MySqlCommand("ComprobarExistenciaUsuario", conn);
                //cmd.CommandType = System.Data.CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("p_user", userObj.username);
                string queryString = "SELECT * FROM V_MostrarUsuarios WHERE username = @user";

                MySqlCommand cmd = new MySqlCommand(queryString, conn);
                cmd.Parameters.AddWithValue("@user", userObj.username);
                cmd.Prepare();

                DbDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    if (Convert.ToChar(reader["estado"]) != 'A')
                        throw new ArgumentException("El usuario no está activo.");

                    user = new Usuario(
                        Convert.ToInt32(reader["id_usuario"]),
                        reader["username"].ToString(),
                        reader["password"].ToString(),
                        reader["apellidos"].ToString(),
                        reader["nombres"].ToString(),
                        reader["email"].ToString(),
                        Convert.ToChar(reader["estado"]),
                        Convert.ToChar(reader["rol"])
                    );
                }
                else
                    throw new ArgumentException("El usuario no existe.");

                conn.Close();
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
