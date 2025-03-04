using BoerisCreaciones.Core;
using BoerisCreaciones.Core.Models.Compras;
using BoerisCreaciones.Core.Models.MateriasPrimas;
using BoerisCreaciones.Repository.Interfaces;
using MySql.Data.MySqlClient;

namespace BoerisCreaciones.Repository.Repositories
{
    public class ComprasRepository : IComprasRepository
    {
        private readonly ConnectionStringProvider _connectionStringProvider;
        private readonly IUsuariosRepository _repositorySocios;
        private readonly ICatalogoProveedoresRepository _repositoryProveedores;
        private readonly ICatalogoMateriasPrimasRepository _materiasPrimasRepository;
        private readonly ICatalogoSucursalesRepository _repositorySucursales;

        public ComprasRepository(ConnectionStringProvider connectionStringProvider, IUsuariosRepository repositorySocios, ICatalogoProveedoresRepository repositoryProveedores, ICatalogoMateriasPrimasRepository materiasPrimasRepository, ICatalogoSucursalesRepository repositorySucursales)
        {
            _connectionStringProvider = connectionStringProvider;
            _repositorySocios = repositorySocios;
            _repositoryProveedores = repositoryProveedores;
            _materiasPrimasRepository = materiasPrimasRepository;
            _repositorySucursales = repositorySucursales;
        }

        public List<CompraVM> GetPurchases()
        {
            List<CompraVM> comprasVM = new List<CompraVM>();

            using (MySqlConnection connection = new MySqlConnection(_connectionStringProvider.ConnectionString))
            {
                connection.Open();
                string query = "SELECT * FROM V_ListarCompras";
                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    object fechaRecepcionDB = reader["fecha_recepcion"];
                    object fechaCanceladoDB = reader["fecha_cancelado"];

                    object precioDB = reader["precio"];
                    object facturaDB = reader["factura"];

                    CompraVM compraVM = new CompraVM(
                        reader.GetInt32("id_compra"),
                        _repositorySocios.GetUserById(reader.GetInt32("id_socio")),
                        _repositoryProveedores.GetProvider(reader.GetInt32("id_proveedor")),
                        reader["descripcion"].ToString(),
                        reader.GetDateTime("fecha_pedido"),
                        fechaRecepcionDB == DBNull.Value ? null : (DateTime?)fechaRecepcionDB,
                        fechaCanceladoDB == DBNull.Value ? null : (DateTime?)fechaCanceladoDB,
                        reader["moneda"].ToString(),
                        reader.GetChar("tipo_pago"),
                        reader.GetChar("modo_recepcion"),
                        reader.GetChar("estado"),
                        _repositorySucursales.GetById(reader.GetInt32("id_sucursal")),
                        precioDB == DBNull.Value ? null : (float)Convert.ToDouble(precioDB),
                        facturaDB == DBNull.Value ? null : reader["factura"].ToString()
                    );

                    comprasVM.Add(compraVM);
                }
            }

            return comprasVM;
        }

        public CompraVM GetPurchaseById(int idPurchase)
        {
            CompraVM compra = null;

            using (MySqlConnection connection = new MySqlConnection(_connectionStringProvider.ConnectionString))
            {
                connection.Open();
                string query = "SELECT * FROM V_ListarCompras WHERE id_compra = @id";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", idPurchase);
                command.Prepare();
                MySqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    object fechaRecepcionDB = reader["fecha_recepcion"];
                    object fechaCanceladoDB = reader["fecha_cancelado"];

                    object precioDB = reader["precio"];
                    object facturaDB = reader["factura"];

                    compra = new CompraVM(
                        reader.GetInt32("id_compra"),
                        _repositorySocios.GetUserById(reader.GetInt32("id_socio")),
                        _repositoryProveedores.GetProvider(reader.GetInt32("id_proveedor")),
                        reader["descripcion"].ToString(),
                        reader.GetDateTime("fecha_pedido"),
                        fechaRecepcionDB == DBNull.Value ? null : (DateTime?)fechaRecepcionDB,
                        fechaCanceladoDB == DBNull.Value ? null : (DateTime?)fechaCanceladoDB,
                        reader["moneda"].ToString(),
                        reader.GetChar("tipo_pago"),
                        reader.GetChar("modo_recepcion"),
                        reader.GetChar("estado"),
                        _repositorySucursales.GetById(reader.GetInt32("id_sucursal")),
                        precioDB == DBNull.Value ? null : (float)Convert.ToDouble(precioDB),
                        facturaDB == DBNull.Value ? null : reader["factura"].ToString()
                    );
                }
            }

            return compra;
        }

        public List<CompraVM> GetPurchasesByPartner(int idPartner)
        {
            List<CompraVM> compras = new List<CompraVM>();

            using (MySqlConnection connection = new MySqlConnection(_connectionStringProvider.ConnectionString))
            {
                connection.Open();
                string query = "SELECT * FROM V_ListarCompras WHERE id_socio = @id";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", idPartner);
                command.Prepare();
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    object fechaRecepcionDB = reader["fecha_recepcion"];
                    object fechaCanceladoDB = reader["fecha_cancelado"];

                    object precioDB = reader["precio"];
                    object facturaDB = reader["factura"];

                    CompraVM compra = new CompraVM(
                        reader.GetInt32("id_compra"),
                        _repositorySocios.GetUserById(reader.GetInt32("id_socio")),
                        _repositoryProveedores.GetProvider(reader.GetInt32("id_proveedor")),
                        reader["descripcion"].ToString(),
                        reader.GetDateTime("fecha_pedido"),
                        fechaRecepcionDB == DBNull.Value ? null : (DateTime?)fechaRecepcionDB,
                        fechaCanceladoDB == DBNull.Value ? null : (DateTime?)fechaCanceladoDB,
                        reader["moneda"].ToString(),
                        reader.GetChar("tipo_pago"),
                        reader.GetChar("modo_recepcion"),
                        reader.GetChar("estado"),
                        _repositorySucursales.GetById(reader.GetInt32("id_sucursal")),
                        precioDB == DBNull.Value ? null : (float)Convert.ToDouble(precioDB),
                        facturaDB == DBNull.Value ? null : reader["factura"].ToString()
                    );

                    compras.Add(compra);
                }
            }

            return compras;
        }

        public List<MateriaPrimaCompraVM> GetPurchasedRawMaterials(int idPurchase)
        {
            List<MateriaPrimaCompraVM> materiasPrimas = new List<MateriaPrimaCompraVM>();

            using (MySqlConnection connection = new MySqlConnection(_connectionStringProvider.ConnectionString))
            {
                connection.Open();
                string query = "SELECT * FROM V_ListarMateriasPrimasCompradas WHERE id_compra = @id";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", idPurchase);
                command.Prepare();
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    MateriaPrimaVM materiaPrima = _materiasPrimasRepository.GetRawMaterialsItem(reader.GetInt32("id_matP"));

                    MateriaPrimaCompraVM purchasedMateriaPrima = new MateriaPrimaCompraVM(
                        materiaPrima.id_matP,
                        materiaPrima.id_rubroMP,
                        materiaPrima.rubro,
                        materiaPrima.nombre,
                        reader.GetInt32("cantidad"),
                        reader.GetFloat("precio_unitario")
                    );

                    materiasPrimas.Add(purchasedMateriaPrima);
                }
            }

            return materiasPrimas;
        }

        public CompraVM AddPurchase(NuevaCompraDTO newPurchase)
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionStringProvider.ConnectionString))
            {
                connection.Open();
                string query = "INSERT INTO ComprasMateriasPrimas (id_socio, id_proveedor, descripcion, fecha_pedido, moneda, tipo_pago, modo_recepcion, estado) VALUES (@id_socio, @id_proveedor, @descripcion, @fecha_pedido, @moneda, @tipo_pago, @modo_recepcion, @estado)";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@id_socio", newPurchase.partner.id_user);
                command.Parameters.AddWithValue("@id_proveedor", newPurchase.provider.id);
                command.Parameters.AddWithValue("@descripcion", newPurchase.description);
                command.Parameters.AddWithValue("@fecha_pedido", DateTime.Now);
                command.Parameters.AddWithValue("@moneda", newPurchase.currency);
                command.Parameters.AddWithValue("@tipo_pago", newPurchase.payment_type);
                command.Parameters.AddWithValue("@modo_recepcion", newPurchase.reception_mode);
                command.Parameters.AddWithValue("@estado", 'P');
                command.Prepare();
                command.ExecuteNonQuery();
            }

            int idCompra = 0;

            using (MySqlConnection connection = new MySqlConnection(_connectionStringProvider.ConnectionString))
            {
                connection.Open();
                string query = "SELECT MAX(id_compra) FROM ComprasMateriasPrimas";
                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                    idCompra = reader.GetInt32(0);
            }

            foreach (MateriaPrimaCompraDTO materiaPrima in newPurchase.raw_materials)
            {
                using (MySqlConnection connection = new MySqlConnection(_connectionStringProvider.ConnectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO LineasCompras (id_compra, id_matP, cantidad, precio_unitario) VALUES (@id_compra, @id_matP, @cantidad, @precio)";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@id_compra", idCompra);
                    command.Parameters.AddWithValue("@id_matP", materiaPrima.raw_material_id);
                    command.Parameters.AddWithValue("@cantidad", materiaPrima.quantity);
                    command.Parameters.AddWithValue("@precio", materiaPrima.unit_price);
                    command.Prepare();
                    command.ExecuteNonQuery();
                }
            }

            return GetPurchaseById(idCompra);
        }

        public void ReceivePurchase(int idPurchase, int idUser, int idBranch)
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionStringProvider.ConnectionString))
            {
                connection.Open();
                string query = "UPDATE ComprasMateriasPrimas SET estado = 'R', fecha_recepcion = @fecha WHERE id_compra = @id";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@fecha", DateTime.Now);
                command.Parameters.AddWithValue("@id", idPurchase);
                command.Prepare();
                command.ExecuteNonQuery();
            }
            
            CompraVM compra = GetPurchaseById(idPurchase);

            using (MySqlConnection connection = new MySqlConnection(_connectionStringProvider.ConnectionString))
            {
                connection.Open();
                string query = "INSERT INTO EntradasMP (id_usuario, razon, fecha) VALUES (@id_usuario, 'Compra @id_compra: @descripcion', @fecha)";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@id_usuario", idUser);
                command.Parameters.AddWithValue("@id_compra", idPurchase);
                command.Parameters.AddWithValue("@descripcion", compra.descripcion);
                command.Parameters.AddWithValue("@fecha", DateTime.Now);
                command.Prepare();
                command.ExecuteNonQuery();
            }

            int idEntrada = 0;

            using (MySqlConnection connection = new MySqlConnection(_connectionStringProvider.ConnectionString))
            {
                connection.Open();
                string query = "SELECT MAX(id_entradaMP) FROM EntradasMP";
                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    idEntrada = reader.GetInt32(0);
                }
            }

            List<MateriaPrimaCompraVM> materiasPrimasCompradas = GetPurchasedRawMaterials(idPurchase);

            foreach (MateriaPrimaCompraVM materiaPrima in materiasPrimasCompradas)
            {
                using (MySqlConnection connection = new MySqlConnection(_connectionStringProvider.ConnectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO LineasEntradasMP (id_entradaMP, id_matP, cantidad) VALUES (@id, @id_matP, @cantidad)";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@id", idEntrada);
                    command.Parameters.AddWithValue("@id_matP", materiaPrima.id_matP);
                    command.Parameters.AddWithValue("@cantidad", materiaPrima.cantidad);
                    command.Prepare();
                    command.ExecuteNonQuery();
                }
            }

            using (MySqlConnection connection = new MySqlConnection(_connectionStringProvider.ConnectionString))
            {
                connection.Open();
                string query = "UPDATE MateriasPrimas SET cantidad_restante = cantidad_restante + @cantidad WHERE id_matP = @id";
                MySqlCommand command = new MySqlCommand(query, connection);
                foreach (MateriaPrimaCompraVM materiaPrima in materiasPrimasCompradas)
                {
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@id", materiaPrima.id_matP);
                    command.Parameters.AddWithValue("@cantidad", materiaPrima.cantidad);
                    command.Prepare();
                    command.ExecuteNonQuery();
                }
            }
        }

        public void CancelPurchase(int idPurchase)
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionStringProvider.ConnectionString))
            {
                connection.Open();
                string query = "UPDATE ComprasMateriasPrimas SET estado = 'C', fecha_cancelado = @fecha WHERE id_compra = @id";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", idPurchase);
                command.Parameters.AddWithValue("@fecha", DateTime.Now);
                command.Prepare();
                command.ExecuteNonQuery();
            }

            using (MySqlConnection connection = new MySqlConnection(_connectionStringProvider.ConnectionString))
            {
                connection.Open();
                string query = "DELETE FROM LineasCompras WHERE id_compra = @id";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", idPurchase);
                command.Prepare();
                command.ExecuteNonQuery();
            }
        }

        public void DisablePurchase(int idPurchase)
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionStringProvider.ConnectionString))
            {
                connection.Open();
                string query = "UPDATE ComprasMateriasPrimas SET estado = 'I' WHERE id_compra = @id";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", idPurchase);
                command.Prepare();
                command.ExecuteNonQuery();
            }
        }

        public void DeletePurchase(int idPurchase)
        {
            // Primero debe descontarse la cantidad de materias primas compradas en caso de que la compra haya sido recibida
            CompraVM compra = GetPurchaseById(idPurchase);
            if(compra.fecha_recepcion != null)
            {
                List<MateriaPrimaCompraVM> materiasPrimasCompradas = GetPurchasedRawMaterials(idPurchase);
                using (MySqlConnection connection = new MySqlConnection(_connectionStringProvider.ConnectionString))
                {
                    connection.Open();
                    string query = "UPDATE MateriasPrimas SET cantidad_restante = cantidad_restante - @cantidad WHERE id_matP = @id";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    foreach (MateriaPrimaCompraVM materiaPrima in materiasPrimasCompradas)
                    {
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@id", materiaPrima.id_matP);
                        command.Parameters.AddWithValue("@cantidad", materiaPrima.cantidad);
                        command.Prepare();
                        command.ExecuteNonQuery();
                    }
                }

                // Luego, se eliminan las entradas de materias primas
                using (MySqlConnection connection = new MySqlConnection(_connectionStringProvider.ConnectionString))
                {
                    connection.Open();
                    string query = "DELETE FROM LineasEntradasMP WHERE id_entradaMP = (SELECT id_entradaMP FROM EntradasMP WHERE razon = 'Compra @id_compra: @descripcion')";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@id_compra", idPurchase);
                    command.Parameters.AddWithValue("@descripcion", compra.descripcion);
                    command.Prepare();
                    command.ExecuteNonQuery();
                }

                using (MySqlConnection connection = new MySqlConnection(_connectionStringProvider.ConnectionString))
                {
                    connection.Open();
                    string query = "DELETE FROM EntradasMP WHERE razon = 'Compra @id_compra: @descripcion'";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@id_compra", idPurchase);
                    command.Parameters.AddWithValue("@descripcion", compra.descripcion);
                    command.Prepare();
                    command.ExecuteNonQuery();
                }
            }

            // Luego, deben eliminarse los elementos de la tabla LineasCompras
            using (MySqlConnection connection = new MySqlConnection(_connectionStringProvider.ConnectionString))
            {
                connection.Open();
                string query = "DELETE FROM LineasCompras WHERE id_compra = @id";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", idPurchase);
                command.Prepare();
                command.ExecuteNonQuery();
            }

            // Por último, se elimina la compra
            using (MySqlConnection connection = new MySqlConnection(_connectionStringProvider.ConnectionString))
            {
                connection.Open();
                string query = "DELETE FROM ComprasMateriasPrimas WHERE id_compra = @id";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", idPurchase);
                command.Prepare();
                command.ExecuteNonQuery();
            }
        }
    }
}
