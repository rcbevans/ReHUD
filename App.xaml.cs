using ElectronNET.API;
using log4net.Config;
using log4net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ReHUD.Interfaces;
using ReHUD.Services;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;

namespace ReHUD
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IHost? host;

        public string LogFilePath { get; private set; }

        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);
#if DEBUG
            Debugger.Launch();
#endif

            LogFilePath = Path.Combine(UserData.dataPath, "ReHUD.log");
            GlobalContext.Properties["LogFilePath"] = LogFilePath;

            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

            try {
                var builder = Host.CreateDefaultBuilder(e.Args)
            .ConfigureWebHostDefaults(webBuilder => {
                Console.WriteLine("Configuring Web Host Defaults");
                webBuilder.UseElectron(e.Args);
#if DEBUG
                webBuilder.UseEnvironment(Environments.Development);
#endif
                webBuilder.UseStartup<Startup>();
            }).ConfigureLogging(builder => {

                builder.ClearProviders();
                builder.AddConsole();
                builder.AddDebug();
                builder.AddEventSourceLogger();
                builder.AddFilter("Microsoft", LogLevel.Warning)
                        .AddFilter("System", LogLevel.Warning)
                        .AddFilter("LoggingConsoleApp.Program", LogLevel.Debug);
            })
            .ConfigureServices(services => {
                services.AddLogging();
                services.AddSignalR();
                services.AddRazorPages();
                services.AddElectron();
                services.AddSingleton<IMessageBoxService, MessageBoxService>();
                services.AddSingleton<IVersionService, VersionService>();
            });

                host = builder.Build();

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
