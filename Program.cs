using GestaoDesignerDeMemorias.Data;
using GestaoDesignerDeMemorias.Middleware;
using GestaoDesignerDeMemorias.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Controllers
builder.Services.AddControllers();

// Registra o serviço de PDF
builder.Services.AddScoped<PropostaPdfService>();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "Gestão Designer de Memórias API", 
        Version = "v1",
        Description = "API para automatizar captação via WhatsApp, organizar pedidos de identidade visual/digital, briefings e pagamentos"
    });
});

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();