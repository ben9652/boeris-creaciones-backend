using BoerisCreaciones.Core;
using BoerisCreaciones.Core.Models.Rubros;
using BoerisCreaciones.Repository.Interfaces;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.Common;

namespace BoerisCreaciones.Repository.Repositories
{
    public class RubrosMateriasPrimasRepository : IRubrosMateriasPrimasRepository
    {
        private readonly ConnectionStringProvider _connectionStringProvider;

        public RubrosMateriasPrimasRepository(ConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }

        public List<RubroMateriaPrima> GetRawMaterialsCategories()
        {
            List<RubroMateriaPrima> rubrosMP = new List<RubroMateriaPrima>();
            using (MySqlConnection conn = new MySqlConnection(_connectionStringProvider.ConnectionString))
            {
                conn.Open();

                string queryString = "SELECT * FROM V_ObtenerRubrosMP";

                MySqlCommand cmd = new MySqlCommand(queryString, conn);
                DbDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    RubroMateriaPrima rubroMP = new RubroMateriaPrima(
                        Convert.ToInt32(reader["id_rubroMP"]),
                        reader["nombre"].ToString()
                    );
                    rubrosMP.Add(rubroMP);
                }

                conn.Close();
            }

            return rubrosMP;
        }

        public RubroMateriaPrima GetRawMaterialsCategory(int id)
        {
            RubroMateriaPrima rubroMP = null;
            using (MySqlConnection conn = new MySqlConnection(_connectionStringProvider.ConnectionString))
            {
                conn.Open();

                string queryString = "SELECT * FROM V_ObtenerRubrosMP WHERE id_rubroMP = @id";

                MySqlCommand cmd = new MySqlCommand(queryString, conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Prepare();
                DbDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    rubroMP = new RubroMateriaPrima(
                        Convert.ToInt32(reader["id_rubroMP"]),
                        reader["nombre"].ToString()
                    );
                }
                else
                    throw new KeyNotFoundException("No se encuentra el rubro de materia prima especificado");

                conn.Close();
            }

            return rubroMP;
        }

        public void CreateRawMaterialCategory(string category)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionStringProvider.ConnectionString))
            {
                conn.Open();

                string queryString = "SELECT * FROM V_ObtenerRubrosMP WHERE nombre = @nombre";

                MySqlCommand cmd = new MySqlCommand(queryString, conn);
                cmd.Parameters.AddWithValue("@nombre", category);
                cmd.Prepare();

                DbDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                    throw new DuplicateNameException("Ya existe este rubro");

                reader.Close();

                queryString = "INSERT INTO RubrosMP(nombre) VALUES (@nombre)";

                cmd = new MySqlCommand(queryString, conn);

                cmd.Parameters.AddWithValue("@nombre", category);
                cmd.Prepare();

                cmd.ExecuteNonQuery();

                conn.Close();
            }
        }

        public void ModifyRawMaterialCategory(RubroMateriaPrima category, List<string> attributesToChange)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionStringProvider.ConnectionString))
            {
                conn.Open();
                string queryString = "SELECT * FROM V_ObtenerRubrosMP WHERE nombre = @nombre";

                MySqlCommand cmd = new MySqlCommand(queryString, conn);
                cmd.Parameters.AddWithValue("@nombre", category.nombre);
                cmd.Prepare();

                DbDataReader reader = cmd.ExecuteReader();

                if (reader.Read() && attributesToChange.Find(attr => attr == "nombre") != null)
                    throw new DuplicateNameException("El rubro de materia especificado ya existe");

                reader.Close();

                queryString = $"UPDATE RubrosMP SET nombre = '{category.nombre}' WHERE id_rubroMP = {category.id_rubroMP}";
                cmd = new MySqlCommand(queryString, conn);

                int affectedColumns = cmd.ExecuteNonQuery();

                conn.Close();
            }
        }

        public void DeleteRawMaterialCategory(int id)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionStringProvider.ConnectionString))
            {
                conn.Open();

                string queryString = $"DELETE FROM RubrosMP WHERE id_rubroMP = {id}";

                MySqlCommand cmd = new MySqlCommand(queryString, conn);

                cmd.ExecuteNonQuery();

                conn.Close();
            }
        }
    }
}
