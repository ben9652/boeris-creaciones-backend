using BoerisCreaciones.Api;
using BoerisCreaciones.Core;
using BoerisCreaciones.Repository;
using BoerisCreaciones.Repository.Interfaces;
using BoerisCreaciones.Repository.Repositories;
using BoerisCreaciones.Service.Interfaces;
using BoerisCreaciones.Service.Services;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;

var envVariable = DotNetEnv.Env.Load();

if (!DotEnv.CheckEnvVars())
{
    Console.WriteLine("No est�n definidas las variables de entorno necesarias correctamente.");
    Console.In.ReadLine();
    return;
}

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connection = builder.Configuration.GetConnectionString("BoerisCreacionesConnection");
if (connection == null)
{
    Console.WriteLine("La cadena de conexi�n 'BoerisCreacionesConnection' no se encuentra en la configuraci�n.");
    Console.In.ReadLine();
    return;
}
else
{
    builder.Services.AddDbContext<BoerisCreacionesContext>(options =>
    {
        options.UseMySql(connection, new MySqlServerVersion(new Version(8, 0, 27)));
    });
}

connection = DotEnv.ParseConnectionString(connection);
if (connection == null)
{
    Console.WriteLine("La cadena de conexi�n 'BoerisCreacionesConnection' est� configurada incorrectamente.");
    Console.In.ReadLine();
    return;
}

MySqlConnection conn = new MySqlConnection(connection);
//if(conn.State == System.Data.ConnectionState.Closed)
//{
//    Console.WriteLine("La base de datos no existe o las credenciales son incorrectas. La cadena de conexi�n es: " + connection);
//    Console.In.ReadLine();
//    return;
//}

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder =>
        builder.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod()
    );
});

builder.Services.AddControllers();

// Esto le dice a la aplicaci�n que, cuando se inyecte una dependencia de tipo IUsuariosRepository, se debe instanciar un UsuariosRepository
builder.Services.AddScoped<IUsuariosRepository, UsuariosRepository>();

// Esto le dice a la aplicaci�n que, cuando se inyecte una dependencia de tipo IUsuariosService, se debe instanciar un UsuariosService
builder.Services.AddScoped<IUsuariosService, UsuariosService>();

builder.Services.AddScoped<IMateriasPrimasRepository, MateriasPrimasRepository>();

builder.Services.AddScoped<IMateriasPrimasService, MateriasPrimasService>();

builder.Configuration.Bind("ApplicationConfig", new ApplicationConfig());

builder.Services.AddSingleton(builder.Configuration.Get<ApplicationConfig>());

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAllOrigins");

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

try
{
    conn.Open();
}
catch(Exception ex)
{
    Console.WriteLine("La base de datos no existe o las credenciales son incorrectas. La cadena de conexi�n es: " + connection);
    Console.WriteLine(ex.Message);
    Console.In.ReadLine();
    return;
}

app.Run();
