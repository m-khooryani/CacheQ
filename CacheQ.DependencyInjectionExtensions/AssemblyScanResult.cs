using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Extensions.DependencyInjection
{
	[ExcludeFromCodeCoverage]
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
