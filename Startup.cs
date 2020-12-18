using EVendas.Repository;
using EVendas.Repository.Interfaces;
using EVendas.Service;
using EVendas.Service.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace eVendasEstoque
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {        
            services.AddDbContext<RepositoryContext>(options => options.UseInMemoryDatabase(databaseName: "EVendasDataBaseEstoque"));           
            
            services.AddTransient<IProdutoRepository, ProdutoRepository>();
                
            services.AddTransient<IProdutoService, ProdutoService>();

            services.AddTransient<IServiceBusProducer, ServiceBusProducer>();
            
            services.AddSingleton<IServiceBusConsumer, ServiceBusConsumer>();

            services.AddSwaggerGen();

            services.AddControllers();
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "EVendasEstoque");
                c.RoutePrefix = string.Empty;
            });

            var bus = app.ApplicationServices.GetService<IServiceBusConsumer>();
            bus.RegisterOnMessageHandlerAndReceiveMessages();
        }
    }
}
