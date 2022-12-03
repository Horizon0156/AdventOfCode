using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder()
               .ConfigureServices((context, services) => 
               {
                    services.Configure<Settings>(s => context.Configuration.GetSection("Settings").Bind(s));
                    services.AddTransient<IPuzzleLoader, HttpPuzzleLoader>();
                    services.AddTransient<ISolverLocator, LocalSolverLocator>();
                    services.AddTransient<CommandLineInterpreter>();
               })
               .Build();

return await host.Services
                 .GetRequiredService<CommandLineInterpreter>()
                 .RunAsync(args);