using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Modelos;
using WebApplication1;
using WebApplication1.Servicos;
using Microsoft.AspNetCore.Identity;


var builder = WebApplication.CreateBuilder(args);

// Gonçalo
//var connectionString = builder.Configuration.GetConnectionString("LigacaoGoncalo");

//Sofia
var connectionString = builder.Configuration.GetConnectionString("LigacaoSofia");

// Daniela
//var connectionString = builder.Configuration.GetConnectionString("LigacaoDaniela");

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddIdentity<UserFuncionario, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<JwtService>();

// Add services to the container.

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
