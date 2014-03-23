using System;
using System.Collections.Generic;
using System.Linq;

namespace Run00.NuProduct
{
	public class VersionChange
	{
		public Version Change { get; set; }

		public IEnumerable<Difference> Differences { get; set; }

		public override string ToString()
		{
			return "Version Increase:" + Change +
				" Changes:" + Differences.Where(d => d.Reason == Difference.ChangeReason.Removed).Count() +
				" Updates:" + Differences.Where(d => d.Reason == Difference.ChangeReason.Added).Count();
		}

	}
}
