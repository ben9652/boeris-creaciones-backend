namespace BoerisCreaciones.Api
{
    using System;
    using System.IO;

    public static class DotEnv
    {
        public static bool CheckEnvVars()
        {
            List<string> vars = new List<string>()
            {
                "MYSQL__DATABASE__SERVER",
                "MYSQL__DATABASE__USER",
                "MYSQL__DATABASE__PASSWORD",
                "MYSQL__DATABASE__DBNAME"
            };

            bool doesntExists = true;
            foreach(string envVar in vars)
            {
                string? s;
                if ((s = Environment.GetEnvironmentVariable(envVar)) == null)
                {
                    Console.WriteLine("Falta la variable de entorno " + envVar);
                    doesntExists = false;
                }
            }

            return doesntExists;
        }
    }
}
