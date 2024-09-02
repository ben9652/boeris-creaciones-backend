using BoerisCreaciones.Core;
using BoerisCreaciones.Core.Models.Proveedores;
using BoerisCreaciones.Repository.Interfaces;
using MySql.Data.MySqlClient;
using System.Data.Common;

namespace BoerisCreaciones.Repository.Repositories
{
    public class CatalogoProveedoresRepository : ICatalogoProveedoresRepository
    {
        private readonly ConnectionStringProvider _connectionString;
        private readonly BoerisCreacionesContext _ctx;

        public CatalogoProveedoresRepository(ConnectionStringProvider connectionString, BoerisCreacionesContext ctx)
        {
            _connectionString = connectionString;
            _ctx = ctx;
        }

        public List<ProveedorVM> GetProviders()
        {
            List<ProveedorVM> proveedores = new List<ProveedorVM>();

            using (MySqlConnection conn = new MySqlConnection(_connectionString.ConnectionString))
            {
                conn.Open();

                string queryString = "SELECT * FROM V_ListarCatalogoProveedores";

                MySqlCommand cmd = new MySqlCommand(queryString, conn);

                DbDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    object domicilioDB = reader["domicilio"];
                    object telefonoDB = reader["telefono"];
                    object cvuDB = reader["cvu"];
                    object aliasDB = reader["alias"];

                    string? domicilio = domicilioDB == DBNull.Value ? null : domicilioDB.ToString();
                    long? telefono = telefonoDB == DBNull.Value ? null : Convert.ToInt64(telefonoDB);
                    string? cvu = cvuDB == DBNull.Value ? null : cvuDB.ToString();
                    string? alias = aliasDB == DBNull.Value ? null : aliasDB.ToString();

                    ProveedorVM proveedor = new ProveedorVM(
                        Convert.ToInt32(reader["id"]),
                        reader["nombre"].ToString(),
                        Convert.ToInt32(reader["id_rubro"]),
                        reader["rubroAsociado"].ToString(),
                        domicilio,
                        telefono,
                        cvu,
                        alias
                    );

                    proveedores.Add(proveedor);
                }

                conn.Close();
            }

            return proveedores;
        }

        public ProveedorVM GetProvider(int id)
        {
            ProveedorVM proveedor;

            using (MySqlConnection conn = new(_connectionString.ConnectionString))
            {
                conn.Open();

                string queryString = $"SELECT * FROM V_ListarCatalogoProveedores WHERE id = {id}";

                MySqlCommand cmd = new MySqlCommand(queryString, conn);

                DbDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    object domicilioDB = reader["domicilio"];
                    object telefonoDB = reader["telefono"];
                    object cvuDB = reader["cvu"];
                    object aliasDB = reader["alias"];

                    string? domicilio = domicilioDB == DBNull.Value ? null : domicilioDB.ToString();
                    long? telefono = telefonoDB == DBNull.Value ? null : Convert.ToInt64(telefonoDB);
                    string? cvu = cvuDB == DBNull.Value ? null : cvuDB.ToString();
                    string? alias = aliasDB == DBNull.Value ? null : aliasDB.ToString();

                    proveedor = new ProveedorVM(
                        Convert.ToInt32(reader["id"]),
                        reader["nombre"].ToString(),
                        Convert.ToInt32(reader["id_rubro"]),
                        reader["rubroAsociado"].ToString(),
                        domicilio,
                        telefono,
                        cvu,
                        alias
                    );
                }
                else
                    throw new KeyNotFoundException("No existe un proveedor con el ID proporcionado");

                conn.Close();
            }

            return proveedor;
        }

        public ProveedorVM CreateProvider(ProveedorVM provider)
        {
            return _ctx.LoadStoredProcedure("CrearProveedor", _connectionString)
                .WithSqlParam("p_nombre", provider.nombre)
                .WithSqlParam("p_id_rubro", provider.id_rubro)
                .WithSqlParam("p_domicilio", provider.domicilio)
                .WithSqlParam("p_telefono", provider.telefono)
                .WithSqlParam("p_cvu", provider.cvu)
                .WithSqlParam("p_alias", provider.alias)
                .ExecuteSingleResultStoredProcedure<ProveedorVM>();
        }

        public ProveedorVM UpdateProvider(ProveedorVM provider, List<string> attributesToChange)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionString.ConnectionString))
            {
                conn.Open();

                string queryString = "SELECT * FROM V_ListarCatalogoProveedores WHERE nombre = @nombre";

                MySqlCommand cmd = new MySqlCommand(queryString, conn);
                cmd.Parameters.AddWithValue("@nombre", provider.nombre);
                cmd.Prepare();

                DbDataReader reader = cmd.ExecuteReader();

                if (reader.Read() && attributesToChange.Find(attr => attr == "nombre") != null)
                    throw new DuplicateWaitObjectException("El nombre del proveedor ya existe");

                reader.Close();
            }

            return _ctx.LoadStoredProcedure("ActualizarProveedor", _connectionString)
                .WithSqlParam("p_id", provider.id)
                .WithSqlParam("p_nombre", provider.nombre)
                .WithSqlParam("p_id_rubro", provider.id_rubro)
                .WithSqlParam("p_domicilio", provider.domicilio)
                .WithSqlParam("p_telefono", provider.telefono)
                .WithSqlParam("p_cvu", provider.cvu)
                .WithSqlParam("p_alias", provider.alias)
                .ExecuteSingleResultStoredProcedure<ProveedorVM>();
        }

        public void DeleteProvider(int id)
        {
            _ctx.LoadStoredProcedure("EliminarProveedor", _connectionString)
                .WithSqlParam("p_id_usuario", id)
                .ExecuteSingleResultStoredProcedure<ProveedorVM>();
        }
    }
}
