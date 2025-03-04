using BoerisCreaciones.Core;
using BoerisCreaciones.Core.Models.MateriasPrimas;
using BoerisCreaciones.Repository.Interfaces;
using MySql.Data.MySqlClient;
using System.Data.Common;

namespace BoerisCreaciones.Repository.Repositories
{
    public class CatalogoMateriasPrimasRepository : ICatalogoMateriasPrimasRepository
    {
        private readonly ConnectionStringProvider connectionStringProvider;
        private readonly BoerisCreacionesContext ctx;

        public CatalogoMateriasPrimasRepository(ConnectionStringProvider connectionStringProvider, BoerisCreacionesContext ctx)
        {
            this.connectionStringProvider = connectionStringProvider;
            this.ctx = ctx;
        }

        public List<MateriaPrimaVM> GetRawMaterialsItems(List<int>? categories = null)
        {
            List<MateriaPrimaVM> materiasPrimas = new List<MateriaPrimaVM>();

            using(MySqlConnection conn = new MySqlConnection(connectionStringProvider.ConnectionString))
            {
                conn.Open();

                string queryString = "SELECT * FROM V_ListarCatalogoMateriasPrimas";

                if (categories != null)
                {
                    queryString += " WHERE id_rubroMP IN (";
                    for (int i = 0; i < categories.Count; i++)
                    {
                        queryString += categories[i];
                        if (i < categories.Count - 1)
                            queryString += ", ";
                    }
                    queryString += ")";
                }

                MySqlCommand cmd = new MySqlCommand(queryString, conn);

                DbDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    object comentarioDB = reader["comentario"];
                    string? comentario = comentarioDB == DBNull.Value ? null : comentarioDB.ToString();

                    MateriaPrimaVM materiaPrima = new MateriaPrimaVM(
                        Convert.ToInt32(reader["id_matP"]),
                        Convert.ToInt32(reader["id_rubroMP"]),
                        reader["rubro"].ToString(),
                        Convert.ToInt32(reader["id_unidad"]),
                        reader["unidad"].ToString(),
                        reader["nombre"].ToString(),
                        Convert.ToChar(reader["origen"]),
                        Convert.ToInt32(reader["cantidad_restante"]),
                        Convert.ToInt32(reader["cantidad_descartada"]),
                        reader["imagen"].ToString(),
                        comentario
                    );

                    materiasPrimas.Add(materiaPrima);
                }

                conn.Close();
            }

            return materiasPrimas;
        }

        public MateriaPrimaVM GetRawMaterialsItem(int id)
        {
            MateriaPrimaVM materiaPrima;

            using (MySqlConnection conn = new(connectionStringProvider.ConnectionString))
            {
                conn.Open();

                string queryString = $"SELECT * FROM V_ListarCatalogoMateriasPrimas WHERE id_matP = {id}";

                MySqlCommand cmd = new MySqlCommand(queryString, conn);

                DbDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    object comentarioDB = reader["comentario"];
                    string? comentario = comentarioDB == DBNull.Value ? null : comentarioDB.ToString();

                    materiaPrima = new MateriaPrimaVM(
                        Convert.ToInt32(reader["id_matP"]),
                        Convert.ToInt32(reader["id_rubroMP"]),
                        reader["rubro"].ToString(),
                        Convert.ToInt32(reader["id_unidad"]),
                        reader["unidad"].ToString(),
                        reader["nombre"].ToString(),
                        Convert.ToChar(reader["origen"]),
                        Convert.ToInt32(reader["cantidad_restante"]),
                        Convert.ToInt32(reader["cantidad_descartada"]),
                        reader["imagen"].ToString(),
                        comentario
                    );
                }
                else
                    throw new KeyNotFoundException("No existe una materia prima con el ID proporcionado");

                conn.Close();
            }

            return materiaPrima;
        }

        public MateriaPrimaVM CreateRawMaterialItem(MateriaPrimaVM item)
        {
            return ctx.LoadStoredProcedure("CrearMateriaPrima", connectionStringProvider)
                .WithSqlParam("p_id_rubro", item.id_rubroMP)
                .WithSqlParam("p_id_unidad", item.id_unidad)
                .WithSqlParam("p_nombre", item.nombre)
                .WithSqlParam("p_origen", item.origen)
                .WithSqlParam("p_imagen", item.imagen)
                .WithSqlParam("p_comentario", item.comentario)
                .ExecuteSingleResultStoredProcedure<MateriaPrimaVM>();
        }

        public MateriaPrimaVM UpdateRawMaterialItem(MateriaPrimaVM item, List<string> attributesToChange)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionStringProvider.ConnectionString))
            {
                conn.Open();

                string queryString = "SELECT * FROM MateriasPrimas WHERE nombre = @nombre";

                MySqlCommand cmd = new MySqlCommand(queryString, conn);
                cmd.Parameters.AddWithValue("@nombre", item.nombre);
                cmd.Prepare();

                DbDataReader reader = cmd.ExecuteReader();

                if (reader.Read() && attributesToChange.Find(attr => attr == "name") != null)
                {
                    string? nombre = reader["nombre"].ToString();
                    if(item.nombre == nombre)
                        throw new DuplicateWaitObjectException("El nombre de la materia prima ya existe");
                }

                reader.Close();
            }

            return ctx.LoadStoredProcedure("ActualizarMateriaPrima", connectionStringProvider)
                .WithSqlParam("p_id", item.id_matP)
                .WithSqlParam("p_id_rubro", item.id_rubroMP)
                .WithSqlParam("p_id_unidad", item.id_unidad)
                .WithSqlParam("p_nombre", item.nombre)
                .WithSqlParam("p_origen", item.origen)
                .WithSqlParam("p_imagen", item.imagen)
                .WithSqlParam("p_comentario", item.comentario)
                .ExecuteSingleResultStoredProcedure<MateriaPrimaVM>();
        }

        public void DeleteRawMaterialItem(int id)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionStringProvider.ConnectionString))
            {
                conn.Open();

                string queryString = $"DELETE FROM MateriasPrimas WHERE id_matP = {id}";

                MySqlCommand cmd = new MySqlCommand(queryString, conn);

                int rowsAffected = cmd.ExecuteNonQuery();

                conn.Close();
            }
        }
    }
}
