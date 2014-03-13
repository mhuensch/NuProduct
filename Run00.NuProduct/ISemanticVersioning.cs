using System.Collections.Generic;

namespace Run00.NuProduct
{
	public interface ISemanticVersioning
	{
		VersionChange Calculate(IEnumerable<string> targets, IEnumerable<string> published);
	}
}
