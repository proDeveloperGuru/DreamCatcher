using DreamCatcher.Core.Servicees;
using DreamCatcher.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace DreamCatcher
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ServiceProvider _serviceProvider;

        public App()
        {
            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(ServiceCollection services)
        {
            RepositoryRegistator.Register(services);
            ServiceRegistator.Register(services);
            RegisterWindows(services);
        }

        private void RegisterWindows(ServiceCollection services)
        {
            services.AddSingleton<MainWindow>();
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = _serviceProvider.GetService<MainWindow>();
            if(mainWindow != null)             
                mainWindow.Show();
        }
    }
}
