using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CacheQ;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
	{
		/// <summary>
		/// Adds all cache policies in specified assemblies
		/// </summary>
		/// <param name="services">The collection of services</param>
		/// <param name="assemblies">The assemblies to scan</param>
		/// <param name="lifetime"> CachePolicies life time. The default is scoped (per-request in web applications)</param>
		/// <returns></returns>
		public static IServiceCollection AddCachePoliciesFromAssemblies(
			this IServiceCollection services, 
			IEnumerable<Assembly> assemblies, 
			ServiceLifetime lifetime = ServiceLifetime.Scoped)
		{
			foreach (var assembly in assemblies)
			{
				services.AddCachePoliciesFromAssembly(assembly, lifetime);
			}

			return services;
		}

		/// <summary>
		/// Adds all cache policies in specified assembly
		/// </summary>
		/// <param name="services">The collection of services</param>
		/// <param name="assembly">The assembly to scan</param>
		/// <param name="lifetime">CachePolicies life time. The default is scoped (per-request in web application)</param>
		/// <returns></returns>
		public static IServiceCollection AddCachePoliciesFromAssembly(
			this IServiceCollection services, 
			Assembly assembly, 
			ServiceLifetime lifetime = ServiceLifetime.Scoped)
		{
            FindCachePoliciesInAssembly(assembly.GetTypes())
                .ToList()
                .ForEach(scanResult => services.AddScanResult(scanResult, lifetime));

			return services;
		}

		/// <summary>
		/// Helper method to register a CachePolicy from an AssemblyScanner result
		/// </summary>
		/// <param name="services">The collection of services</param>
		/// <param name="scanResult">The scan result</param>
		/// <param name="lifetime">CachePolicies life time. The default is scoped (per-request in web applications)</param>
		/// <returns></returns>
		private static IServiceCollection AddScanResult(
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
