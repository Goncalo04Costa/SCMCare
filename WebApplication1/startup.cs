using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Modelos;
using WebApplication1.Servicos;

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
            // Configuração do Entity Framework e do contexto da base de dados
            //var connectionString = Configuration.GetConnectionString("LigacaoGoncalo");
            var connectionString = Configuration.GetConnectionString("LigacaoDiogo2");
            //var connectionString = builder.Configuration.GetConnectionString("LigacaoSofia");
            //var connectionString = builder.Configuration.GetConnectionString("LigacaoDaniela");
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Registo do ASP.NET Core Identity e configuração da base de dados
            services.AddIdentity<UserFuncionario, IdentityRole>()
                    .AddEntityFrameworkStores<AppDbContext>()
                    .AddSignInManager<SignInManager<UserFuncionario>>();

            // Registo do serviço JwtSettings
            services.Configure<JwtSettings>(Configuration.GetSection("jwt"));

            // Registo do serviço AppSettings
            services.Configure<AppSettings>(Configuration);

            // Registo da interface IJwtService e a sua implementação JwtService
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
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(env.ContentRootPath, "HTML"))
            });

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
