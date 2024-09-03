using BoerisCreaciones.Core;
using BoerisCreaciones.Core.Models;
using BoerisCreaciones.Repository.Interfaces;
using MySql.Data.MySqlClient;
using MySqlConnector.Logging;
using System.Data;
using System.Data.Common;

namespace BoerisCreaciones.Repository.Repositories
{
    public class ProvinciasRepository : IProvinciasRepository
    {
        private readonly BoerisCreacionesContext _ctx;
        private readonly ConnectionStringProvider _connection;

        public ProvinciasRepository(BoerisCreacionesContext ctx, ConnectionStringProvider connection)
        {
            _ctx = ctx;
            _connection = connection;
        }

        public List<ProvinciaVM> GetAllProvinces()
        {
            List<ProvinciaVM> provincias = new List<ProvinciaVM>();

            using (MySqlConnection conn = new MySqlConnection(_connection.ConnectionString))
            {
                conn.Open();

                string queryString = "SELECT * FROM Provincias";

                MySqlCommand cmd = new MySqlCommand(queryString, conn);

                DbDataReader reader = cmd.ExecuteReader();

                while(reader.Read())
                {
                    ProvinciaVM provincia = new ProvinciaVM(
                        Convert.ToInt32(reader["id_provincia"]),
                        reader["nombre"].ToString()
                    );

                    provincias.Add(provincia);
                }

                conn.Close();
            }

            return provincias;
        }

        public ProvinciaVM GetProvince(int id)
        {
            ProvinciaVM provincia = null;

            using (MySqlConnection conn = new MySqlConnection(_connection.ConnectionString))
            {
                conn.Open();

                string queryString = $"SELECT * FROM Provincias WHERE id_provincia = {id}";

                MySqlCommand cmd = new MySqlCommand(queryString , conn);

                DbDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    provincia = new ProvinciaVM(
                        Convert.ToInt32(reader["id_provincia"]),
                        reader["nombre"].ToString()
                    );
                }
                else
                    throw new KeyNotFoundException("No existe la provincia especificada");

                conn.Close();
            }

            return provincia;
        }

        public ProvinciaVM CreateProvince(ProvinciaVM provincia)
        {
            return _ctx.LoadStoredProcedure("CrearProvincia", _connection)
                .WithSqlParam("p_nombre", provincia.nombre)
                .ExecuteSingleResultStoredProcedure<ProvinciaVM>();
        }

        public ProvinciaVM UpdateProvince(ProvinciaVM provincia, List<string> attributesToChange)
        {
            return _ctx.LoadStoredProcedure("ActualizarProvincia", _connection)
                .WithSqlParam("p_id", provincia.id_provincia)
                .WithSqlParam("p_nombre", provincia.nombre)
                .ExecuteSingleResultStoredProcedure<ProvinciaVM>();
        }

        public void DeleteProvince(int id)
        {
            using (MySqlConnection conn = new MySqlConnection(_connection.ConnectionString))
            {
                conn.Open();

                string queryString = $"DELETE FROM Provincias WHERE id_provincia = {id}";

                MySqlCommand cmd = new MySqlCommand(queryString, conn);

                cmd.ExecuteNonQuery();

                conn.Close();
            }
        }
    }
}
