using BoerisCreaciones.Core;
using BoerisCreaciones.Core.Models.MateriasPrimas;
using BoerisCreaciones.Core.Models.Productos;
using BoerisCreaciones.Repository.Interfaces;
using MySql.Data.MySqlClient;
using System.Data.Common;

namespace BoerisCreaciones.Repository.Repositories
{
    public class CatalogoProductosRepository : ICatalogoProductosRepository
    {
        private readonly ConnectionStringProvider _connectionString;
        private readonly BoerisCreacionesContext _ctx;

        public CatalogoProductosRepository(ConnectionStringProvider connectionString, BoerisCreacionesContext ctx)
        {
            _connectionString = connectionString;
            _ctx = ctx;
        }

        public List<ProductosItemVM> GetProductsItems()
        {
            List<ProductosItemVM> productos = new List<ProductosItemVM>();

            using (MySqlConnection conn = new MySqlConnection(_connectionString.ConnectionString))
            {
                conn.Open();

                string queryString = "SELECT * FROM V_ListarCatalogoProductos";

                MySqlCommand cmd = new MySqlCommand(queryString, conn);

                DbDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    object comentarioDB = reader["comentario"];
                    string? comentario = comentarioDB == DBNull.Value ? null : comentarioDB.ToString();

                    double precio = Convert.ToDouble(reader["precio"]);

                    ProductosItemVM producto = new ProductosItemVM(
                        Convert.ToInt32(reader["id_producto"]),
                        Convert.ToInt32(reader["id_rubroP"]),
                        reader["rubro"].ToString(),
                        reader["nombre"].ToString(),
                        (float)precio,
                        Convert.ToInt32(reader["cantidad_restante"]),
                        Convert.ToInt32(reader["cantidad_descartada"]),
                        reader["imagen"].ToString(),
                        comentario
                    );

                    productos.Add(producto);
                }

                conn.Close();
            }

            return productos;
        }

        public ProductosItemVM GetProductsItem(int id)
        {
            ProductosItemVM producto;

            using (MySqlConnection conn = new MySqlConnection(_connectionString.ConnectionString))
            {
                conn.Open();

                string queryString = $"SELECT * FROM V_ListarCatalogoProductos WHERE id_producto = {id}";

                MySqlCommand cmd = new MySqlCommand(queryString, conn);

                DbDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    object comentarioDB = reader["comentario"];
                    string? comentario = comentarioDB == DBNull.Value ? null : comentarioDB.ToString();

                    double precio = Convert.ToDouble(reader["precio"]);

                    producto = new ProductosItemVM(
                        Convert.ToInt32(reader["id_producto"]),
                        Convert.ToInt32(reader["id_rubroP"]),
                        reader["rubro"].ToString(),
                        reader["nombre"].ToString(),
                        (float)precio,
                        Convert.ToInt32(reader["cantidad_restante"]),
                        Convert.ToInt32(reader["cantidad_descartada"]),
                        reader["imagen"].ToString(),
                        comentario
                    );
                }
                else
                    throw new KeyNotFoundException("No existe un producto con el ID proporcionado");

                conn.Close();
            }

            return producto;
        }

        public ProductosItemVM CreateProductItem(ProductosItemVM item)
        {
            return _ctx.LoadStoredProcedure("CrearProducto", _connectionString)
                .WithSqlParam("p_id_rubro", item.id_rubroP)
                .WithSqlParam("p_nombre", item.nombre)
                .WithSqlParam("p_precio", item.precio)
                .WithSqlParam("p_imagen", item.imagen)
                .WithSqlParam("p_comentario", item.comentario)
                .ExecuteSingleResultStoredProcedure<ProductosItemVM>();
        }

        public ProductosItemVM UpdateProductItem(ProductosItemVM item, List<string> attributesToChange)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionString.ConnectionString))
            {
                conn.Open();

                string queryString = "SELECT * FROM Productos WHERE nombre = @nombre";

                MySqlCommand cmd = new MySqlCommand(queryString, conn);
                cmd.Parameters.AddWithValue("@nombre", item.nombre);
                cmd.Prepare();

                DbDataReader reader = cmd.ExecuteReader();

                if (reader.Read() && attributesToChange.Find(attr => attr == "name") != null)
                {
                    string? nombre = reader["nombre"].ToString();
                    if (item.nombre == nombre)
                        throw new DuplicateWaitObjectException("El nombre del producto ya existe");
                }

                reader.Close();
            }

            return _ctx.LoadStoredProcedure("ActualizarProducto", _connectionString)
                .WithSqlParam("p_id", item.id_producto)
                .WithSqlParam("p_id_rubro", item.id_rubroP)
                .WithSqlParam("p_nombre", item.nombre)
                .WithSqlParam("p_precio", item.precio)
                .WithSqlParam("p_imagen", item.imagen)
                .WithSqlParam("p_comentario", item.comentario)
                .ExecuteSingleResultStoredProcedure<ProductosItemVM>();
        }

        public void DeleteProductItem(int id)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionString.ConnectionString))
            {
                conn.Open();

                string queryString = $"DELETE FROM Productos WHERE id_producto = {id}";

                MySqlCommand cmd = new MySqlCommand(queryString, conn);

                int rowsAffected = cmd.ExecuteNonQuery();

                conn.Close();
            }
        }
    }
}
