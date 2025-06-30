using MVCwebApi.Repositories;
using MVCwebApi.Repositories.Interfaces;
using MVCwebApi.Repositories.Interfaces;
using MVCwebApi.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Configuraci�n de servicios
builder.Services.AddScoped<IEmpleadoRepository, EmpleadoRepository>();
builder.Services.AddControllers();

// Configuraci�n de Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Empleados API",
        Version = "v1",
        Description = "API para gesti�n de empleados"
    });
});

var app = builder.Build();

// Habilitar Swagger en todos los entornos (desarrollo y producci�n)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Empleados API v1");
    c.RoutePrefix = "swagger"; // Accede en /swagger
});

app.UseHttpsRedirection();
app.MapControllers();
app.Run();