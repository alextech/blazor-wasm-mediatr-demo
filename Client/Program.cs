using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using System.Text;
using MediatR;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Ata.DeloSled.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddMediatR(typeof(RpcBehavior<,>).GetTypeInfo().Assembly);
            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RpcBehavior<,>));
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            await builder.Build().RunAsync();
        }
    }
}
