﻿using Microsoft.AspNetCore.Identity;
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
            // Configuração do Entity Framework e do contexto do banco de dados
            //var connectionString = Configuration.GetConnectionString("LigacaoGoncalo");
            //var connectionString = Configuration.GetConnectionString("LigacaoDiogo");
            //var connectionString = Configuration.GetConnectionString("LigacaoSofia");
            //var connectionString = Configuration.GetConnectionString("LigacaoDaniela");
            var connectionString = Configuration.GetConnectionString("LigacaoDanielaB");
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));

           
            // Registo do serviço AppSettings
            services.Configure<AppSettings>(Configuration);

            

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
