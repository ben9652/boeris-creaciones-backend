using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoerisCreaciones.Core
{
    public class ConnectionStringProvider
    {
        public string ConnectionString { get; set; }

        public ConnectionStringProvider(string connectionString)
        {
            ConnectionString = connectionString;
        }
    }
}
