using App_Bet.Analytics.Interfaces;
using App_Bet.Analytics.Models;
using App_Bets.Application.Commands.CommandsBilhetes.CreateBilhetes;
using App_Bets.Application.Commands.CommandsUser.CreateUsuario;
using App_Bets.Application.FluentValidation;
using App_Bets.Application.FluentValidation.Bilhete;
using App_Bets.Application.FluentValidation.Usuario;
using App_Bets.Domain.IRepositorio;
using App_Bets.Domain.IServices.Autentication;
using App_Bets.Domain.Modelos;
using App_Bets.Domain.Services;
using App_Bets.Infrastructure.ApiExterna;
using App_Bets.Infrastructure.Context;
using App_Bets.Infrastructure.Repository;
using App_Bets.Infrastructure.Service.Identity;
using App_Bets.Infrastructure.Services.AuthService;
using App_Bets.Infrastructure.Services.AuthService.TokenGeracao;
using BookReviewManager.Infrastructure.Service.Identity;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Converters;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace App_Bets.Extensions
{
    public static class ExtencaoDeConfiguracoes
    {
        public static IServiceCollection AddContextAppBet(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<BetDbContext>(options =>
             options.UseSqlServer(connectionString));

            //Identity
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<BetDbContext>()
                .AddDefaultTokenProviders();

            return services;
        }

        public static IServiceCollection AddInjecaoDependencias(this IServiceCollection services,
            IConfiguration configuration)
        {

            services.AddScoped<IUsuarioRepositorio, UsuarioRepository>();
            services.AddScoped<IBilheteRepository, BilheteRepository>();
            services.AddScoped<ICreateUser, CreateUser>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ICreateRole, CreateRole>();
            services.AddScoped<ILoginUser, LoginUser>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAddRole, AddRole>();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddHttpContextAccessor();
            services.AddScoped<IUsuarioContext, UsuarioContext>();

            services.Configure<BalldontlieSettings>(
                configuration.GetSection("Balldontlie"));

            services.AddHttpClient<IBalldontlieClient, BalldontlieClient>((serviceProvider, client) =>
            {
                var settings = serviceProvider
                    .GetRequiredService<IOptions<BalldontlieSettings>>().Value;

                client.BaseAddress = new Uri(settings.BaseUrl);

                client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue(
                        "Bearer",
                        settings.ApiKey);
            });

            //services.AddScoped<AnaliseJogadorService>();
            //services.AddScoped<AnalisarJogadorUseCase>();

            services.AddMemoryCache();

            ////fluentvalidation
            //services.AddFluentValidationAutoValidation()
            //    .AddValidatorsFromAssemblyContaining<CriacaoValidacao>();

            ////MediatR
            //var myHandlers = AppDomain.CurrentDomain.Load("App_Bets.Application");
            //services.AddMediatR(config =>
            //    config.RegisterServicesFromAssembly(myHandlers));

            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(
                    typeof(CreateBilheteCommand).Assembly));

            services.AddValidatorsFromAssemblyContaining<BilheteValidacao>();

            services.AddTransient(
                typeof(IPipelineBehavior<,>),
                typeof(ValidationBehavior<,>)
            );

            //Cors
            services.AddCors(options =>
            {
                options.AddPolicy("AllowNextJS",
                    policy => policy
                        .WithOrigins("http://localhost:3000") // URL do seu Next.js
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()); // Se usar cookies/auth
            });

            return services;
        }

        public static IServiceCollection AddSettingsController(this IServiceCollection services)
        {
            services.AddControllers()
               .AddJsonOptions(op =>
               {
                   op.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());// mostra no Schemas do swagger os valores do enum
                   op.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
               })
               .AddNewtonsoftJson(op => op.SerializerSettings.Converters.Add(new StringEnumConverter()));


            return services;
        }


        public static IServiceCollection AddJwtAuthetication(this IServiceCollection services, IConfiguration configuration)
        {
            //Jwt Token
            var secretKey = configuration["Jwt:SecretKey"] ?? throw new ArgumentException("Invalid secret Key ..");

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; // desafio de solicitar o token
            }).AddJwtBearer(opt =>
            {
                opt.SaveToken = true; // salvar o token
                opt.RequireHttpsMetadata = false; // se é preciso https para transmitir o token , em produçao é true
                opt.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidAudience = configuration["Jwt:ValidAudience"],
                    ValidIssuer = configuration["Jwt:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))

                };
            });

            //Politicas que serão usadas para acessar os endpoints
            services.AddAuthorization(opt =>
            {
                opt.AddPolicy("Admin", policy => policy.RequireRole("Admin"));

                opt.AddPolicy("Usuario", policy => policy.RequireRole("Usuario"));
            }
            );

            return services;
        }

    }
}
