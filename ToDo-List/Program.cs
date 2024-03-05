
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Reflection;
using ToDo_List.Controllers.ExceptionResolvers;
using ToDo_List.Controllers.Extensions;
using ToDo_List.Controllers.Filters;
using ToDo_List.Controllers.Middlewares;
using ToDo_List.Models.DataBase;
using ToDo_List.Models.MappingProfiles;
using ToDo_List.Models.Services.Auth.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AuthOptionsModel>(options =>
    builder.Configuration.GetSection("AuthOptions").Bind(options));

builder.Services.AddAuthorizationExtension();
builder.Services.AddJwtAuthenticationExtension(builder.Configuration);

builder.Services.AddControllersWithViews()
    .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNameCaseInsensitive = true)
    .AddMvcOptions(options =>
    {
        options.Filters.Add<ValidationActionFilter>();
        options.Filters.Add<ExceptionFilter>();
    });

builder.Services.AddScoped<IExceptionResolver, GlobalExceptionResolver>();

builder.Services.AddCors(options => options.AddPolicy("MyCORS", policyBuilder => policyBuilder
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

app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
    HttpOnly = HttpOnlyPolicy.Always,
    Secure = CookieSecurePolicy.Always
});

app.UseStaticFiles();

app.UseRouting();

app.UseCors("MyCORS");

app.UseMiddleware<TokenHandlerMiddleware>();

app.UseStatusCodePages(async context =>
{
    var response = context.HttpContext.Response;

    var isUnauthorized = response.StatusCode == (int)HttpStatusCode.Unauthorized;
    var isNotAPIRoute = !context.HttpContext.Request.Path.ToString().Contains("/api/");

    if (isUnauthorized && isNotAPIRoute)
    {
        response.Redirect("/Home/Login");
    }
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");

app.Run();