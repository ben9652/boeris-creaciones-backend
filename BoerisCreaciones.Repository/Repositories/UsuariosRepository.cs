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
    public class UsuariosRepository : Repository<UsuarioVM>, IUsuariosRepository
    {
        private readonly ConnectionStringProvider _connectionStringProvider;
        private readonly BoerisCreacionesContext ctx;

        public UsuariosRepository(ConnectionStringProvider connectionStringProvider, BoerisCreacionesContext context) : base(context)
        {
            _connectionStringProvider = connectionStringProvider;
            ctx = context;
        }

        private UsuarioVM GetUser(int id)
        {
            UsuarioVM user;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Console.WriteLine("Ejecución de Authenticate()");

            try
            {
                user = ctx.LoadStoredProcedure("ObtenerUsuario", _connectionStringProvider)
                    .WithSqlParam("p_id_usuario", id)
                    .ExecuteSingleResultStoredProcedure<UsuarioVM>();
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
        
        public UsuarioVM Authenticate(UsuarioLogin userObj)
        {
            UsuarioVM user = null;

            using (MySqlConnection conn = new MySqlConnection(_connectionStringProvider.ConnectionString))
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

                    user = new UsuarioVM(
                        Convert.ToInt32(reader["id_usuario"]),
                        reader["nombre"].ToString(),
                        reader["email"].ToString(),
                        reader["username"].ToString(),
                        reader["password"].ToString(),
                        Convert.ToDateTime(reader["fecha_alta"]),
                        Convert.ToChar(reader["rol"]),
                        Convert.ToChar(reader["estado"]),
                        reader["domicilio"].ToString(),
                        Convert.ToUInt64(reader["telefono"]),
                        reader["observaciones"].ToString()
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
                ctx.LoadStoredProcedure("CrearUsuario", _connectionStringProvider)
                    .WithSqlParam("p_nombre", userObj.nombre)
                    .WithSqlParam("p_email", userObj.email)
                    .WithSqlParam("p_user", userObj.username)
                    .WithSqlParam("p_pass", userObj.password)
                    .WithSqlParam("p_rol", userObj.rol)
                    .WithSqlParam("p_domicilio", userObj.domicilio)
                    .WithSqlParam("p_telefono", userObj.telefono)
                    .WithSqlParam("p_observ", userObj.observaciones)
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
                return ctx.LoadStoredProcedure("ObtenerParametrosMail", _connectionStringProvider)
                    .ExecuteSingleResultStoredProcedure<dynamic>();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
