using BoerisCreaciones.Core;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Reflection;
using MySql.Data.MySqlClient;

namespace BoerisCreaciones.Repository
{
    public static class DbUtils
    {
        public static MySqlCommand LoadStoredProcedure(this DbContext context, string storedProcedureName, ConnectionStringProvider _connectionStringProvider)
        {
            var conn = new MySqlConnection(_connectionStringProvider.ConnectionString);
            var cmd = conn.CreateCommand();
            cmd.CommandTimeout = 0;
            cmd.CommandText = storedProcedureName;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            return cmd;
        }

        public static MySqlCommand WithSqlParam(this MySqlCommand cmd, string paramName, object? paramValue)
        {
            if (string.IsNullOrEmpty(cmd.CommandText))
                throw new InvalidOperationException("Llamá a LoadStoredProcedure antes de usar este método");

            var param = cmd.CreateParameter();
            param.ParameterName = paramName;
            cmd.CommandTimeout = 0;
            param.Value = paramValue;
            cmd.Parameters.Add(param);
            return cmd;
        }

        public static List<T> ExecuteStoredProcedure<T>(this DbCommand command)
        {
            using (command)
            {
                command.CommandTimeout = 0;
                if (command.Connection.State == System.Data.ConnectionState.Closed)
                    command.Connection.Open();
                try
                {
                    using var reader = command.ExecuteReader();
                    return reader.MapToList<T>();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    command.Connection.Close();
                }
            }
        }

        public static T ExecuteSingleResultStoredProcedure<T>(this DbCommand command)
        {
            using (command)
            {
                if (command.Connection.State == System.Data.ConnectionState.Closed)
                    command.Connection.Open();
                command.CommandTimeout = 0;
                try
                {
                    using var reader = command.ExecuteReader();
                    return reader.MapToList<T>().FirstOrDefault();
                }
                finally
                {
                    command.Connection.Close();
                }
            }
        }

        public static string ExecuteStringStoredProcedure(this MySqlCommand command)
        {
            using (command)
            {
                if (command.Connection.State == System.Data.ConnectionState.Closed)
                    command.Connection.Open();
                command.CommandTimeout = 0;
                try
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return reader.GetString(0);
                        }
                        return "";
                    }
                }
                finally
                {
                    command.Connection.Close();
                }
            }
        }

        public static void ExecuteVoidStoredProcedure(this DbCommand command)
        {
            using (command)
            {
                if (command.Connection.State == System.Data.ConnectionState.Closed)
                    command.Connection.Open();
                command.CommandTimeout = 0;
                try
                {
                    using var reader = command.ExecuteReader();
                }
                finally
                {
                    command.Connection.Close();
                }
            }
        }

        private static List<T> MapToList<T>(this DbDataReader dr)
        {
            var objList = new List<T>();
            var props = typeof(T).GetRuntimeProperties();

            var colMapping = dr.GetColumnSchema()
                .Where(x => props.Any(y => y.Name.ToLower() == x.ColumnName.ToLower()))
                .ToDictionary(key => key.ColumnName.ToLower());

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    T obj = Activator.CreateInstance<T>();
                    foreach (var prop in props)
                    {
                        try
                        {
                            DbColumn colmap = colMapping[prop.Name.ToLower()];
                            int index = colmap.ColumnOrdinal.Value - 1;
                            var val = dr.GetValue(index);
                            if(val is string && val.ToString().Length == 1)
                            {
                                char c = val.ToString()[0];
                                prop.SetValue(obj, c);
                            }
                            else
                                prop.SetValue(obj, val == DBNull.Value ? null : val);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    objList.Add(obj);
                }
            }

            return objList;
        }
    }
}
