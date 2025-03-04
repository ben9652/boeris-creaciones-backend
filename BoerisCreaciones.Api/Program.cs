using BoerisCreaciones.Api;
using BoerisCreaciones.Core;
using BoerisCreaciones.Repository;
using BoerisCreaciones.Repository.Interfaces;
using BoerisCreaciones.Repository.Repositories;
using BoerisCreaciones.Service.Interfaces;
using BoerisCreaciones.Service.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Serialization;
using System.Text;

var envVariable = DotNetEnv.Env.Load();

if (!DotEnv.CheckEnvVars())
{
    Console.WriteLine("No están definidas las variables de entorno necesarias correctamente.");
    Console.In.ReadLine();
    return;
}

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connection = builder.Configuration.GetConnectionString("BoerisCreacionesConnection");
if (connection == null)
{
    Console.WriteLine("La cadena de conexión 'BoerisCreacionesConnection' no se encuentra en la configuración.");
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
    Console.WriteLine("La cadena de conexión 'BoerisCreacionesConnection' está configurada incorrectamente.");
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

builder.Services.AddControllers().AddNewtonsoftJson(s =>
{
    s.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

// Esto le dice a la aplicaci�n que, cuando se inyecte una dependencia de tipo IUsuariosRepository, se debe instanciar un UsuariosRepository
builder.Services.AddScoped<IUsuariosRepository, UsuariosRepository>();
builder.Services.AddScoped<ISociosRepository, SociosRepository>();
builder.Services.AddScoped<IRolesSociosRepository, RolesSociosRepository>();
builder.Services.AddScoped<IRubrosMateriasPrimasRepository, RubrosMateriasPrimasRepository>();
builder.Services.AddScoped<IRubrosProductosRepository, RubrosProductosRepository>();
builder.Services.AddScoped<IUnidadesRepository, UnidadesRepository>();
builder.Services.AddScoped<ICatalogoMateriasPrimasRepository, CatalogoMateriasPrimasRepository>();
builder.Services.AddScoped<ICatalogoProductosRepository, CatalogoProductosRepository>();
builder.Services.AddScoped<ICatalogoProveedoresRepository, CatalogoProveedoresRepository>();
builder.Services.AddScoped<ICatalogoSucursalesRepository, CatalogoSucursalesRepository>();
builder.Services.AddScoped<IProvinciasRepository, ProvinciasRepository>();
builder.Services.AddScoped<ILocalidadesRepository, LocalidadesRepository>();
builder.Services.AddScoped<IComprasRepository, ComprasRepository>();

// Esto le dice a la aplicaci�n que, cuando se inyecte una dependencia de tipo IUsuariosService, se debe instanciar un UsuariosService
builder.Services.AddScoped<IUsuariosService, UsuariosService>();
builder.Services.AddScoped<ISociosService, SociosService>();
builder.Services.AddScoped<IRolesSociosService, RolesSociosService>();
builder.Services.AddScoped<IRubrosMateriasPrimasService, RubrosMateriasPrimasService>();
builder.Services.AddScoped<IUnidadesService, UnidadesService>();
builder.Services.AddScoped<ICatalogoMateriasPrimasService, CatalogoMateriasPrimasService>();
builder.Services.AddScoped<ICatalogoProductosService, CatalogoProductosService>();
builder.Services.AddScoped<ICatalogoProveedoresService, CatalogoProveedoresService>();
builder.Services.AddScoped<ICatalogoSucursalesService, CatalogoSucursalesService>();
builder.Services.AddScoped<IProvinciasService, ProvinciasService>();
builder.Services.AddScoped<ILocalidadesService, LocalidadesService>();
builder.Services.AddScoped<IComprasService, ComprasService>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Agrego como un singleton un objeto para referirse a la cadena de conexi�n personalizada que obtuve mediante las variables de entorno
builder.Services.AddSingleton(new ConnectionStringProvider(connection));

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

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

// Pruebo una primera conexi�n a la base de datos para abortar el programa si es que no se conecta correctamente.
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
