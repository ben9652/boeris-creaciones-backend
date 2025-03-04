using BoerisCreaciones.Core;
using BoerisCreaciones.Core.Models;
using BoerisCreaciones.Core.Models.Compras;
using BoerisCreaciones.Core.Models.MateriasPrimas;
using BoerisCreaciones.Core.Models.Proveedores;
using BoerisCreaciones.Core.Models.Sucursales;
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

        public List<CompraVM> GetPurchases(List<char> filters, BusquedaCompra? search, string? orderCriteria, bool ascendingSort)
        {
            List<CompraVM> comprasVM = new List<CompraVM>();

            using (MySqlConnection connection = new MySqlConnection(_connectionStringProvider.ConnectionString))
            {
                connection.Open();
                string query = "SELECT * FROM V_ListarCompras";
                List<MySqlParameter> parameters = new List<MySqlParameter>();

                if (
                    search != null
                )
                {
                    if (
                        search.key.CompareTo("proveedor") != 0 &&
                        search.key.CompareTo("socio") != 0 &&
                        search.key.CompareTo("sucursal") != 0
                    )
                    {
                        if (search.key.CompareTo("fecha") == 0)
                        {
                            DateTime endDate;
                            if (DateTime.TryParseExact(search.name, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out endDate))
                            {
                                query += " WHERE (fecha_pedido <= @endDate OR fecha_recepcion <= @endDate OR fecha_cancelado <= @endDate)";
                                parameters.Add(new MySqlParameter("@endDate", endDate));
                            }
                            else
                            {
                                throw new ArgumentException("Formato de fecha inválido. Por favor, usar dd/MM/yyyy.");
                            }
                        }
                        else if (search.key.CompareTo("presupuesto") == 0)
                        {
                            query += " WHERE presupuesto <= @presupuesto";
                            parameters.Add(new MySqlParameter("@presupuesto", float.Parse(search.name)));
                        }
                        else
                        {
                            query += " WHERE " + search.key + " LIKE @searchName";
                            parameters.Add(new MySqlParameter("@searchName", "%" + search.name + "%"));
                        }
                    }
                    else
                    {
                        if (search.key.CompareTo("proveedor") == 0)
                        {
                            query += " WHERE id_proveedor IN (SELECT id_proveedor FROM Proveedores WHERE nombre LIKE @searchName)";
                            parameters.Add(new MySqlParameter("@searchName", "%" + search.name + "%"));
                        }
                        if (search.key.CompareTo("socio") == 0)
                        {
                            query += " WHERE id_socio IN (SELECT id_usuario FROM Usuarios WHERE nombre LIKE @searchName)";
                            parameters.Add(new MySqlParameter("@searchName", "%" + search.name + "%"));
                        }
                        if (search.key.CompareTo("sucursal") == 0)
                        {
                            query += " WHERE id_sucursal IN (SELECT id_sucursal FROM Sucursales WHERE nombre LIKE @searchName)";
                            parameters.Add(new MySqlParameter("@searchName", "%" + search.name + "%"));
                        }
                    }
                }

                if (filters.Count > 0)
                {
                    if (search != null)
                        query += " AND ";
                    else
                        query += " WHERE ";
                    query += "estado IN (";
                    for (int i = 0; i < filters.Count; i++)
                    {
                        string paramName = "@filter" + i;
                        query += paramName;
                        if (i < filters.Count - 1)
                            query += ", ";
                        parameters.Add(new MySqlParameter(paramName, filters[i]));
                    }
                    query += ")";
                }

                if (
                    orderCriteria != null &&
                    orderCriteria.CompareTo("proveedor") != 0 &&
                    orderCriteria.CompareTo("socio") != 0 &&
                    orderCriteria.CompareTo("sucursal") != 0
                )
                {
                    if (orderCriteria.Contains("fecha"))
                    {
                        if (orderCriteria.Contains("pedido"))
                            query += " ORDER BY CASE estado WHEN 'P' THEN 1 WHEN 'R' THEN 2 WHEN 'C' THEN 3 END, fecha_pedido";
                        else if (orderCriteria.Contains("recepcion"))
                            query += " ORDER BY CASE estado WHEN 'R' THEN 1 WHEN 'P' THEN 2 WHEN 'C' THEN 3 END, fecha_recepcion";
                        else if (orderCriteria.Contains("cancelado"))
                            query += " ORDER BY CASE estado WHEN 'C' THEN 1 WHEN 'R' THEN 2 WHEN 'P' THEN 3 END, fecha_cancelado";
                    }
                    else
                        query += " ORDER BY " + orderCriteria;

                    if (!ascendingSort)
                        query += " DESC";
                }

                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddRange(parameters.ToArray());
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    object fechaRecepcionDB = reader["fecha_recepcion"];
                    object fechaCanceladoDB = reader["fecha_cancelado"];

                    object precioFinalDB = reader["precio_final"];
                    object razonMontoAdicional = reader["razon_monto_adicional"];
                    object facturaDB = reader["factura"];

                    object idSucursalDB = reader["id_sucursal"];
                    int id_sucursal = idSucursalDB == DBNull.Value ? 0 : reader.GetInt32("id_sucursal");

                    int id = reader.GetInt32("id_compra");
                    UsuarioVM socio = _repositorySocios.GetUserById(reader.GetInt32("id_socio"));
                    ProveedorVM proveedor = _repositoryProveedores.GetProvider(reader.GetInt32("id_proveedor"));
                    string? descripcion = reader["descripcion"].ToString();
                    DateTime fecha_pedido = reader.GetDateTime("fecha_pedido");
                    string? moneda = reader["moneda"].ToString();
                    char tipo_pago = reader.GetChar("tipo_pago");
                    char modo_recepcion = reader.GetChar("modo_recepcion");
                    float presupuesto = reader.GetFloat("presupuesto");
                    char estado = reader.GetChar("estado");

                    SucursalVM? sucursal = null;
                    if (id_sucursal != 0)
                        sucursal = _repositorySucursales.GetById(id_sucursal);

                    CompraVM compra = new CompraVM(
                        id,
                        socio,
                        proveedor,
                        descripcion,
                        fecha_pedido,
                        fechaRecepcionDB == DBNull.Value ? null : (DateTime?)fechaRecepcionDB,
                        fechaCanceladoDB == DBNull.Value ? null : (DateTime?)fechaCanceladoDB,
                        moneda,
                        tipo_pago,
                        modo_recepcion,
                        presupuesto,
                        estado,
                        precioFinalDB == DBNull.Value ? null : reader.GetFloat("precio_final"),
                        razonMontoAdicional == DBNull.Value ? null : razonMontoAdicional.ToString(),
                        sucursal,
                        facturaDB == DBNull.Value ? null : facturaDB.ToString()
                    );

                    comprasVM.Add(compra);
                }
            }

            if (orderCriteria != null)
            {
                if (orderCriteria.CompareTo("proveedor") == 0)
                {
                    if (ascendingSort)
                        comprasVM.Sort((c1, c2) => c1.proveedor.nombre.CompareTo(c2.proveedor.nombre));
                    else
                        comprasVM.Sort((c1, c2) => c2.proveedor.nombre.CompareTo(c1.proveedor.nombre));
                }

                if (orderCriteria.CompareTo("socio") == 0)
                {
                    if (ascendingSort)
                        comprasVM.Sort((c1, c2) => c1.socio_que_hizo_pedido.nombre.CompareTo(c2.socio_que_hizo_pedido.nombre));
                    else
                        comprasVM.Sort((c1, c2) => c2.socio_que_hizo_pedido.nombre.CompareTo(c1.socio_que_hizo_pedido.nombre));
                }

                if (orderCriteria.CompareTo("sucursal") == 0)
                {
                    if (ascendingSort)
                        comprasVM.Sort((c1, c2) => c1.sucursal?.nombre.CompareTo(c2.sucursal?.nombre) ?? 1);
                    else
                        comprasVM.Sort((c1, c2) => c2.sucursal?.nombre.CompareTo(c1.sucursal?.nombre) ?? 1);
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

                    object precioFinalDB = reader["precio_final"];
                    object razonMontoAdicional = reader["razon_monto_adicional"];
                    object facturaDB = reader["factura"];

                    object idSucursalDB = reader["id_sucursal"];
                    int id_sucursal = idSucursalDB == DBNull.Value ? 0 : reader.GetInt32("id_sucursal");

                    int id = reader.GetInt32("id_compra");
                    UsuarioVM socio = _repositorySocios.GetUserById(reader.GetInt32("id_socio"));
                    ProveedorVM proveedor = _repositoryProveedores.GetProvider(reader.GetInt32("id_proveedor"));
                    string? descripcion = reader["descripcion"].ToString();
                    DateTime fecha_pedido = reader.GetDateTime("fecha_pedido");
                    string? moneda = reader["moneda"].ToString();
                    char tipo_pago = reader.GetChar("tipo_pago");
                    char modo_recepcion = reader.GetChar("modo_recepcion");
                    float presupuesto = reader.GetFloat("presupuesto");
                    char estado = reader.GetChar("estado");

                    SucursalVM? sucursal = null;
                    if(id_sucursal != 0)
                        sucursal = _repositorySucursales.GetById(id_sucursal);

                    compra = new CompraVM(
                        id,
                        socio,
                        proveedor,
                        descripcion,
                        fecha_pedido,
                        fechaRecepcionDB == DBNull.Value ? null : (DateTime?)fechaRecepcionDB,
                        fechaCanceladoDB == DBNull.Value ? null : (DateTime?)fechaCanceladoDB,
                        moneda,
                        tipo_pago,
                        modo_recepcion,
                        presupuesto,
                        estado,
                        precioFinalDB == DBNull.Value ? null : reader.GetFloat("precio_final"),
                        razonMontoAdicional == DBNull.Value ? null : razonMontoAdicional.ToString(),
                        sucursal,
                        facturaDB == DBNull.Value ? null : facturaDB.ToString()
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

                    object precioFinalDB = reader["precio_final"];
                    object razonMontoAdicional = reader["razon_monto_adicional"];
                    object facturaDB = reader["factura"];

                    object idSucursalDB = reader["id_sucursal"];
                    int id_sucursal = idSucursalDB == DBNull.Value ? 0 : reader.GetInt32("id_sucursal");

                    int id = reader.GetInt32("id_compra");
                    UsuarioVM socio = _repositorySocios.GetUserById(reader.GetInt32("id_socio"));
                    ProveedorVM proveedor = _repositoryProveedores.GetProvider(reader.GetInt32("id_proveedor"));
                    string? descripcion = reader["descripcion"].ToString();
                    DateTime fecha_pedido = reader.GetDateTime("fecha_pedido");
                    string? moneda = reader["moneda"].ToString();
                    char tipo_pago = reader.GetChar("tipo_pago");
                    char modo_recepcion = reader.GetChar("modo_recepcion");
                    float presupuesto = reader.GetFloat("presupuesto");
                    char estado = reader.GetChar("estado");

                    SucursalVM? sucursal = null;
                    if (id_sucursal != 0)
                        sucursal = _repositorySucursales.GetById(id_sucursal);

                    CompraVM compra = new CompraVM(
                        id,
                        socio,
                        proveedor,
                        descripcion,
                        fecha_pedido,
                        fechaRecepcionDB == DBNull.Value ? null : (DateTime?)fechaRecepcionDB,
                        fechaCanceladoDB == DBNull.Value ? null : (DateTime?)fechaCanceladoDB,
                        moneda,
                        tipo_pago,
                        modo_recepcion,
                        presupuesto,
                        estado,
                        precioFinalDB == DBNull.Value ? null : reader.GetFloat("precio_final"),
                        razonMontoAdicional == DBNull.Value ? null : razonMontoAdicional.ToString(),
                        sucursal,
                        facturaDB == DBNull.Value ? null : facturaDB.ToString()
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

        public CompraVM AddPurchase(NuevaCompra newPurchase)
        {
            float totalPrice = newPurchase.raw_materials.Sum(m => m.quantity * m.unit_price);

            using (MySqlConnection connection = new MySqlConnection(_connectionStringProvider.ConnectionString))
            {
                connection.Open();
                string query = "INSERT INTO ComprasMateriasPrimas (id_socio, id_proveedor, descripcion, fecha_pedido, moneda, tipo_pago, modo_recepcion, presupuesto, estado) VALUES (@id_socio, @id_proveedor, @descripcion, @fecha_pedido, @moneda, @tipo_pago, @modo_recepcion, @presupuesto, @estado)";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@id_socio", newPurchase.partner.id_user);
                command.Parameters.AddWithValue("@id_proveedor", newPurchase.provider.id);
                command.Parameters.AddWithValue("@descripcion", newPurchase.description);
                command.Parameters.AddWithValue("@fecha_pedido", DateTime.Now);
                command.Parameters.AddWithValue("@moneda", newPurchase.currency);
                command.Parameters.AddWithValue("@tipo_pago", newPurchase.payment_type);
                command.Parameters.AddWithValue("@modo_recepcion", newPurchase.reception_mode);
                command.Parameters.AddWithValue("@presupuesto", totalPrice);
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

        public void ReceivePurchase(int idPurchase, int idUser, RecepcionCompra purchaseReception)
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionStringProvider.ConnectionString))
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand($"SELECT * FROM Socios WHERE id_usuario = {idUser}", connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                            throw new Exception("El usuario que quiere recibir la compra no es ni un socio");
                    }
                }

                float presupuesto = 0;
                using (MySqlCommand selectCommand = new MySqlCommand($"SELECT presupuesto FROM ComprasMateriasPrimas WHERE id_compra = {idPurchase}", connection))
                {
                    using(MySqlDataReader reader = selectCommand.ExecuteReader())
                    {
                        if (reader.Read())
                            presupuesto = reader.GetFloat(0);
                    }
                }

                string query = "UPDATE ComprasMateriasPrimas SET estado = 'R', fecha_recepcion = @fecha, precio_final = @precio, razon_monto_adicional = @razon, factura = @factura, id_sucursal = @id_sucursal WHERE id_compra = @id";
                using (MySqlCommand updateCommand = new MySqlCommand(query, connection))
                {
                    updateCommand.Parameters.AddWithValue("@fecha", DateTime.Now);
                    updateCommand.Parameters.AddWithValue("@precio", presupuesto + purchaseReception.additional_amount);
                    updateCommand.Parameters.AddWithValue("@razon", purchaseReception.additional_amount_reason);
                    updateCommand.Parameters.AddWithValue("@factura", purchaseReception.invoice);
                    updateCommand.Parameters.AddWithValue("@id_sucursal", purchaseReception.id_branch_reception);
                    updateCommand.Parameters.AddWithValue("@id", idPurchase);
                    updateCommand.Prepare();
                    updateCommand.ExecuteNonQuery();
                }
            }
            
            using (MySqlConnection connection = new MySqlConnection(_connectionStringProvider.ConnectionString))
            {
                connection.Open();
                string query = $"INSERT INTO EntradasMP (id_usuario, razon, fecha) VALUES (@id_usuario, 'c({idPurchase})', @fecha)";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@id_usuario", idUser);
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

            using (MySqlConnection connection = new MySqlConnection(_connectionStringProvider.ConnectionString))
            {
                connection.Open();
                string query = "INSERT INTO LineasEntradasMP (id_entradaMP, id_matP, cantidad) VALUES (@id, @id_matP, @cantidad)";
                MySqlCommand command = new MySqlCommand(query, connection);
                foreach (MateriaPrimaCompraVM materiaPrima in materiasPrimasCompradas)
                {
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@id", idEntrada);
                    command.Parameters.AddWithValue("@id_matP", materiaPrima.id_matP);
                    command.Parameters.AddWithValue("@cantidad", materiaPrima.cantidad);
                    command.Prepare();
                    command.ExecuteNonQuery();
                }
            }

            List<MateriaPrimaCompraVM> materiasPrimasCompradasNoContables = materiasPrimasCompradas.FindAll(m => m.cantidad == 0);
            List<MateriaPrimaCompraVM> materiasPrimasCompradasContables = materiasPrimasCompradas.FindAll(m => m.cantidad != 0);

            using (MySqlConnection connection = new MySqlConnection(_connectionStringProvider.ConnectionString))
            {
                connection.Open();
                string query = "UPDATE MateriasPrimas SET cantidad_restante = cantidad_restante + @cantidad WHERE id_matP = @id";
                MySqlCommand command = new MySqlCommand(query, connection);
                foreach (MateriaPrimaCompraVM materiaPrima in materiasPrimasCompradasContables)
                {
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@id", materiaPrima.id_matP);
                    command.Parameters.AddWithValue("@cantidad", materiaPrima.cantidad);
                    command.Prepare();
                    command.ExecuteNonQuery();
                }

                query = "UPDATE MateriasPrimas SET cantidad_restante = 1 WHERE id_matP = @id";
                command = new MySqlCommand(query, connection);
                foreach (MateriaPrimaCompraVM materiaPrima in materiasPrimasCompradasNoContables)
                {
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@id", materiaPrima.id_matP);
                    command.Prepare();
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }

            using (MySqlConnection connection = new MySqlConnection(_connectionStringProvider.ConnectionString))
            {
                connection.Open();
                foreach (MateriaPrimaCompraVM materiaPrima in materiasPrimasCompradas)
                {
                    string query = "SELECT * FROM AlojamientosMP WHERE id_sucursal = @id_sucursal AND id_matP = @id_matP";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@id_sucursal", purchaseReception.id_branch_reception);
                    command.Parameters.AddWithValue("@id_matP", materiaPrima.id_matP);
                    command.Prepare();
                    MySqlDataReader reader = command.ExecuteReader();

                    bool hayAlojamientosHechosParaEstaMateriaPrima = reader.Read();

                    reader.Close();

                    if (materiaPrima.cantidad != 0)
                    {
                        if (hayAlojamientosHechosParaEstaMateriaPrima)
                        {
                            query = "UPDATE AlojamientosMP SET cantidad = cantidad + @cantidad WHERE id_sucursal = @id_sucursal AND id_matP = @id_matP";
                            command = new MySqlCommand(query, connection);
                            command.Parameters.AddWithValue("@id_sucursal", purchaseReception.id_branch_reception);
                            command.Parameters.AddWithValue("@id_matP", materiaPrima.id_matP);
                            command.Parameters.AddWithValue("@cantidad", materiaPrima.cantidad);
                            command.Prepare();
                            command.ExecuteNonQuery();
                        }
                        else
                        {
                            query = "INSERT INTO AlojamientosMP (id_sucursal, id_matP, cantidad) VALUES (@id_sucursal, @id_matP, @cantidad)";
                            command = new MySqlCommand(query, connection);
                            command.Parameters.AddWithValue("@id_sucursal", purchaseReception.id_branch_reception);
                            command.Parameters.AddWithValue("@id_matP", materiaPrima.id_matP);
                            command.Parameters.AddWithValue("@cantidad", materiaPrima.cantidad);
                            command.Prepare();
                            command.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        query = "UPDATE AlojamientosMP SET cantidad = 1 WHERE id_sucursal = @id_sucursal AND id_matP = @id_matP";
                        command = new MySqlCommand(query, connection);
                        command.Parameters.AddWithValue("@id_sucursal", purchaseReception.id_branch_reception);
                        command.Parameters.AddWithValue("@id_matP", materiaPrima.id_matP);
                        command.Prepare();
                        command.ExecuteNonQuery();
                    }
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
