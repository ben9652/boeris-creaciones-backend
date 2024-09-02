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

        public List<MateriasPrimasItemVM> GetRawMaterialsItems()
        {
            List<MateriasPrimasItemVM> materiasPrimas = new List<MateriasPrimasItemVM>();

            using(MySqlConnection conn = new MySqlConnection(connectionStringProvider.ConnectionString))
            {
                conn.Open();

                string queryString = "SELECT * FROM V_ListarCatalogoMateriasPrimas";

                MySqlCommand cmd = new MySqlCommand(queryString, conn);

                DbDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    object comentarioDB = reader["comentario"];
                    string? comentario = comentarioDB == DBNull.Value ? null : comentarioDB.ToString();

                    MateriasPrimasItemVM materiaPrima = new MateriasPrimasItemVM(
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

        public MateriasPrimasItemVM GetRawMaterialsItem(int id)
        {
            MateriasPrimasItemVM materiaPrima;

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

                    materiaPrima = new MateriasPrimasItemVM(
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

        public MateriasPrimasItemVM CreateRawMaterialItem(MateriasPrimasItemVM item)
        {
            return ctx.LoadStoredProcedure("CrearMateriaPrima", connectionStringProvider)
                .WithSqlParam("p_id_rubro", item.id_rubroMP)
                .WithSqlParam("p_id_unidad", item.id_unidad)
                .WithSqlParam("p_nombre", item.nombre)
                .WithSqlParam("p_origen", item.origen)
                .WithSqlParam("p_imagen", item.imagen)
                .WithSqlParam("p_comentario", item.comentario)
                .ExecuteSingleResultStoredProcedure<MateriasPrimasItemVM>();
        }

        public MateriasPrimasItemVM UpdateRawMaterialItem(MateriasPrimasItemVM item, List<string> attributesToChange)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionStringProvider.ConnectionString))
            {
                conn.Open();

                string queryString = "SELECT * FROM MateriasPrimas WHERE nombre = @nombre";

                MySqlCommand cmd = new MySqlCommand(queryString, conn);
                cmd.Parameters.AddWithValue("@nombre", item.nombre);
                cmd.Prepare();

                DbDataReader reader = cmd.ExecuteReader();

                if (reader.Read() && attributesToChange.Find(attr => attr == "nombre") != null)
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
                .ExecuteSingleResultStoredProcedure<MateriasPrimasItemVM>();
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
