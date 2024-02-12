using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DreamCatcher.Core.Servicees
{
    public static class ServiceRegistator
    {
        public static void Register(ServiceCollection services)
        {
            RegisterServices(services);
        }

        private static void RegisterServices(ServiceCollection services)
        {
            var serviceList = typeof(IService).GetTypeInfo().Assembly.DefinedTypes
            .Where(t => typeof(IService).GetTypeInfo().IsAssignableFrom(t.AsType()) && t.IsClass);

            foreach (var service in serviceList)
            {
                Type? interfaceType = service.GetInterfaces().Where(x => x != typeof(IService)).FirstOrDefault();
                if (interfaceType != null)
                {
                    services.AddSingleton(interfaceType, service.AsType());
                }
            }
        }
    }
}
