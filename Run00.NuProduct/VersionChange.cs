using System;
using System.Collections.Generic;

namespace Run00.NuProduct
{
	public class VersionChange
	{
		public Version Change { get; set; }

		public IEnumerable<Difference> Differences { get; set; }

	}
}
