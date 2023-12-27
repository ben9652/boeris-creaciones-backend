# boeris-creaciones-backend
Proyecto en ASP.NET para el Back-End de la app.

## Configuración para la conexión adecuada con el servidor de base de datos
Debe crearse un archivo `appsettings.json` en la ubicación `./BoerisCreaciones.Api/` que contenga lo siguiente:
```
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "BoerisCreacionesConnection": "server=localhost;user=root;password=facet;Initial Catalog=BoerisCreaciones_dev"
  }
}
```

En la cadena de conexión `BoerisCreacionesConnection` está especificada la dirección IP del servidor, el usuario de MySQL y su contraseña, y la base de datos con la que se conecta.