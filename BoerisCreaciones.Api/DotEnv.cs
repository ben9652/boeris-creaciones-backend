namespace BoerisCreaciones.Api
{
    using BoerisCreaciones.Service;
    using System;
    using System.IO;
    using System.Text.RegularExpressions;

    public static class DotEnv
    {
        public static bool CheckEnvVars()
        {
            List<string> vars = new List<string>()
            {
                "MYSQL__DATABASE__SERVER",
                "MYSQL__DATABASE__USER",
                "MYSQL__DATABASE__PASSWORD",
                "MYSQL__DATABASE__DBNAME",
                "JWT__KEY",
                "JWT__ISSUER",
                "JWT__AUDIENCE"
            };

            bool doesntExists = true;
            foreach (string envVar in vars)
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

        public static string? ParseConnectionString(string? connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                return null;

            string pattern = @"\${(.*?)}";

            Regex regex = new Regex(pattern);

            MatchCollection matches = regex.Matches(connectionString);

            foreach (Match match in matches)
            {
                string subString = match.Groups[1].Value;
                string? envVar = Environment.GetEnvironmentVariable(subString);
                if (envVar == null)
                    return null;

                string replacement = $"${{{subString}}}";
                connectionString = connectionString.Replace(replacement, envVar);
            }

            return connectionString;
        }
    }
}
