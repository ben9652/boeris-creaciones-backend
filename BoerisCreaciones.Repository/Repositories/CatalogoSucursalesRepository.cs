using BoerisCreaciones.Core;
using BoerisCreaciones.Core.Models.Localidades;
using BoerisCreaciones.Core.Models.Sucursales;
using BoerisCreaciones.Repository.Interfaces;
using MySql.Data.MySqlClient;
using System.Data.Common;

namespace BoerisCreaciones.Repository.Repositories
{
    public class CatalogoSucursalesRepository : ICatalogoSucursalesRepository
    {
        private readonly ConnectionStringProvider _connection;
        private readonly BoerisCreacionesContext _ctx;

        public CatalogoSucursalesRepository(ConnectionStringProvider connection, BoerisCreacionesContext ctx)
        {
            _connection = connection;
            _ctx = ctx;
        }

        public List<SucursalVM> GetAll()
        {
            List<SucursalVM> sucursales = new List<SucursalVM>();

            using (MySqlConnection conn = new MySqlConnection(_connection.ConnectionString))
            {
                conn.Open();

                string queryString = "SELECT * FROM V_ListarSucursales";

                MySqlCommand cmd = new MySqlCommand(queryString, conn);

                DbDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    SucursalVM sucursal = new SucursalVM(
                        Convert.ToInt32(reader["id_sucursal"]),
                        reader["nombre"].ToString(),
                        reader["domicilio"].ToString(),
                        Convert.ToInt32(reader["id_localidad"]),
                        reader["localidad"].ToString()
                    );

                    sucursales.Add(sucursal);
                }

                conn.Close();
            }

            return sucursales;
        }

        public SucursalVM GetById(int id)
        {
            SucursalVM sucursal = null;

            using (MySqlConnection conn = new MySqlConnection(_connection.ConnectionString))
            {
                conn.Open();

                string queryString = $"SELECT * FROM V_ListarSucursales WHERE id_sucursal = {id}";

                MySqlCommand cmd = new MySqlCommand(queryString, conn);

                DbDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    sucursal = new SucursalVM(
                        Convert.ToInt32(reader["id_sucursal"]),
                        reader["nombre"].ToString(),
                        reader["domicilio"].ToString(),
                        Convert.ToInt32(reader["id_localidad"]),
                        reader["localidad"].ToString()
                    );
                }
                else
                    throw new KeyNotFoundException("No existe la sucursal especificada");

                conn.Close();
            }

            return sucursal;
        }

        public List<SucursalVM> GetByIdLocality(int id)
        {
            List<SucursalVM> sucursales = new List<SucursalVM>();

            using (MySqlConnection conn = new MySqlConnection(_connection.ConnectionString))
            {
                conn.Open();

                string queryString = $"SELECT * FROM V_ListarSucursales WHERE id_localidad = {id}";

                MySqlCommand cmd = new MySqlCommand(queryString, conn);

                DbDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    sucursales.Add(new SucursalVM(
                        Convert.ToInt32(reader["id_sucursal"]),
                        reader["nombre"].ToString(),
                        reader["domicilio"].ToString(),
                        Convert.ToInt32(reader["id_localidad"]),
                        reader["localidad"].ToString()
                    ));
                }

                conn.Close();
            }

            return sucursales;
        }

        public SucursalVM Create(SucursalVM sucursal)
        {
            return _ctx.LoadStoredProcedure("CrearSucursal", _connection)
                .WithSqlParam("p_id_localidad", sucursal.id_localidad)
                .WithSqlParam("p_nombre", sucursal.nombre)
                .WithSqlParam("p_domicilio", sucursal.domicilio)
                .ExecuteSingleResultStoredProcedure<SucursalVM>()
                ;
        }

        public SucursalVM Update(SucursalVM sucursal, List<string> attributes)
        {
            return _ctx.LoadStoredProcedure("ActualizarLocalidad", _connection)
                .WithSqlParam("p_id", sucursal.id_sucursal)
                .WithSqlParam("p_id_localidad", sucursal.id_localidad)
                .WithSqlParam("p_nombre", sucursal.nombre)
                .WithSqlParam("p_domicilio", sucursal.domicilio)
                .ExecuteSingleResultStoredProcedure<SucursalVM>();
        }

        public void Delete(int id)
        {
            using (MySqlConnection conn = new MySqlConnection(_connection.ConnectionString))
            {
                conn.Open();

                string queryString = $"DELETE FROM Sucursales WHERE id_sucursal = {id}";

                MySqlCommand cmd = new MySqlCommand(queryString, conn);

                cmd.ExecuteNonQuery();

                conn.Close();
            }
        }
    }
}
