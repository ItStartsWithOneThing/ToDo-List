using Microsoft.EntityFrameworkCore;
using ToDo_List.Models.DataBase;
using ToDo_List.Models.DataBase.Repositories;
using ToDo_List.Models.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

string connection = builder.Configuration.GetConnectionString("IdentityConnection");
builder.Services.AddDbContext<ToDoDbContext>(options => options.UseNpgsql(connection));

builder.Services.AddScoped<IReadRepository, ReadRepository>();
builder.Services.AddScoped<IWriteRepository, WriteRepository>();

builder.Services.AddScoped<ITaskCardService, TaskCardService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
