using BoerisCreaciones.Core;
using BoerisCreaciones.Core.Models;
using BoerisCreaciones.Repository.Interfaces;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.Common;

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

        public UsuarioVM GetUserById(int id)
        {
            UsuarioVM user = null;

            using (MySqlConnection conn = new MySqlConnection(_connectionStringProvider.ConnectionString))
            {
                conn.Open();

                string queryString = "SELECT * FROM V_MostrarUsuarios WHERE id_usuario = @id";

                MySqlCommand cmd = new MySqlCommand(queryString, conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Prepare();

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

                    user = new UsuarioVM(
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

            return user;
        }
        
        public UsuarioVM Authenticate(UsuarioLogin userObj)
        {
            UsuarioVM user = null;

            using (MySqlConnection conn = new MySqlConnection(_connectionStringProvider.ConnectionString))
            {
                conn.Open();

                string queryString = "SELECT * FROM V_MostrarUsuarios WHERE username = @user";

                MySqlCommand cmd = new MySqlCommand(queryString, conn);
                cmd.Parameters.AddWithValue("@user", userObj.username);
                cmd.Prepare();

                DbDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    if (Convert.ToChar(reader["estado"]) != 'A')
                        throw new ArgumentException("El usuario no está activo.");

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

                    user = new UsuarioVM(
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
                else
                    throw new ArgumentException("El usuario no existe.");

                conn.Close();
            }

            return user;
        }

        public void RegisterUser(UsuarioVM userObj)
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

        public void UpdateUser(UsuarioVM userObj, List<string> attributesToChange)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionStringProvider.ConnectionString))
            {
                conn.Open();

                string queryString = "SELECT * FROM V_MostrarUsuarios WHERE username = @username";

                MySqlCommand cmd = new MySqlCommand(queryString, conn);
                cmd.Parameters.AddWithValue("@username", userObj.username);
                cmd.Prepare();

                DbDataReader reader = cmd.ExecuteReader();

                if (reader.Read() && attributesToChange.Find(attr => attr == "username") != null)
                    throw new DuplicateNameException("El nombre de usuario ya existe");
            }

            ctx.LoadStoredProcedure("ActualizarUsuario", _connectionStringProvider)
                    .WithSqlParam("p_id_usuario", userObj.id_usuario)
                    .WithSqlParam("p_nombre", userObj.nombre)
                    .WithSqlParam("p_email", userObj.email)
                    .WithSqlParam("p_username", userObj.username)
                    .WithSqlParam("p_password", userObj.password)
                    .WithSqlParam("p_fecha_alta", userObj.fecha_alta)
                    .WithSqlParam("p_rol", userObj.rol)
                    .WithSqlParam("p_estado", userObj.estado)
                    .WithSqlParam("p_domicilio", userObj.domicilio)
                    .WithSqlParam("p_telefono", userObj.telefono)
                    .WithSqlParam("p_observaciones", userObj.observaciones)
                    .ExecuteVoidStoredProcedure();
        }

        public void DeleteUser(int id)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionStringProvider.ConnectionString))
            {
                conn.Open();

                string queryString = "DELETE FROM Usuarios WHERE id_usuario = @id";

                MySqlCommand cmd = new MySqlCommand(queryString, conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Prepare();

                cmd.ExecuteNonQuery();
            }
        }

        private dynamic GetMailParams()
        {
            return ctx.LoadStoredProcedure("ObtenerParametrosMail", _connectionStringProvider)
                .ExecuteSingleResultStoredProcedure<dynamic>();
        }
    }
}
