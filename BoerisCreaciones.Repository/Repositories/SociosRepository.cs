using BoerisCreaciones.Core;
using BoerisCreaciones.Core.Models;
using BoerisCreaciones.Core.Models.Socio;
using BoerisCreaciones.Repository.Interfaces;
using MySql.Data.MySqlClient;
using System.Data.Common;

namespace BoerisCreaciones.Repository.Repositories
{
    public class SociosRepository : ISociosRepository
    {
        private readonly ConnectionStringProvider _connectionStringProvider;
        private readonly BoerisCreacionesContext ctx;

        public SociosRepository(ConnectionStringProvider connectionStringProvider, BoerisCreacionesContext ctx)
        {
            _connectionStringProvider = connectionStringProvider;
            this.ctx = ctx;
        }

        public List<UsuarioVM> GetPartners()
        {
            List<UsuarioVM> partnersList = new List<UsuarioVM>();

            using (MySqlConnection conn = new MySqlConnection(_connectionStringProvider.ConnectionString))
            {
                conn.Open();

                string queryString = "SELECT * FROM V_MostrarSocios";

                MySqlCommand cmd = new MySqlCommand(queryString, conn);

                DbDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    object domicilioDB = reader["domicilio"];
                    object telefonoDB = reader["telefono"];
                    object observacionesDB = reader["observaciones"];

                    int id_usuario = Convert.ToInt32(reader["id_usuario"]);
                    string? nombre = reader["nombre"].ToString();
                    string? email = reader["email"].ToString();
                    string? username = reader["username"].ToString();
                    string? password = reader["password"].ToString();
                    DateTime fecha_alta = Convert.ToDateTime(reader["fecha_alta"]);
                    char rol = Convert.ToChar(reader["rol"]);
                    char estado = Convert.ToChar(reader["estado"]);
                    string? domicilio = domicilioDB == DBNull.Value ? null : domicilioDB.ToString();
                    UInt64? telefono = telefonoDB == DBNull.Value ? null : Convert.ToUInt64(telefonoDB);
                    string? observaciones = observacionesDB == DBNull.Value ? null : observacionesDB.ToString();

                    partnersList.Add(new UsuarioVM(
                        id_usuario,
                        nombre,
                        email,
                        username,
                        password,
                        fecha_alta,
                        rol,
                        estado,
                        domicilio,
                        telefono,
                        observaciones
                    ));
                }

                conn.Close();
            }

            return partnersList;
        }

        public UsuarioVM GetPartner(int id)
        {
            UsuarioVM partner = null;

            using (MySqlConnection conn = new MySqlConnection(_connectionStringProvider.ConnectionString))
            {
                conn.Open();

                string queryString = "SELECT * FROM V_MostrarSocios WHERE id_usuario = @id_usuario";

                MySqlCommand cmd = new MySqlCommand(queryString, conn);

                cmd.Parameters.AddWithValue("@id_usuario", id);

                DbDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    object domicilioDB = reader["domicilio"];
                    object telefonoDB = reader["telefono"];
                    object observacionesDB = reader["observaciones"];

                    int id_usuario = Convert.ToInt32(reader["id_usuario"]);
                    string? nombre = reader["nombre"].ToString();
                    string? email = reader["email"].ToString();
                    string? username = reader["username"].ToString();
                    string? password = reader["password"].ToString();
                    DateTime fecha_alta = Convert.ToDateTime(reader["fecha_alta"]);
                    char rol = Convert.ToChar(reader["rol"]);
                    char estado = Convert.ToChar(reader["estado"]);
                    string? domicilio = domicilioDB == DBNull.Value ? null : domicilioDB.ToString();
                    UInt64? telefono = telefonoDB == DBNull.Value ? null : Convert.ToUInt64(telefonoDB);
                    string? observaciones = observacionesDB == DBNull.Value ? null : observacionesDB.ToString();

                    partner = new UsuarioVM(
                        id_usuario,
                        nombre,
                        email,
                        username,
                        password,
                        fecha_alta,
                        rol,
                        estado,
                        domicilio,
                        telefono,
                        observaciones
                    );
                }

                conn.Close();
            }

            return partner;
        }

        public UsuarioVM RegisterPartner(UsuarioVM userObj)
        {
            return ctx.LoadStoredProcedure("CrearSocio", _connectionStringProvider)
                .WithSqlParam("p_nombre", userObj.nombre)
                .WithSqlParam("p_email", userObj.email)
                .WithSqlParam("p_username", userObj.username)
                .WithSqlParam("p_password", userObj.password)
                .WithSqlParam("p_domicilio", userObj.domicilio)
                .WithSqlParam("p_telefono", userObj.telefono)
                .WithSqlParam("p_observaciones", userObj.observaciones)
                .ExecuteSingleResultStoredProcedure<UsuarioVM>();
        }

        public void DeletePartner(int id)
        {
            ctx.LoadStoredProcedure("EliminarSocio", _connectionStringProvider)
                .WithSqlParam("p_id_usuario", id)
                .ExecuteVoidStoredProcedure();
        }
    }
}
