#region Using Imports
using Karage.Application.Interfaces;
using Karage.Application.Services;
using Karage.Domain.Entities;
using Karage.Domain.Interfaces;
using Karage.Domain.Jwt;
using Karage.Infrastructure.Data;
using Karage.Infrastructure.Repositories;
using Karage.Infrastructure.Services;
using Karage.Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
#endregion

namespace Karage.APIs.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            #region inject QB Services
            services.AddHttpClient<IQuickBooksService, QuickBooksService>();
            services.AddScoped<IInvoiceService, InvoiceService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IQBReportService, QBReportService>();
            #endregion

            #region inject JWT 
            services.Configure<JwtOptions>(configuration.GetSection("Jwt")); 
            services.AddScoped<IJwtService, JwtService>();
            #endregion

            #region Inject Database
            services.AddDbContext<KarageDBContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))); 
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            #endregion

            return services;
        }

        /// <summary>
        /// Configures Identity with ApplicationUser.
        /// </summary>
        public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<KarageDBContext>()
                .AddDefaultTokenProviders();

            return services;
        }

        /// <summary>
        /// Configures JWT authentication.
        /// </summary>
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtOptions = new JwtOptions();
            configuration.GetSection("Jwt").Bind(jwtOptions); 

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                 .AddJwtBearer(options =>
                 {
                     options.TokenValidationParameters = new TokenValidationParameters
                     {
                         ValidateIssuer = true,
                         ValidateAudience = true,
                         ValidateLifetime = true,
                         ValidateIssuerSigningKey = true,
                         ValidIssuer = jwtOptions.Issuer, // Should match "Issuer" in appsettings.json
                         ValidAudience = jwtOptions.Audience, // Should match "Audience" in appsettings.json
                         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key))
                     };

                     options.Events = new JwtBearerEvents
                     {
                         OnAuthenticationFailed = context =>
                         {
                             Console.WriteLine($"Authentication failed: {context.Exception.Message}");
                             return Task.CompletedTask;
                         }
                     };
                 });
             
            return services;
        }

        /// <summary>
        /// Configures Swagger with JWT Bearer authentication.
        /// </summary>
        public static IServiceCollection AddSwaggerWithJwt(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Karage API",
                    Version = "v1",
                    Description = "Karage API with JWT Authentication",
                    Contact = new OpenApiContact
                    {
                        Name = "Your Name",
                        Email = "your@email.com"
                    }
                });

                // Add JWT Authentication to Swagger
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Enter 'Bearer {token}'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                        new string[] {}
                    }
                });
            });

            return services;
        }
    }
}
