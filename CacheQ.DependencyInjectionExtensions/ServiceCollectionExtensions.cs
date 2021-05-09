using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CacheQ;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Internal;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
	{
        /// <summary>
        /// Adds all cache policies in specified assemblies
        /// </summary>
        /// <param name="services">The collection of services</param>
        /// <param name="assemblies">The assemblies to scan</param>
        /// <returns></returns>
        public static IServiceCollection AddCachePoliciesFromAssemblies(
            this IServiceCollection services,
            IEnumerable<Assembly> assemblies)
        {
            var lifetime = ServiceLifetime.Scoped;
            FindCachePoliciesInAssembly(assemblies.SelectMany(x => x.GetTypes()))
                .ToList()
                .ForEach(scanResult => services.Register(scanResult, lifetime));

            return AddDefaultDependencies(services);
        }

        /// <summary>
        /// Adds all cache policies in specified assembly
        /// </summary>
        /// <param name="services">The collection of services</param>
        /// <param name="assembly">The assembly to scan</param>
        /// <returns></returns>
        public static IServiceCollection AddCacheQ(
			this IServiceCollection services, 
			Assembly assembly,
			Action<ICacheQConfigurator> configure = null)
        {
            var lifetime = ServiceLifetime.Scoped;
            FindCachePoliciesInAssembly(assembly.GetTypes())
                .ToList()
                .ForEach(scanResult => services.Register(scanResult, lifetime));

            return AddDefaultDependencies(services, configure);
        }

        private static IServiceCollection AddDefaultDependencies(
			IServiceCollection services, 
			Action<ICacheQConfigurator> configure = null)
        {
            services.AddSingleton<ICacheExpirationResolver, CacheExpirationResolver>();
            services.AddSingleton<ICacheManager, CacheManager>();
            services.AddSingleton<ISystemClock, SystemClock>();
            services.AddSingleton(serviceProvider =>
            {
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                var cacheExpirationSettings = new CacheExpirationSettings();
                configuration.GetSection("CacheQ").Bind(cacheExpirationSettings);

                return cacheExpirationSettings;
            });

            var builder = new CacheQConfigurator(services);
            builder.UsePrefixKey(type =>
            {
                return type.Assembly.GetName().Name + "," + type.FullName;
            });
            configure?.Invoke(builder);

            return services;
        }

        /// <summary>
        /// Adds all cache policies in specified assembly
        /// </summary>
        /// <param name="configurator">The collection of services</param>
        /// <returns></returns>
        public static ICacheQConfigurator UsePrefixKey(
			this ICacheQConfigurator configurator, 
			Func<Type, string> prefixFunc)
		{
			configurator.Services.AddSingleton(new PrefixKeyResolver()
			{
				Func = prefixFunc
			});

			return configurator;
		}

		/// <summary>
		/// Helper method to register a CachePolicy from an AssemblyScanner result
		/// </summary>
		/// <param name="services">The collection of services</param>
		/// <param name="scanResult">The scan result</param>
		/// <param name="lifetime">CachePolicies life time. The default is scoped (per-request in web applications)</param>
		/// <returns></returns>
		private static IServiceCollection Register(
			this IServiceCollection services, 
			AssemblyScanResult scanResult, 
			ServiceLifetime lifetime)
		{
			services.Add(
				new ServiceDescriptor(
					scanResult.InterfaceType,
					scanResult.CachePolicyType,
					lifetime));

			services.Add(
				new ServiceDescriptor(
					scanResult.CachePolicyType,
					scanResult.CachePolicyType,
					lifetime));

			return services;
		}

		private static IEnumerable<AssemblyScanResult> FindCachePoliciesInAssembly(IEnumerable<Type> types)
        {
			return 
				from type 
				in types
					where !type.IsAbstract && !type.IsGenericTypeDefinition
					let interfaces = type.GetInterfaces()
					let genericInterfaces = interfaces.Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICachePolicy<>))
					let matchingInterface = genericInterfaces.FirstOrDefault()
					where matchingInterface != null
					select new AssemblyScanResult(matchingInterface, type);
		}
	}
}
