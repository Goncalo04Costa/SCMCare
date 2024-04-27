using Microsoft.EntityFrameworkCore;
using WebApplication1;
using WebApplication1.Servicos;

var builder = WebApplication.CreateBuilder(args);

// Gon�alo
//var connectionString = builder.Configuration.GetConnectionString("LigacaoGoncalo");

// Diogo
var connectionString = builder.Configuration.GetConnectionString("LigacaoDiogo");

//Sofia
//var connectionString = builder.Configuration.GetConnectionString("LigacaoSofia");

// Daniela
//var connectionString = builder.Configuration.GetConnectionString("LigacaoDaniela");

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHostedService<CronometroServico>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

