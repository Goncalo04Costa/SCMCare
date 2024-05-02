using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Modelos;
using WebApplication1.Servicos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using WebApplication1.Services;

namespace WebApplication1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Configuração do Entity Framework e do contexto do banco de dados
            var connectionString = Configuration.GetConnectionString("LigacaoGoncalo");
            //var connectionString = Configuration.GetConnectionString("LigacaoDiogo");
            //var connectionString = builder.Configuration.GetConnectionString("LigacaoSofia");
            //var connectionString = builder.Configuration.GetConnectionString("LigacaoDaniela");
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Registro do ASP.NET Core Identity e configuração do banco de dados
            services.AddIdentity<UserFuncionario, IdentityRole>()
                    .AddEntityFrameworkStores<AppDbContext>()
                    .AddSignInManager<SignInManager<UserFuncionario>>(); // Adicione esta linha para configurar o SignInManager

            // Registro do serviço JwtSettings
            services.Configure<JwtSettings>(Configuration.GetSection("jwt"));

            // Registro do serviço AppSettings
            services.Configure<AppSettings>(Configuration); // Adicione esta linha para registrar o AppSettings

            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));


            // Registro da interface IJwtService e sua implementação JwtService
            services.AddScoped<IJwtService, JwtService>();

            // Outros serviços
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddScoped<NotificacoesServico>();
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
