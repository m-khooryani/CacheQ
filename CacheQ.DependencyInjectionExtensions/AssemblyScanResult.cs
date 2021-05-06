using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal class AssemblyScanResult
	{
		public Type InterfaceType { get; }
		public Type CachePolicyType { get; }

		public AssemblyScanResult(
			Type interfaceType, 
			Type cachePolicyType)
		{
			InterfaceType = interfaceType;
			CachePolicyType = cachePolicyType;
		}
	}
}
