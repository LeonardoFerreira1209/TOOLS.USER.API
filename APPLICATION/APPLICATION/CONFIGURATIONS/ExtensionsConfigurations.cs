using APPLICATION.APPLICATION.CONFIGURATIONS.SWAGGER;
using APPLICATION.APPLICATION.SERVICES.TOKEN;
using APPLICATION.APPLICATION.SERVICES.USER;
using APPLICATION.DOMAIN.CONTRACTS.API;
using APPLICATION.DOMAIN.CONTRACTS.SERVICES.TOKEN;
using APPLICATION.DOMAIN.CONTRACTS.SERVICES.USER;
using APPLICATION.DOMAIN.DTOS.CONFIGURATION.AUTH.TOKEN;
using APPLICATION.DOMAIN.DTOS.REQUEST.USER;
using APPLICATION.DOMAIN.UTILS;
using APPLICATION.INFRAESTRUTURE.CONTEXTO;
using APPLICATION.INFRAESTRUTURE.FACADES.EMAIL;
using HotChocolate;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Refit;
using Serilog;
using Serilog.Context;
using Serilog.Events;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Net.Mime;
using System.Text;

namespace APPLICATION.APPLICATION.CONFIGURATIONS;

public static class ExtensionsConfigurations
{
    public static readonly string HealthCheckEndpoint = "/application/healthcheck";

    /// <summary>
    /// Configuração de Logs do sistema.
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static WebApplicationBuilder ConfigureSerilog(this WebApplicationBuilder applicationBuilder)
    {
        Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();

        applicationBuilder.Host.UseSerilog((context, config) =>
        {
            config
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("System", LogEventLevel.Error)
            .Enrich.FromLogContext()
            .Enrich.WithEnvironmentUserName()
            .Enrich.WithMachineName()
            .Enrich.WithProcessId()
            .Enrich.WithProcessName()
            .Enrich.WithThreadId()
            .Enrich.WithThreadName()
            .WriteTo.Console();
        });

        return applicationBuilder;
    }

    /// <summary>
    /// Configuração de linguagem principal do sistema.
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection ConfigureLanguage(this IServiceCollection services)
    {
        var cultureInfo = new CultureInfo("pt-BR");

        CultureInfo
            .DefaultThreadCurrentCulture = cultureInfo;

        CultureInfo
            .DefaultThreadCurrentUICulture = cultureInfo;

        return services;
    }

    /// <summary>
    /// Configuração do banco de dados do sistema.
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection ConfigureContexto(this IServiceCollection services, IConfiguration configurations)
    {
        services
            .AddDbContext<Contexto>(options => options.UseSqlServer(configurations.GetValue<string>("ConnectionStrings:BaseDados")));

        return services;
    }

    /// <summary>
    /// Configuração do identity server do sistema.
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection ConfigureIdentityServer(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddIdentity<IdentityUser<Guid>, IdentityRole<Guid>>(options =>
            {
                #region Signin
                options.SignIn.RequireConfirmedEmail = true;

                options.SignIn.RequireConfirmedAccount = true;
                #endregion

                #region User
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";

                options.User.RequireUniqueEmail = true;
                #endregion

                #region Stores
                options.Stores.MaxLengthForKeys = 20;
                #endregion

                #region Lockout
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);

                options.Lockout.MaxFailedAccessAttempts = 3;

                options.Lockout.AllowedForNewUsers = true;
                #endregion

                #region Password
                options.Password.RequireDigit = true;

                options.Password.RequireLowercase = true;

                options.Password.RequireUppercase = true;

                options.Password.RequiredLength = configuration.GetValue<int>("Auth:Password:RequiredLength");

                options.Password.RequireNonAlphanumeric = true;

                options.Password.RequiredUniqueChars = 1;
                #endregion

            }).AddEntityFrameworkStores<Contexto>().AddDefaultTokenProviders();

        return services;
    }

    /// <summary>
    /// Configuração da autenticação do sistema.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configurations"></param>
    /// <returns></returns>
    public static IServiceCollection ConfigureAuthentication(this IServiceCollection services, IConfiguration configurations)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

        }).AddJwtBearer(options =>
        {
            options.SaveToken = true;

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = configurations.GetValue<string>("Auth:ValidIssuer"),
                ValidAudience = configurations.GetValue<string>("Auth:ValidAudience"),
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configurations.GetValue<string>("Auth:SecurityKey")))
            };

            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    Log.Information("[LOG INFORMATION] - OnAuthenticationFailed " + context.Exception.Message);

                    return Task.CompletedTask;
                },

                OnTokenValidated = context =>
                {
                    Log.Information("[LOG INFORMATION] - OnTokenValidated " + context.SecurityToken);

                    return Task.CompletedTask;
                }
            };

        });

        return services;
    }

    /// <summary>
    /// Configuração da authorização do sistema.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configurations"></param>
    /// <returns></returns>
    public static IServiceCollection ConfigureAuthorization(this IServiceCollection services, IConfiguration configurations)
    {
        services
            .AddAuthorization(options =>
            {
                options.AddPolicy("User", policy => policy.RequireClaim("Permission", "Admin", "Master"));
            });
        
        return services;
    }

    /// <summary>
    /// Configura os cookies da applicação.
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection ConfigureApllicationCookie(this IServiceCollection services)
    {
        return services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.HttpOnly = true;

            options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

            options.SlidingExpiration = true;

        });
    }

    /// <summary>
    /// Configuração do swagger do sistema.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configurations"></param>
    /// <returns></returns>
    public static IServiceCollection ConfigureSwagger(this IServiceCollection services, IConfiguration configurations)
    {
        var apiVersion = configurations.GetValue<string>("SwaggerInfo:ApiVersion"); var apiDescription = configurations.GetValue<string>("SwaggerInfo:ApiDescription"); var uriMyGit = configurations.GetValue<string>("SwaggerInfo:UriMyGit");

        services.AddSwaggerGen(swagger =>
        {
            swagger.EnableAnnotations();

            swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme."
            });

            swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },

                    Array.Empty<string>()
                }
            });

            swagger.SwaggerDoc(apiVersion, new OpenApiInfo
            {
                Version = apiVersion,
                Title = $"{apiDescription} - {apiVersion}",
                Description = apiDescription,

                Contact = new OpenApiContact
                {
                    Name = "HYPER.IO DESENVOLVIMENTOS LTDA",
                    Email = "HYPER.IO@OUTLOOK.COM",
                },
            });


            swagger.DocumentFilter<HealthCheckSwagger>();
        });

        return services;
    }

    /// <summary>
    /// Configuração das dependencias (Serrvices, Repository, Facades, etc...).
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection ConfigureDependencies(this IServiceCollection services, IConfiguration configurations)
    {
        services
            .AddTransient(x => configurations)
            // Services
            .AddTransient<IUserService, UserService>()
            .AddTransient<IRoleService, RoleService>()
            .AddTransient<ITokenService, TokenService>()
            // Facades
            .AddSingleton<EmailFacade, EmailFacade>();

        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        return services;
    }

    /// <summary>
    /// Configura chamadas a APIS externas através do Refit.
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection ConfigureRefit(this IServiceCollection services, IConfiguration configurations)
    {
        services
            .AddRefitClient<IEmailExternal>().ConfigureHttpClient(c => c.BaseAddress = configurations.GetValue<Uri>("UrlBase:TOOLS_MAIL_API"));

        return services;
    }

    /// <summary>
    /// Configuração do HealthChecks do sistema.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configurations"></param>
    /// <returns></returns>
    public static IServiceCollection ConfigureHealthChecks(this IServiceCollection services, IConfiguration configurations)
    {
        services
            .AddHealthChecks().AddSqlServer(configurations.GetConnectionString("BaseDados"), name: "Base de dados padrão.", tags: new string[] { "Core", "SQL Server" });

        return services;
    }

    /// <summary>
    /// Configuração dos cors aceitos.
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection ConfigureCors(this IServiceCollection services)
    {
        return services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", policy =>
            {
                policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
            });
        });
    }

    /// <summary>
    /// Configuração do HealthChecks do sistema.
    /// </summary>
    /// <param name="application"></param>
    /// <returns></returns>
    public static IApplicationBuilder ConfigureHealthChecks(this IApplicationBuilder application)
    {
        application.UseHealthChecks(ExtensionsConfigurations.HealthCheckEndpoint, new HealthCheckOptions
        {
            ResponseWriter = async (context, report) =>
            {
                var result = JsonConvert.SerializeObject(new
                {
                    statusApplication = report.Status.ToString(),

                    healthChecks = report.Entries.Select(e => new
                    {
                        check = e.Key,
                        ErrorMessage = e.Value.Exception?.Message,
                        status = Enum.GetName(typeof(HealthStatus), e.Value.Status)
                    })
                });

                context.Response.ContentType = MediaTypeNames.Application.Json;

                await context.Response.WriteAsync(result);
            }
        });

        return application;
    }

    /// <summary>
    /// Configuração de uso do swagger do sistema.
    /// </summary>
    /// <param name="application"></param>
    /// <param name="configurations"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseSwaggerConfigurations(this IApplicationBuilder application, IConfiguration configurations)
    {
        var apiVersion = configurations.GetValue<string>("SwaggerInfo:ApiVersion");

        application.UseSwagger(c =>
        {
            c.RouteTemplate = "swagger/{documentName}/swagger.json";
        });

        application
            .UseSwaggerUI(swagger =>
            {
                swagger.SwaggerEndpoint($"/swagger/{apiVersion}/swagger.json", $"{apiVersion}");
            });

        application
            .UseMvcWithDefaultRoute();

        return application;
    }

    /// <summary>
    /// Configruação de minimals APIS.
    /// </summary>
    /// <param name="applicationBuilder"></param>
    /// <returns></returns>
    public static WebApplication UseMinimalAPI(this WebApplication application, IConfiguration configurations)
    {
        #region User's
        application.MapPost("/security/create",
        [EnableCors("CorsPolicy")][SwaggerOperation(Summary = "Criar uauário.", Description = "Método responsavel por criar usuário")]
        [ProducesResponseType(typeof(DOMAIN.DTOS.RESPONSE.ApiResponse<TokenJWT>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(DOMAIN.DTOS.RESPONSE.ApiResponse<TokenJWT>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(DOMAIN.DTOS.RESPONSE.ApiResponse<TokenJWT>), StatusCodes.Status500InternalServerError)]
        async ([Service] IUserService userService, UserRequest request) =>
        {
            using (LogContext.PushProperty("Controller", "UserController"))
            using (LogContext.PushProperty("Payload", JsonConvert.SerializeObject(request)))
            using (LogContext.PushProperty("Metodo", "Create"))
            {
                return await Tracker.Time(() => userService.Create(request), "Criar usuário");
            }
        });

        application.MapPost("/security/authentication",
        [EnableCors("CorsPolicy")][SwaggerOperation(Summary = "Autenticação do usuário", Description = "Método responsável por Autenticar usuário")]
        [ProducesResponseType(typeof(DOMAIN.DTOS.RESPONSE.ApiResponse<TokenJWT>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(DOMAIN.DTOS.RESPONSE.ApiResponse<TokenJWT>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(DOMAIN.DTOS.RESPONSE.ApiResponse<TokenJWT>), StatusCodes.Status500InternalServerError)]
        async ([Service] IUserService userService, LoginRequest request) =>
        {
            using (LogContext.PushProperty("Controller", "UserController"))
            using (LogContext.PushProperty("Payload", JsonConvert.SerializeObject(request)))
            using (LogContext.PushProperty("Metodo", "Authentication"))
            {
                return await Tracker.Time(() => userService.Authentication(request), "Autenticar usuário");
            }
        });


        application.MapGet("/security/activate/{code}/{userId}",
        [EnableCors("CorsPolicy")][SwaggerOperation(Summary = "Ativar usuário", Description = "Método responsável por Ativar usuário")]
        [ProducesResponseType(typeof(DOMAIN.DTOS.RESPONSE.ApiResponse<TokenJWT>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(DOMAIN.DTOS.RESPONSE.ApiResponse<TokenJWT>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(DOMAIN.DTOS.RESPONSE.ApiResponse<TokenJWT>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(DOMAIN.DTOS.RESPONSE.ApiResponse<TokenJWT>), StatusCodes.Status500InternalServerError)]
        async ([Service] IUserService userService, string code, Guid userId) =>
        {
            var request = new ActivateUserRequest(code, userId);

            using (LogContext.PushProperty("Controller", "UserController"))
            using (LogContext.PushProperty("Payload", JsonConvert.SerializeObject(request)))
            using (LogContext.PushProperty("Metodo", "activate"))
            {
                return await Tracker.Time(() => userService.Activate(request), "Ativar usuário");
            }
        });
        #endregion

        return application;
    }

}
