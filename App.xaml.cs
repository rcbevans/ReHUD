using ElectronNET.API;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Windows;

namespace ReHUD
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IHost? host;

        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);
#if DEBUG
            Debugger.Launch();
#endif

            try {
                host = Host.CreateDefaultBuilder(e.Args)
            .ConfigureWebHostDefaults(webBuilder => {
                Console.WriteLine("Configuring Web Host Defaults");
                webBuilder.UseElectron(e.Args);
#if DEBUG
                webBuilder.UseEnvironment(Environments.Development);
#endif
                webBuilder.UseStartup<Startup>();
            }).ConfigureLogging((hostingContext, logging) => {
                logging.ClearProviders();
                logging.AddConsole();
                logging.AddDebug();
                logging.AddEventSourceLogger();
                logging.AddFilter("Microsoft", LogLevel.Warning)
                        .AddFilter("System", LogLevel.Warning)
                        .AddFilter("LoggingConsoleApp.Program", LogLevel.Debug);
            }).Build();

                Task.Run(() => host.StartAsync());
            }
            catch (Exception ex) {
                Console.WriteLine($"An error occurred: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }
        }

        protected override void OnExit(ExitEventArgs e) {
            base.OnExit(e);

            host?.Dispose();
        }
    }

}
