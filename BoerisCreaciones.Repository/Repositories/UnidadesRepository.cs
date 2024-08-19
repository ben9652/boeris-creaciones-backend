using BoerisCreaciones.Core;
using BoerisCreaciones.Core.Models.Unidades;
using BoerisCreaciones.Repository.Interfaces;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoerisCreaciones.Repository.Repositories
{
    public class UnidadesRepository : IUnidadesRepository
    {
        private readonly ConnectionStringProvider _connectionStringProvider;

        public UnidadesRepository(ConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }

        public List<UnidadVM> GetUnits()
        {
            List<UnidadVM> units = new List<UnidadVM>();

            using (MySqlConnection conn = new MySqlConnection(_connectionStringProvider.ConnectionString))
            {
                conn.Open();

                string queryString = "SELECT * FROM Unidades";

                MySqlCommand cmd = new MySqlCommand(queryString, conn);

                DbDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    UnidadVM unit = new UnidadVM(Convert.ToInt32(reader["id_unidad"]), reader["nombre"].ToString());
                    units.Add(unit);
                }

                conn.Close();
            }

            return units;
        }
        public UnidadVM GetUnit(int id)
        {
            UnidadVM unit;

            using (MySqlConnection conn = new MySqlConnection(_connectionStringProvider.ConnectionString))
            {
                conn.Open();

                string queryString = $"SELECT * FROM Unidades WHERE id_unidad = {id}";

                MySqlCommand cmd = new MySqlCommand(queryString, conn);

                DbDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                    unit = new UnidadVM(Convert.ToInt32(reader["id_unidad"]), reader["nombre"].ToString());
                else
                    throw new KeyNotFoundException("No existe la UnidadVM con el ID especificado");

                conn.Close();
            }

            return unit;
        }
    }
}
