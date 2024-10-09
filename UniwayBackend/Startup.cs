using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using UniwayBackend.Config;
using UniwayBackend.Factories;
using UniwayBackend.Helpers.Filters;
using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Base.Response;
using UniwayBackend.Repositories.Base;
using UniwayBackend.Repositories.Core.Implements;
using UniwayBackend.Repositories.Core.Interfaces;
using UniwayBackend.Services.implements;
using UniwayBackend.Services.interfaces;

namespace UniwayBackend
{
    public class Startup
    {

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration) 
        { 
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Injection Dependency
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            RegisterServices(services);
        }


        private static void RegisterServices(IServiceCollection services)
        {

            // Add services to the container.
            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(ModelStateInvalidFilter));
            });
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            // AutoMapper
            services.AddAutoMapper(typeof(AutoMapperProfile));

            // Configre JwtAuthentication
            JwtConfig(services);

            // Add swagger services 
            SwaggerConfig(services);

            // SignalR
            services.AddSignalR();
            services.AddSingleton<IUserIdProvider, UserIdJwtProvider>();

            ///// Injection Dependency

            // Factories
            // Para servicios que deben ser accesibles globalmente y mantener un estado único, como una fábrica que 
            // crea y administra tipos específicos de objetos, AddSingleton es ideal.
            services.AddSingleton<IUser, TechnicalCreator>();
            services.AddSingleton<IUser, ClientCreator>();
            services.AddSingleton<IUser, EmployeeCreator>();
            services.AddSingleton<UserFactory>();
            services.AddSingleton<ILoggerFactory, LoggerFactory>();

            services.AddScoped(typeof(UtilitariesResponse<>));
            services.AddScoped(typeof(AppSettings));
            services.AddScoped<IConfigurationLib, ConfigurationLib>();

            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();


            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IRoleService, RoleService>();

            services.AddScoped<ITechnicalRepository, TechnicalRepository>();
            services.AddScoped<IUserTechnicalRepository, UserTechnicalRepository>();

            services.AddScoped<IClientRepository, ClientRepository>();

            services.AddScoped<IWorkshopTechnicalProfessionRepository, WorkshopTechnicalProfessionRepository>();
            services.AddScoped<IWorkshopTechnicalProfessionService, WorkshopTechnicalProfessionService>();

            services.AddScoped<IWorkshopRepository, WorkshopRepository>();

            services.AddScoped<ILocationService, LocationService>();
            
            services.AddScoped<IReviewService, ReviewService>();
            services.AddScoped<IReviewRepository, ReviewRepository>();

            services.AddScoped<ITechnicalService, TechnicalService>();
            
            services.AddScoped<ICategoryServiceRepository, CategoryServiceRepository>();
            services.AddScoped<ICategoryServiceService, CategoryServiceService>();

            services.AddScoped<IStateRequestService, StateRequestService>();
            services.AddScoped<ICategoryRequestService, CategoryRequestService>();

            services.AddScoped<IImagesProblemRequestService, ImagesProblemRequestService>();

            services.AddScoped<IRequestService, RequestService>();
            services.AddScoped<IRequestRepository, RequestRepository>();

            services.AddScoped<IStorageService, StorageService>();

            services.AddScoped<IBaseRepository<StateRequest, short>, BaseRepository<StateRequest, short>>();
            services.AddScoped<IBaseRepository<CategoryRequest, short>, BaseRepository<CategoryRequest, short>>();
            services.AddScoped<IBaseRepository<ImagesProblemRequest, int>, BaseRepository<ImagesProblemRequest, int>>();

            services.AddScoped<ITechnicalProfessionAvailabilityRepository, TechnicalProfessionAvailabilityRepository>();
            services.AddScoped<ITechnicalProfessionAvailabilityService, TechnicalProfessionAvailabilityService>();

            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<ITechnicalProfessionAvailabilityRequestRepository, TechnicalProfessionAvailabilityRequestRepository>();

            services.AddScoped<ITechnicalResponseRepository, TechnicalResponseRepository>();
            services.AddScoped<ITechnicalResponseService, TechnicalResponseService>();

            services.AddScoped<IServiceTechnicalRepository, ServiceTechnicalRepository>();
            services.AddScoped<IWorkshopService, WorkshopService>();

            services.AddScoped<IImagesProblemRequestRepository, ImagesProblemRequestRepository>();
            services.AddScoped<ITechnicalProfessionAvailabilityRequestRepository, TechnicalProfessionAvailabilityRequestRepository>();
            services.AddScoped<ITechnicalProfessionAvailabilityRequestService, TechnicalProfessionAvailabilityRequestService>();
        }

        private static void JwtConfig(IServiceCollection services)
        {
            var secretKey = services.BuildServiceProvider()
                                  .GetRequiredService<IConfiguration>()
                                  .GetValue<string>("AppSettings:Secret"); 

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
               options.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuer = false, // Verifica que el emisor (Issuer) del token sea válido.
                   ValidateAudience = false, // Verifica que la audiencia (Audience) del token sea correcta.
                   ValidateLifetime = true, // Verifica que el token no esté expirado.
                   ValidateIssuerSigningKey = true, // Verifica que el token fue firmado con una clave válida.
                   IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey)),
                   //ValidIssuer = "https://localhost:44300",
                   //ValidAudience = "https://localhost:5500",
                   ClockSkew = TimeSpan.Zero // Para evitar retrasos en la expiración del token
               };

               // Permitir el uso de JWT en SignalR
               options.Events = new JwtBearerEvents
               {
                   OnMessageReceived = context =>
                   {
                       var accessToken = context.Request.Query["access_token"];

                       // Si la solicitud es para el hub de SignalR
                       var path = context.HttpContext.Request.Path;
                       if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/notificationHub"))
                       {
                           // Asigna el token al contexto
                           context.Token = accessToken;
                       }
                       return Task.CompletedTask;
                   }
               };
           });
        }

        private static void SwaggerConfig(IServiceCollection services)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(swagger =>
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
        }
    }
}
