using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using UniwayBackend;
using UniwayBackend.Config;
using UniwayBackend.Factories;
using UniwayBackend.Helpers;
using UniwayBackend.Helpers.Filters;
using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads;
using UniwayBackend.Repositories.Base;
using UniwayBackend.Repositories.Core.Implements;
using UniwayBackend.Repositories.Core.Interfaces;
using UniwayBackend.Services.implements;
using UniwayBackend.Services.interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(ModelStateInvalidFilter));
});
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(swagger =>
{
    //Generamos la UI por defecto de la documentación de Swagger
    swagger.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "v1",
        Title = "JWT Token Authentication API",
        Description = ".NET 8 Web API"
    });

    //Habilitamos la autorización usando Swagger(JWT)
    swagger.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization", // Nombre del parámetro en el encabezado HTTP
        Type = SecuritySchemeType.ApiKey, // Tipo de esquema de seguridad, en este caso, una clave de API
        Scheme = "Bearer", // El esquema utilizado en la autorización, aquí es 'Bearer'
        BearerFormat = "JWT", // Formato del token, que es un JWT
        In = ParameterLocation.Header, // Ubicación del parámetro, en este caso, en el encabezado HTTP
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
        // Descripción que se muestra en la UI de Swagger para indicar cómo usar el token JWT
    });

    //Definimos los requisitos de seguridad para los endpoints de la API
    swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme, // Tipo de referencia, que es un esquema de seguridad
                    Id = "Bearer" // El ID del esquema de seguridad, que debe coincidir con el definido anteriormente
                }
            },
            new string[] {} // Indica que no se requieren ámbitos específicos para este esquema de seguridad
        }
    });
});



// AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));



// Injection Dependency
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

builder.Services.AddScoped(typeof(UtilitariesResponse<>));
builder.Services.AddScoped(typeof(AppSettings));
builder.Services.AddScoped<IConfigurationLib, ConfigurationLib>();

builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();


builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IRoleService, RoleService>();

builder.Services.AddScoped<ITechnicalRepository, TechnicalRepository>();
builder.Services.AddScoped<IUserTechnicalRepository, UserTechnicalRepository>();

// Factories
// Para servicios que deben ser accesibles globalmente y mantener un estado único, como una fábrica que 
// crea y administra tipos específicos de objetos, AddSingleton es ideal.
builder.Services.AddSingleton<IUser, TechnicalCreator>();
builder.Services.AddSingleton<IUser, ClientCreator>();
builder.Services.AddSingleton<IUser, EmployeeCreator>();
builder.Services.AddSingleton<UserFactory>();
builder.Services.AddSingleton<ILoggerFactory, LoggerFactory>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<JwtMiddleware>(); // Agregamos nuestro MIDDLEWARE de JWT
app.MapControllers();

app.Run();
