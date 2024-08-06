using BoerisCreaciones.Core;
using BoerisCreaciones.Core.Models.Socio;
using BoerisCreaciones.Repository.Interfaces;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoerisCreaciones.Repository.Repositories
{
    public class RolesSociosRepository : IRolesSociosRepository
    {
        private readonly ConnectionStringProvider _connectionStringProvider;
        private readonly BoerisCreacionesContext ctx;

        public RolesSociosRepository(ConnectionStringProvider connectionStringProvider, BoerisCreacionesContext context)
        {
            _connectionStringProvider = connectionStringProvider;
            ctx = context;
        }

        public List<TipoSocioVM> GetPartnerRoles(int id)
        {
            List<TipoSocioVM> rolesDeSocio = new List<TipoSocioVM>();

            using (MySqlConnection conn = new MySqlConnection(_connectionStringProvider.ConnectionString))
            {
                conn.Open();

                string queryString = "SELECT * FROM V_MostrarSocios WHERE id_usuario = @id";

                MySqlCommand cmd = new MySqlCommand(queryString, conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Prepare();

                DbDataReader reader = cmd.ExecuteReader();
                if (!reader.HasRows)
                    throw new Exception("No existe el socio con el ID especificado.");

                reader.Close();

                queryString = "SELECT * FROM V_MostrarRolesSocios WHERE id_usuario = @id";

                cmd = new MySqlCommand(queryString, conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Prepare();

                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int id_tipoSocio = Convert.ToInt32(reader["id_tipoSocio"]);
                    char tipoSocio = Convert.ToChar(reader["tipoSocio"]);
                    string? explicacion_rol = reader["explicacion_rol"].ToString();

                    rolesDeSocio.Add(new TipoSocioVM(id_tipoSocio, tipoSocio, explicacion_rol));
                }

                conn.Close();
            }

            return rolesDeSocio;
        }

        public List<TipoSocioVM> GetPossibleRoles()
        {
            List<TipoSocioVM> roles = new List<TipoSocioVM>();

            using (MySqlConnection conn = new MySqlConnection(_connectionStringProvider.ConnectionString))
            {
                conn.Open();
                string queryString = "SELECT * FROM TiposSocio";

                MySqlCommand cmd = new MySqlCommand(queryString, conn);
                DbDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    TipoSocioVM tipoSocio = new TipoSocioVM(
                        Convert.ToInt32(reader["id_tipoSocio"]),
                        Convert.ToChar(reader["tipoSocio"]),
                        reader["explicacion_rol"].ToString()
                    );
                    roles.Add(tipoSocio);
                }

                conn.Close();
            }

            return roles;
        }

        public void AssignRoles(int id, List<int> roles)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionStringProvider.ConnectionString))
            {
                conn.Open();
                string queryString = "INSERT INTO RolesSocios(id_usuario, id_tipoSocio) VALUES ";
                for (int i = 0; i < roles.Count; i++)
                {
                    int role = roles[i];
                    if (i == roles.Count - 1)
                        queryString += $"({id}, {role});";
                    else
                        queryString += $"({id}, {role}), ";
                }

                MySqlCommand cmd = new MySqlCommand(queryString, conn);

                int rowsAffected = cmd.ExecuteNonQuery();

                conn.Close();
            }
        }

        public void UpdateRoles(int id, List<int> roles)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionStringProvider.ConnectionString))
            {
                conn.Open();
                string queryString = $"DELETE FROM RolesSocios WHERE id_usuario = {id}";

                MySqlCommand cmd = new MySqlCommand(queryString, conn);
                cmd.ExecuteNonQuery();

                queryString = "INSERT INTO RolesSocios(id_usuario, id_tipoSocio) VALUES ";
                for (int i = 0; i < roles.Count; i++)
                {
                    int role = roles[i];
                    if (i == roles.Count - 1)
                        queryString += $"({id}, {role});";
                    else
                        queryString += $"({id}, {role}), ";
                }

                cmd = new MySqlCommand(queryString, conn);

                int rowsAffected = cmd.ExecuteNonQuery();

                conn.Close();
            }
        }

        public void DeleteRoles(int id)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionStringProvider.ConnectionString))
            {
                conn.Open();
                string queryString = $"DELETE FROM RolesSocios WHERE id_usuario = {id}";

                MySqlCommand cmd = new MySqlCommand(queryString, conn);
                cmd.ExecuteNonQuery();

                conn.Close();
            }
        }
    }
}
