
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using ToDo_List.Models.DataBase;
using ToDo_List.Models.DataBase.Repositories;
using ToDo_List.Models.MappingProfiles;
using ToDo_List.Models.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo { Title = "ToDo-List", Version = "v1" }));

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

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "ToDo-List v1");
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
