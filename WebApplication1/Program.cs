// No seu arquivo Startup.cs

using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Servicos;
using WebApplication1;

var builder = WebApplication.CreateBuilder(args);

// Gonçalo
var connectionString = builder.Configuration.GetConnectionString("LigacaoGoncalo");

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

// Register AuthService as a scoped service
builder.Services.AddScoped(AuthService);

object AuthService(IServiceProvider provider)
{
    throw new NotImplementedException();
}

// Adicione serviços ao contêiner.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure o pipeline de solicitação HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
