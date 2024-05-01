# boeris-creaciones-backend
Proyecto en ASP.NET para el Back-End de la app.

## Configuración para la conexión adecuada con el servidor de base de datos
Debe crearse un archivo `.env` en la ubicación `./BoerisCreaciones.Api/` que contenga las siguientes variables de entorno para poder realizar las conexiones a la base de datos:
```
MYSQL__DATABASE__SERVER=<IP del servidor>
MYSQL__DATABASE__USER=<nombre de usuario del servidor de base de datos>
MYSQL__DATABASE__PASSWORD=<contraseña del servidor de base de datos>
MYSQL__DATABASE__DBNAME=<nombre de la base de datos a la que conectarse>
```

En la cadena de conexión `BoerisCreacionesConnection` está especificada la dirección IP del servidor, el usuario de MySQL y su contraseña, y la base de datos con la que se conecta.