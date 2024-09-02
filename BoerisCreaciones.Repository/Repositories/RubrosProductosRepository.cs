using BoerisCreaciones.Core;
using BoerisCreaciones.Core.Models.Rubros;
using BoerisCreaciones.Repository.Interfaces;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.Common;

namespace BoerisCreaciones.Repository.Repositories
{
    public class RubrosProductosRepository : IRubrosProductosRepository
    {
        private readonly ConnectionStringProvider _connectionString;

        public RubrosProductosRepository(ConnectionStringProvider connectionString)
        {
            _connectionString = connectionString;
        }

        public List<RubroProductoVM> GetProductsCategories()
        {
            List<RubroProductoVM> rubrosP = new List<RubroProductoVM>();
            using (MySqlConnection conn = new MySqlConnection(_connectionString.ConnectionString))
            {
                conn.Open();

                string queryString = "SELECT * FROM V_ObtenerRubrosP";

                MySqlCommand cmd = new MySqlCommand(queryString, conn);
                DbDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    RubroProductoVM rubroP = new RubroProductoVM(
                        Convert.ToInt32(reader["id_rubroP"]),
                        reader["nombre"].ToString()
                    );
                    rubrosP.Add(rubroP);
                }

                conn.Close();
            }

            return rubrosP;
        }

        public RubroProductoVM GetProductsCategory(int id)
        {
            RubroProductoVM rubroP = null;
            using (MySqlConnection conn = new MySqlConnection(_connectionString.ConnectionString))
            {
                conn.Open();

                string queryString = $"SELECT * FROM V_ObtenerRubrosP WHERE id_rubroP = {id}";

                MySqlCommand cmd = new MySqlCommand(queryString , conn);

                DbDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    rubroP = new RubroProductoVM(
                        Convert.ToInt32(reader["id_rubroP"]),
                        reader["nombre"].ToString()
                    );
                }
                else
                    throw new KeyNotFoundException("No se encuentra el rubro de materia prima especificado");

                conn.Close();
            }

            return rubroP;
        }

        public RubroProductoVM CreateProductsCategory(string category)
        {
            RubroProductoVM rubroCreado;

            using (MySqlConnection conn = new MySqlConnection(_connectionString.ConnectionString))
            {
                conn.Open();

                string queryString = "SELECT * FROM V_ObtenerRubrosP WHERE nombre = @nombre";

                MySqlCommand cmd = new MySqlCommand(queryString, conn);
                cmd.Parameters.AddWithValue("@nombre", category);
                cmd.Prepare();

                DbDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                    throw new DuplicateNameException("Ya existe este rubro");

                reader.Close();

                queryString = "INSERT INTO RubrosP(nombre) VALUES (@nombre)";

                cmd = new MySqlCommand(queryString, conn);

                cmd.Parameters.AddWithValue("@nombre", category);
                cmd.Prepare();

                cmd.ExecuteNonQuery();

                queryString = $"SELECT * FROM RubrosP WHERE id_rubroP = LAST_INSERT_ID()";

                cmd = new MySqlCommand(queryString , conn);
                reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    rubroCreado = new RubroProductoVM(Convert.ToInt32(reader["id_rubroP"]), reader["nombre"].ToString());
                    reader.Close();
                }
                else
                    throw new DirectoryNotFoundException("No se pudo crear el rubro");

                conn.Close();
            }

            return rubroCreado;
        }

        public void ModifyProductsCategory(RubroProductoVM category, List<string> attributesToChange)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionString.ConnectionString))
            {
                conn.Open();
                string queryString = "SELECT * FROM V_ObtenerRubrosP WHERE nombre = @nombre";

                MySqlCommand cmd = new MySqlCommand(queryString, conn);
                cmd.Parameters.AddWithValue("@nombre", category.nombre);
                cmd.Prepare();

                DbDataReader reader = cmd.ExecuteReader();

                if (reader.Read() && attributesToChange.Find(attr => attr == "nombre") != null)
                    throw new DuplicateNameException("El rubro de producto especificado ya existe");

                reader.Close();

                queryString = $"UPDATE RubrosP SET nombre = '{category.nombre}' WHERE id_rubroP = {category.id_rubroP}";
                cmd = new MySqlCommand(queryString, conn);

                int affectedColumns = cmd.ExecuteNonQuery();

                conn.Close();
            }
        }

        public void DeleteProductsCategory(int id)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionString.ConnectionString))
            {
                conn.Open();

                string queryString = $"DELETE FROM RubrosP WHERE id_rubroP = {id}";

                MySqlCommand cmd = new MySqlCommand(queryString , conn);

                cmd.ExecuteNonQuery();

                conn.Close();
            }
        }
    }
}
