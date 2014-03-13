using Run00.NuProduct;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Run00.NuProductVersioning
{
	public class SemanticVersioning : ISemanticVersioning
	{
		VersionChange ISemanticVersioning.Calculate(IEnumerable<string> targets, IEnumerable<string> published)
		{
			var result = new VersionChange();
			var diff = GetDifferences(targets, published);
			var update = GetVersionUpdate(diff);
			return new VersionChange() { Change = update, Differences = diff };
		}

		private IEnumerable<Difference> GetDifferences(IEnumerable<string> targets, IEnumerable<string> published)
		{
			var differences = new List<Difference>();

			var addedKeys = targets.Where(c => published.Contains(c) == false);
			differences.AddRange(addedKeys.Select(a => new Difference() { Name = a, Reason = Difference.ChangeReason.Added }));

			var removedKeys = published.Where(c => targets.Contains(c) == false);
			differences.AddRange(removedKeys.Select(a => new Difference() { Name = a, Reason = Difference.ChangeReason.Removed }));

			return differences;
		}

		private Version GetVersionUpdate(IEnumerable<Difference> diff)
		{
			if (diff.Where(d => d.Reason == Difference.ChangeReason.Removed).Any())
				return new Version(1, 0, 0, 0);

			if (diff.Where(d => d.Reason == Difference.ChangeReason.Added).Any())
				return new Version(0, 1, 0, 0);

			return new Version(0, 0, 1, 0);
		}
	}
}
