using Dapr.Client;
using FineCollectionService.DomainServices;
using FineCollectionService.Proxies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FineCollectionService
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
            services.AddDaprClient(builder => builder
                .UseHttpEndpoint($"http://localhost:3601")
                .UseGrpcEndpoint($"http://localhost:60001"));
            
            services.AddSingleton<IFineCalculator, HardCodedFineCalculator>();

             services.AddSingleton<VehicleRegistrationService>(_ => 
                new VehicleRegistrationService(DaprClient.CreateInvokeHttpClient(
                    "vehicleregistrationservice", "http://localhost:3601")));

            services.AddControllers().AddDapr();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseCloudEvents();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapSubscribeHandler();
                endpoints.MapControllers();
            });
        }
    }
}
