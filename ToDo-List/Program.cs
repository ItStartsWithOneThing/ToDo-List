
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using ToDo_List.Controllers.Filters;
using ToDo_List.Models.DataBase;
using ToDo_List.Models.DataBase.Repositories.TaskCardRepositories;
using ToDo_List.Models.DataBase.Repositories.UserRepositories;
using ToDo_List.Models.MappingProfiles;
using ToDo_List.Models.Services;
using ToDo_List.Models.Services.Auth;
using ToDo_List.Models.Services.Auth.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AuthOptionsModel>(options =>
    builder.Configuration.GetSection("AuthOptions").Bind(options));

builder.Services.AddAuthorization();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;

    var key = builder.Configuration.GetSection("JwtSettings").GetValue<string>("SecretKey");
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration.GetSection("JwtSettings").GetValue<string>("Issuer"),
        ValidAudience = builder.Configuration.GetSection("JwtSettings").GetValue<string>("Audience"),
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

builder.Services.AddControllersWithViews()
    .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNameCaseInsensitive = true)
    .AddMvcOptions(options =>
    {
        options.Filters.Add<ValidatonActionFilter>();
    });

builder.Services.AddCors(options => options.AddPolicy("MyCORS", builder => builder
                    .WithOrigins("https://localhost:7274")
                    .AllowAnyHeader()
                    .AllowAnyMethod())
               );


builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ToDo-List", Version = "v1" });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

string connection = builder.Configuration.GetConnectionString("IdentityConnection");
builder.Services.AddDbContext<ToDoDbContext>(options => options.UseNpgsql(connection));

var assemblies = new[]
{
    Assembly.GetAssembly(typeof(MappingProfiles))
};
builder.Services.AddAutoMapper(assemblies);

builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<ITaskCardReadRepository, TaskCardReadRepository>();
builder.Services.AddScoped<ITaskCardWriteRepository, TaskCardWriteRepository>();
builder.Services.AddScoped<IUserReadRepository, UserReadRepository>();
builder.Services.AddScoped<IUserWriteRepository, UserWriteRepository>();

builder.Services.AddScoped<ITaskCardService, TaskCardService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "ToDo-List v1");
    });
}

app.UseHttpsRedirection();

app.UseCors("MyCORS");

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default-home",
    pattern: "{controller=Home}/{action=Index}");


app.Run();
