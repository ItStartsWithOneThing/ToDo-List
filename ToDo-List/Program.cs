
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using ToDo_List.Controllers.Filters;
using ToDo_List.Models.DataBase;
using ToDo_List.Models.DataBase.Repositories;
using ToDo_List.Models.MappingProfiles;
using ToDo_List.Models.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews()
    .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNameCaseInsensitive = true)
    .AddMvcOptions(options =>
    {
        options.Filters.Add<ValidatonActionFilter>();
    });

builder.Services.AddCors(options => options.AddPolicy("MyCORS", builder => builder
                    .WithOrigins("https://google.com")
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

builder.Services.AddScoped<IReadRepository, ReadRepository>();
builder.Services.AddScoped<IWriteRepository, WriteRepository>();

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
