using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using CourseGuide.Contexts;
using CourseGuide.Services.Server;
using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace CourseGuide
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration; // Armazena a configura��o da aplica��o.
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Configura��o do banco de dados usando PostgreSQL
            services.AddDbContext<AppDBContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

            // Configura��o do Swagger para documenta��o da API
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CourseGuide", Version = "v1" });
            });

            // Adiciona suporte a controleadores e configura op��es de serializa��o JSON
            services.AddControllers().AddJsonOptions(
                c => c.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

            services.AddEndpointsApiExplorer();

            // Configura��o de CORS (Cross-Origin Resource Sharing)
            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.WithOrigins("http://localhost:3000", "http://localhost:5173") // Permite origens espec�ficas
                    .AllowAnyMethod() // Permite qualquer m�todo HTTP
                    .AllowAnyHeader() // Permite qualquer cabe�alho
                    .AllowCredentials(); // Permite o uso de credenciais
            }));

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); // Configura��o do AutoMapper

            // Inje��o de Depend�ncias
            services.InjectDependencies();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage(); // P�gina de erro no desenvolvimento
                app.UseSwagger(); // Habilita o Swagger
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sua API V1"); // Endpoint do Swagger
                    c.DocExpansion(DocExpansion.None); // Expans�o inicial da documenta��o
                    c.DisplayRequestDuration(); // Exibe a dura��o da requisi��o
                    c.EnableDeepLinking(); // Permite links diretos
                    c.EnableFilter(); // Habilita filtro
                    c.ShowExtensions(); // Mostra extens�es
                    c.EnableValidator(); // Habilita validador de requisi��es
                    c.SupportedSubmitMethods(SubmitMethod.Get, SubmitMethod.Post, SubmitMethod.Put, SubmitMethod.Delete, SubmitMethod.Patch); // M�todos suportados
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error"); // Manipulador de erros em produ��o
                app.UseHsts(); // Habilita HSTS
            }

            app.UseHttpsRedirection(); // Redireciona para HTTPS
            app.UseStaticFiles(); // Habilita arquivos est�ticos

            app.UseRouting(); // Configura o roteamento

            app.UseCors("MyPolicy"); // Aplica a pol�tica de CORS

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers(); // Mapeia controladores
            });
        }
    }
}
