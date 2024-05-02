/*
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Servicos;
using WebApplication1;

var builder = WebApplication.CreateBuilder(args);


// Gonçalo
//var connectionString = builder.Configuration.GetConnectionString("LigacaoGoncalo");

// Diogo
var connectionString = builder.Configuration.GetConnectionString("LigacaoDiogo");

//Sofia
//var connectionString = builder.Configuration.GetConnectionString("LigacaoSofia");

// Daniela
//var connectionString = builder.Configuration.GetConnectionString("LigacaoDaniela");


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
*/

 
using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace WebApplication1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}