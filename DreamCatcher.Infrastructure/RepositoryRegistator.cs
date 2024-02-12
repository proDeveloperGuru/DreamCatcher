using DreamCatcher.Infrastructure.Database;
using DreamCatcher.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DreamCatcher.Infrastructure
{
    public static class RepositoryRegistator
    {
        public static void Register(ServiceCollection services)
        {
            RegisterRepositories(services);
        }

        private static void RegisterRepositories(ServiceCollection services)
        {
            services.AddSingleton<IDatabaseFactory, DatabaseFactory>();

            var repositoryList = typeof(RepositoryBase).GetTypeInfo().Assembly.DefinedTypes
            .Where(t => typeof(RepositoryBase).GetTypeInfo().IsAssignableFrom(t.AsType()) && t.IsClass);

            foreach (var repository in repositoryList)
            {
                Type? interfaceType = repository.GetInterfaces().Where(x => x != typeof(IRepositoryBase)).FirstOrDefault();
                if (interfaceType != null)
                {
                    services.AddSingleton(interfaceType, repository.AsType());
                }
            }
        }
    }
}
