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
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using WebApplication1.Modelos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

//services.AddScoped<Services.JwtService>();

//services
//      .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//      .AddJwtBearer(options =>
//      {
//          options.TokenValidationParameters = new TokenValidationParameters()
//          {
//              ValidateIssuer = true,
//              ValidateAudience = true,
//              ValidateLifetime = true,
//              ValidateIssuerSigningKey = true,
//              ValidAudience = Configuration["Jwt:Audience"],
//              ValidIssuer = Configuration["Jwt:Issuer"],
//              IssuerSigningKey = new SymmetricSecurityKey(
//                  Encoding.UTF8.GetBytes(Configuration["Jwt:Key"])
//                  )
//          };
//      });

//app.UseAuthentication();


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
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddDbContext<UCCIContext>(options =>
                        options.UseSqlServer(hostContext.Configuration.GetConnectionString("LigacaoGoncalo")));

                    services.AddIdentityCore<IdentityUser>(options =>
                    {
                        options.SignIn.RequireConfirmedAccount = false;
                        options.User.RequireUniqueEmail = true;
                        options.Password.RequireDigit = false;
                        options.Password.RequiredLength = 6;
                        options.Password.RequireNonAlphanumeric = false;
                        options.Password.RequireUppercase = false;
                        options.Password.RequireLowercase = false;
                    }).AddEntityFrameworkStores<UCCIContext>();
                });
    }
}
