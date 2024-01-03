using System.Diagnostics;
using System.IO;
using Ata.DeloSled.Server.Handlers;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace Ata.DeloSled.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining(typeof(ForecastQueryHandler)));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapPost("/rpc",
                    async context =>
                    {

                        StreamReader reader = new StreamReader(context.Request.Body);
                        string requestJson = await reader.ReadToEndAsync();

                        JsonSerializerSettings settings = new JsonSerializerSettings()
                        {
                            PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                            NullValueHandling = NullValueHandling.Include,
                            TypeNameHandling = TypeNameHandling.All
                        };

                        object? requestObject = JsonConvert.DeserializeObject(requestJson, settings);

                        IMediator? mediator = context.RequestServices.GetService<IMediator>();

                        Debug.Assert(mediator != null, nameof(mediator) + " != null");
                        object? commandQueryResponse = await mediator.Send(requestObject);

                        string responseJson = JsonConvert.SerializeObject(commandQueryResponse, settings);

                        await context.Response.WriteAsync(responseJson);
                    });
                    // .RequireAuthorization();
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
