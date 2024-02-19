
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using ToDo_List.Models.DataBase.Repositories.RefreshSessionRepository;
using ToDo_List.Models.DataBase.Repositories.TaskCardRepositories;
using ToDo_List.Models.DataBase.Repositories.UserRepositories;
using ToDo_List.Models.Services.Auth;
using ToDo_List.Models.Services;

namespace ToDo_List.Controllers.Extensions
{
    public static class DependencyInjection
    {
        public static void AddSwaggerExtension(this IServiceCollection services)
        {
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ToDo-List", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new List<string>()
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        public static void AddJwtAuthenticationExtension(this IServiceCollection services, IConfiguration configuration)
        {
            var issuer = configuration.GetSection("JwtSettings").GetValue<string>("Issuer");
            var audience = configuration.GetSection("JwtSettings").GetValue<string>("Audience");
            var key = configuration.GetSection("JwtSettings").GetValue<string>("SecretKey");

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
                };

                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        context.Response.Redirect("/Home/Login");
                        return Task.CompletedTask;
                    },
                    OnChallenge = context =>
                    {
                        context.Response.Redirect("/Home/Login");
                        return Task.CompletedTask;
                    }
                };
            });
        }

        public static void AddRepositoriesExtension(this IServiceCollection services)
        {
            services.AddScoped<IRefreshSessionReadRepository, RefreshSessionReadRepository>();
            services.AddScoped<IRefreshSessionWriteRepository, RefreshSessionWriteRepository>();
            services.AddScoped<ITaskCardReadRepository, TaskCardReadRepository>();
            services.AddScoped<ITaskCardWriteRepository, TaskCardWriteRepository>();
            services.AddScoped<IUserReadRepository, UserReadRepository>();
            services.AddScoped<IUserWriteRepository, UserWriteRepository>();
        }

        public static void AddBusinessLogicServicesExtension(this IServiceCollection services)
        {
            services.AddScoped<ITaskCardService, TaskCardService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITokenService, TokenService>();
        }
    }
}
