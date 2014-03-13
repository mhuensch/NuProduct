using System.Collections.Generic;

namespace Run00.NuProduct
{
	public interface IPackageReader
	{
		IEnumerable<string> ReadPackage(IEnumerable<string> assemblyPaths);
	}
}
