using APPLICATION.APPLICATION.CONFIGURATIONS.APPLICATIONINSIGHTS;
using APPLICATION.APPLICATION.CONFIGURATIONS.SWAGGER;
using APPLICATION.APPLICATION.SERVICES.COMPANY;
using APPLICATION.APPLICATION.SERVICES.FILE;
using APPLICATION.APPLICATION.SERVICES.TOKEN;
using APPLICATION.APPLICATION.SERVICES.USER;
using APPLICATION.DOMAIN.CONTRACTS.API;
using APPLICATION.DOMAIN.CONTRACTS.CONFIGURATIONS;
using APPLICATION.DOMAIN.CONTRACTS.CONFIGURATIONS.APPLICATIONINSIGHTS;
using APPLICATION.DOMAIN.CONTRACTS.REPOSITORY.COMPANY;
using APPLICATION.DOMAIN.CONTRACTS.REPOSITORY.USER;
using APPLICATION.DOMAIN.CONTRACTS.SERVICES.COMPANY;
using APPLICATION.DOMAIN.CONTRACTS.SERVICES.FILE;
using APPLICATION.DOMAIN.CONTRACTS.SERVICES.TOKEN;
using APPLICATION.DOMAIN.CONTRACTS.SERVICES.USER;
using APPLICATION.DOMAIN.ENTITY.COMPANY;
using APPLICATION.DOMAIN.ENTITY.PLAN;
using APPLICATION.DOMAIN.ENTITY.ROLE;
using APPLICATION.DOMAIN.ENTITY.USER;
using APPLICATION.DOMAIN.UTILS.GLOBAL;
using APPLICATION.ENUMS;
using APPLICATION.INFRAESTRUTURE.CONTEXTO;
using APPLICATION.INFRAESTRUTURE.FACADES.EMAIL;
using APPLICATION.INFRAESTRUTURE.JOBS;
using APPLICATION.INFRAESTRUTURE.JOBS.FACTORY;
using APPLICATION.INFRAESTRUTURE.JOBS.FACTORY.FLUENTSCHEDULER;
using APPLICATION.INFRAESTRUTURE.JOBS.INTERFACES;
using APPLICATION.INFRAESTRUTURE.REPOSITORY.COMPANY;
using APPLICATION.INFRAESTRUTURE.REPOSITORY.USER;
using APPLICATION.INFRAESTRUTURE.SIGNALR.HUBS;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using RedeAceitacao.Archetype.Application.Infra.ServiceBus;
using RedeAceitacao.Archetype.Application.Infra.ServiceBus.Provider.Lote;
using Refit;
using Serilog;
using Serilog.Events;
using System.Globalization;
using System.Net.Mime;
using System.Security.Claims;
using System.Text;

namespace APPLICATION.APPLICATION.CONFIGURATIONS;

public static class ExtensionsConfigurations
{
    public static readonly string HealthCheckEndpoint = "/application/healthcheck";

    private static string _applicationInsightsKey;

    private static TelemetryConfiguration _telemetryConfig;

    private static TelemetryClient _telemetryClient;

    /// <summary>
    /// Configuração de Logs do sistema.
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection ConfigureSerilog(this IServiceCollection services)
    {
        Log.Logger = new LoggerConfiguration()
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
                                 .WriteTo.Console()
                                 .WriteTo.ApplicationInsights(_telemetryConfig, TelemetryConverter.Traces, LogEventLevel.Information)
                                 .CreateLogger();
        services
            .AddTransient<ILogWithMetric, LogWithMetric>();

        return services;
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
            .AddDbContext<Contexto>(options =>
            {
                options.UseLazyLoadingProxies().UseSqlServer(configurations.GetValue<string>("ConnectionStrings:BaseDados"));

                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

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
            .AddIdentity<UserEntity, RoleEntity>(options =>
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
                    Log.Information($"[LOG ERROR] {nameof(JwtBearerEvents)} - METHOD OnAuthenticationFailed - {context.Exception.Message}\n");

                    return Task.CompletedTask;
                },

                OnTokenValidated = context =>
                {
                    Log.Information($"[LOG INFORMATION] {nameof(JwtBearerEvents)} - OnTokenValidated - {context.SecurityToken}\n");

                    GlobalData<object>.GlobalUser = new DOMAIN.DTOS.USER.UserData
                    {
                        Id = Guid.Parse(context.Principal.Claims?.FirstOrDefault().Value)
                    };

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
                options.AddPolicy("accessPerson", policy => policy.RequireClaim("accessPerson", "get", "post", "put", "patch", "delete"));

                options.AddPolicy("accessClaim", policy => policy.RequireClaim("accessClaim", "get", "post", "put", "patch", "delete"));

                options.AddPolicy("accessRole", policy => policy.RequireClaim("accessRole", "get", "post", "put", "patch", "delete"));
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
    /// Configuração de métricas
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection ConfigureTelemetry(this IServiceCollection services, IConfiguration configuration)
    {
        var httpContextAccessor = services.BuildServiceProvider().GetService<IHttpContextAccessor>();

        _telemetryConfig = TelemetryConfiguration.CreateDefault();

        _telemetryConfig.ConnectionString = configuration.GetSection("ApplicationInsights:ConnectionStringApplicationInsightsKey").Value;

        _telemetryConfig.TelemetryInitializers.Add(new ApplicationInsightsInitializer(configuration, httpContextAccessor));

        _telemetryClient = new TelemetryClient(_telemetryConfig);

        services
            .AddSingleton<ITelemetryInitializer>(x => new ApplicationInsightsInitializer(configuration, httpContextAccessor))
            .AddSingleton<ITelemetryProxy>(x => new TelemetryProxy(_telemetryClient));

        return services;
    }

    /// <summary>
    /// Configuração de App Insights
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection ConfigureApplicationInsights(this IServiceCollection services, IConfiguration configuration)
    {
        var metrics = new ApplicationInsightsMetrics(_telemetryClient, _applicationInsightsKey);

        var applicationInsightsServiceOptions = new ApplicationInsightsServiceOptions
        {
            ConnectionString = configuration.GetSection("ApplicationInsights:ConnectionStringApplicationInsightsKey").Value
        };

        services
            .AddApplicationInsightsTelemetry(applicationInsightsServiceOptions)
            .AddTransient(x => metrics)
            .AddTransient<IApplicationInsightsMetrics>(x => metrics);

        return services;
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
                License = new OpenApiLicense
                {

                    Name = "HYPER.IO LICENSE",
                },
                TermsOfService = new Uri(uriMyGit)
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
    public static IServiceCollection ConfigureDependencies(this IServiceCollection services, IConfiguration configurations, IWebHostEnvironment webHostEnvironment)
    {
        // Se for ambiente de produção executa
        if (webHostEnvironment.IsProduction())
        {
            if (string.IsNullOrEmpty(configurations.GetValue<string>("ApplicationInsights:InstrumentationKey")))
            {
                var argNullEx = new ArgumentNullException("AppInsightsKey não pode ser nulo.", new Exception("Parametro inexistente.")); throw argNullEx;
            }
            else
            {
                _applicationInsightsKey = configurations.GetValue<string>("ApplicationInsights:InstrumentationKey");
            }
        }

        services
            .AddTransient(x => configurations)
            // Services
            .AddTransient<IUserService, UserService>()
            .AddTransient<IRoleService, RoleService>()
            .AddTransient<ITokenService, TokenService>()
            .AddTransient<IFileService, FileService>()
            .AddTransient<ICompanyService, CompanyService>()
            // Facades
            .AddSingleton<EmailFacade>()
            // Repository
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<ICompanyRepository, CompanyRepository>()
            // Infra
            .AddSingleton<ILoteServiceBusSenderProvider, LoteServiceBusSenderProvider>()
            .AddSingleton<ILoteServiceBusReceiverProvider, LoteServiceBusReceiverProvider>();

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
                policy.AllowAnyHeader().AllowAnyMethod().SetIsOriginAllowed((host) => true).AllowCredentials();
            });
        });
    }

    /// <summary>
    /// Registro de Jobs.
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection ConfigureRegisterJobs(this IServiceCollection services)
    {
        services.AddTransient<IRegistryJobs, RegistryJobs>();

        services.AddTransient<IProcessDeleteUserWithoutPersonJob, ProcessDeleteUserWithoutPersonJob>();

        services.ConfigureStartJobs();

        return services;
    }

    // Configure Hangfire
    public static IServiceCollection ConfigureHangFire(this IServiceCollection services)
    {
        var inMemory = GlobalConfiguration.Configuration.UseMemoryStorage();

        services.AddHangfire(configuration => configuration
                       .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                       .UseSimpleAssemblyNameTypeSerializer()
                       .UseRecommendedSerializerSettings()
                       .UseStorage(inMemory.Entry)
                       );

        // Add the processing server as IHostedService
        services.AddHangfireServer();

        services.GetProvider().GetService<IRegistryJobs>();

        return services;
    }

    /// <summary>
    /// Iniciar Jobs.
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection ConfigureStartJobs(this IServiceCollection services)
    {
        // Iniciar os Jobs.
        new ScheduledTasksManager(services.GetProvider()).StartJobs();

        return services;
    }

    // Configura os subscribers.
    public static IServiceCollection ConfigureSubscribers(this IServiceCollection services)
    {
        services
            //Subscribers
            .AddTransient<LoteSubscriber>();

        return services;
    }

    /// <summary>
    /// Configuração do HealthChecks do sistema.
    /// </summary>
    /// <param name="application"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseHealthChecks(this IApplicationBuilder application)
    {
        application.UseHealthChecks(HealthCheckEndpoint, new HealthCheckOptions
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
    /// Coniguras os endpoints & Hubs
    /// </summary>
    /// <param name="application"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseEndpoints(this IApplicationBuilder application)
    {
        application.UseEndpoints(endpoints =>
        {
            endpoints.MapHub<HubPerson>("/person");
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
    /// Execute Seeds in database
    /// </summary>
    /// <param name="serviceProvider"></param>
    public static async Task<WebApplication> Seeds(this WebApplication application)
    {
        using (var scope = application.Services.CreateScope())
        {
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UserEntity>>();

            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<RoleEntity>>();

            var context = scope.ServiceProvider.GetRequiredService<Contexto>();

            if (await userManager.Users.AnyAsync() is false)
            {
                Log.Debug($"[LOG DEBUG] - Criando Seeds.\n");

                // Set data in user.
                var user = new UserEntity
                {
                    FirstName = "Admin",
                    LastName = "Admin",
                    Email = "Admin@outlook.com",
                    EmailConfirmed = true,
                    UserName = "Admin",
                    PhoneNumber = "+55(18)99776-2452",
                    Created = DateTime.Now,
                    Status = Status.Active,
                };

                // Generate a password hash.
                user.PasswordHash = new PasswordHasher<UserEntity>().HashPassword(user, "Admin@123456789");

                // Create user.
                await userManager.CreateAsync(user);

                // Set user id in user.
                user.CreatedUserId = user.Id;

                // Update user.
                await userManager.UpdateAsync(user);

                // Add Login in user.
                await userManager.AddLoginAsync(user, new UserLoginInfo("TOOLS.USER.API", "TOOLS.USER", "TOOLS.USER.PROVIDER.KEY"));

                // Set data in role.
                var role = new RoleEntity
                {
                    Name = "administrator",
                    CreatedUserId = user.Id,
                    Status = Status.Active,
                    Created = DateTime.Now
                };

                // Create role.
                await roleManager.CreateAsync(role);

                // Add claim in role.
                await roleManager.AddClaimAsync(role, new Claim("User", "Get"));
                await roleManager.AddClaimAsync(role, new Claim("User", "Post"));
                await roleManager.AddClaimAsync(role, new Claim("User", "Put"));
                await roleManager.AddClaimAsync(role, new Claim("User", "Patch"));
                await roleManager.AddClaimAsync(role, new Claim("User", "Delete"));

                await roleManager.AddClaimAsync(role, new Claim("Claim", "Get"));
                await roleManager.AddClaimAsync(role, new Claim("Claim", "Post"));
                await roleManager.AddClaimAsync(role, new Claim("Claim", "Put"));
                await roleManager.AddClaimAsync(role, new Claim("Claim", "Patch"));
                await roleManager.AddClaimAsync(role, new Claim("Claim", "Delete"));

                await roleManager.AddClaimAsync(role, new Claim("Role", "Get"));
                await roleManager.AddClaimAsync(role, new Claim("Role", "Post"));
                await roleManager.AddClaimAsync(role, new Claim("Role", "Put"));
                await roleManager.AddClaimAsync(role, new Claim("Role", "Patch"));
                await roleManager.AddClaimAsync(role, new Claim("Role", "Delete"));

                // Add role to user.
                await userManager.AddToRoleAsync(user, role.Name);

                // Set plan.
                var plan = new PlanEntity
                {
                    PlanName = "Básico",
                    PlanCost = 10.00,
                    PlanDescription = "Plano padrão para uso pessoal.",
                    RoleId = role.Id,
                    Status = Status.Active,
                    Created = DateTime.Now,
                    CreatedUserId = user.Id,
                    TotalMonthsPlan = 12
                };

                // Add plan.
                await context.Plans.AddAsync(plan);

                // Sets data in Company.
                var company = new CompanyEntity
                {
                    Name = "HYPER.IO",
                    Description = "tecnology & future solutions.",
                    StartDate = DateTime.Now,
                    Status = Status.Active,
                    CreatedUserId = user.Id,
                    Created = DateTime.Now,
                    PlanId = plan.Id
                };

                // Add Company.
                await context.Companies.AddAsync(company);

                // Set company in user.
                user.CompanyId = company.Id;

                // Update user.
                await userManager.UpdateAsync(user);

                // Commit de transaction.
                await context.SaveChangesAsync();
            }
        }

        return application;
    }

    /// <summary>
    /// Configruação de minimals APIS.
    /// </summary>
    /// <param name="applicationBuilder"></param>
    /// <returns></returns>
    public static WebApplication UseMinimalAPI(this WebApplication application, IConfiguration configurations)
    {
        return application;
    }

    /// <summary>
    /// Retorna um provider do service.
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    private static ServiceProvider GetProvider(this IServiceCollection services)
    {
        return services.BuildServiceProvider();
    }
}
