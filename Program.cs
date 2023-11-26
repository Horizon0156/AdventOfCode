using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

var host = Host.CreateDefaultBuilder()
               .ConfigureAppConfiguration(c => c.AddUserSecrets(Assembly.GetExecutingAssembly(), optional: true))
               .ConfigureServices((context, services) => 
               {    
                    services.Configure<Settings>(context.Configuration.GetSection("Settings"));
                    services.AddTransient<IPuzzleLoader, HttpPuzzleLoader>();
                    services.AddTransient<ISolverLocator, LocalSolverLocator>();
                    services.AddTransient<CommandLineInterpreter>();
               })
               .Build();

return await host.Services
                 .GetRequiredService<CommandLineInterpreter>()
                 .RunAsync(args);