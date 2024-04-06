using RegrasNegocio;
using System.Data.SqlClient;
using static System.Runtime.InteropServices.JavaScript.JSType;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/testing", (HttpContext context) => { 
    int id = 0;

    if (Int32.TryParse(context.Request.Query["id"], out id))
    {
        // !!! NÃO FUNCIONA REVER COM O STOR
        return Regras.ObterFichaUtente(id);

        return id;
    }
    else
    {
        throw new Exception();
    }
});

app.MapPost("/testeAvaliacao", (HttpContext context) => {
    int UtentesId = Int32.Parse(context.Request.Query["uid"]);
    int FuncionariosId = Int32.Parse(context.Request.Query["fid"]);
    string? Analise = context.Request.Query["an"];
    DateTime Data = DateTime.Parse(context.Request.Query["dat"]);
    int TipoAvaliacaoId = Int32.Parse(context.Request.Query["tai"]);
    string? AuscultacaoPolmunar = context.Request.Query["ap"];
    string? AucultacaoCardiaca = context.Request.Query["ac"];

    return 1;
});

app.Run();
