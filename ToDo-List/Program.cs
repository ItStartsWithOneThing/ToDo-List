
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using ToDo_List.Controllers.Extensions;
using ToDo_List.Controllers.Filters;
using ToDo_List.Models.DataBase;
using ToDo_List.Models.MappingProfiles;
using ToDo_List.Models.Services.Auth.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AuthOptionsModel>(options =>
    builder.Configuration.GetSection("AuthOptions").Bind(options));

builder.Services.AddAuthorization();
builder.Services.AddJwtAuthenticationExtension(builder.Configuration);

builder.Services.AddControllersWithViews()
    .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNameCaseInsensitive = true)
    .AddMvcOptions(options =>
    {
        options.Filters.Add<ValidatonActionFilter>();
    });

builder.Services.AddCors(options => options.AddPolicy("MyCORS", builder => builder
                    .WithOrigins("https://localhost:7271")
                    .AllowAnyHeader()
                    .AllowAnyMethod())
               );


builder.Services.AddSwaggerExtension();

var assemblies = new[]
{
    Assembly.GetAssembly(typeof(MappingProfiles))
};
builder.Services.AddAutoMapper(assemblies);

string connection = builder.Configuration.GetConnectionString("IdentityConnection");
builder.Services.AddDbContext<ToDoDbContext>(options => options.UseNpgsql(connection));

builder.Services.AddRepositoriesExtension();

builder.Services.AddBusinessLogicServicesExtension();

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
