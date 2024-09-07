using BoerisCreaciones.Core;
using BoerisCreaciones.Core.Models.Localidades;
using BoerisCreaciones.Repository.Interfaces;
using MySql.Data.MySqlClient;
using System.Data.Common;

namespace BoerisCreaciones.Repository.Repositories
{
    public class LocalidadesRepository : ILocalidadesRepository
    {
        private readonly ConnectionStringProvider _connection;
        private readonly BoerisCreacionesContext _ctx;

        public LocalidadesRepository(ConnectionStringProvider connection, BoerisCreacionesContext ctx)
        {
            _connection = connection;
            _ctx = ctx;
        }

        public List<LocalidadVM> GetAll()
        {
            List<LocalidadVM> localidades = new List<LocalidadVM>();

            using (MySqlConnection conn = new MySqlConnection(_connection.ConnectionString))
            {
                conn.Open();

                string queryString = "SELECT * FROM V_ListarLocalidades";

                MySqlCommand cmd = new MySqlCommand(queryString, conn);

                DbDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    LocalidadVM localidad = new LocalidadVM(
                        Convert.ToInt32(reader["id_localidad"]),
                        reader["nombre"].ToString(),
                        Convert.ToInt32(reader["id_provincia"]),
                        reader["provincia"].ToString()
                    );

                    localidades.Add(localidad);
                }

                conn.Close();
            }

            return localidades;
        }

        public LocalidadVM GetById(int id)
        {
            LocalidadVM localidad = null;

            using (MySqlConnection conn = new MySqlConnection(_connection.ConnectionString))
            {
                conn.Open();

                string queryString = $"SELECT * FROM V_ListarLocalidades WHERE id_localidad = {id}";

                MySqlCommand cmd = new MySqlCommand(queryString, conn);

                DbDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    localidad = new LocalidadVM(
                        Convert.ToInt32(reader["id_localidad"]),
                        reader["nombre"].ToString(),
                        Convert.ToInt32(reader["id_provincia"]),
                        reader["provincia"].ToString()
                    );
                }
                else
                    throw new KeyNotFoundException("No existe la localidad especificada");

                conn.Close();
            }

            return localidad;
        }

        public List<LocalidadVM> GetByIdProvince(int id)
        {
            List<LocalidadVM> localidades = new List<LocalidadVM>();

            using (MySqlConnection conn = new MySqlConnection(_connection.ConnectionString))
            {
                conn.Open();

                string queryString = $"SELECT * FROM V_ListarLocalidades WHERE id_provincia = {id}";

                MySqlCommand cmd = new MySqlCommand(queryString, conn);

                DbDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    localidades.Add(new LocalidadVM(
                        Convert.ToInt32(reader["id_localidad"]),
                        reader["nombre"].ToString(),
                        Convert.ToInt32(reader["id_provincia"]),
                        reader["provincia"].ToString()
                    ));
                }

                conn.Close();
            }

            return localidades;
        }

        public LocalidadVM Create(LocalidadVM localidad)
        {
            return _ctx.LoadStoredProcedure("CrearLocalidad", _connection)
                .WithSqlParam("p_id_provincia", localidad.id_provincia)
                .WithSqlParam("p_nombre", localidad.nombre)
                .ExecuteSingleResultStoredProcedure<LocalidadVM>();
        }

        public LocalidadVM Update(LocalidadVM localidad, List<string> attributes)
        {
            return _ctx.LoadStoredProcedure("ActualizarLocalidad", _connection)
                .WithSqlParam("p_id", localidad.id_localidad)
                .WithSqlParam("p_id_provincia", localidad.id_provincia)
                .WithSqlParam("p_nombre", localidad.nombre)
                .ExecuteSingleResultStoredProcedure<LocalidadVM>();
        }

        public void Delete(int id)
        {
            using (MySqlConnection conn = new MySqlConnection(_connection.ConnectionString))
            {
                conn.Open();

                string queryString = $"DELETE FROM Localidades WHERE id_localidad = {id}";

                MySqlCommand cmd = new MySqlCommand(queryString, conn);

                cmd.ExecuteNonQuery();

                conn.Close();
            }
        }
    }
}
