using Run00.NuProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Run00.NuProductVersioning
{
	public class SemanticVersioning : ISemanticVersioning
	{
		VersionChange ISemanticVersioning.Calculate(PackageDefinition neoPackage, PackageDefinition paleoPackage)
		{
			var result = new VersionChange();
			var diff = GetDifferences(neoPackage.MemberKeys, paleoPackage.MemberKeys);
			var neoVersion = GetNeoVersion(diff, paleoPackage.Version);

			return new VersionChange()
			{
				Old = paleoPackage.Version,
				New = neoVersion,
				Differences = diff
			};
		}

		private IEnumerable<Difference> GetDifferences(IEnumerable<string> neoMemberKeys, IEnumerable<string> paleoMemberKeys)
		{
			var differences = new List<Difference>();

			var addedKeys = neoMemberKeys.Where(c => paleoMemberKeys.Contains(c) == false);
			differences.AddRange(addedKeys.Select(a => new Difference() { Name = a, Reason = Difference.ChangeReason.Added }));

			var removedKeys = paleoMemberKeys.Where(c => neoMemberKeys.Contains(c) == false);
			differences.AddRange(removedKeys.Select(a => new Difference() { Name = a, Reason = Difference.ChangeReason.Removed }));

			return differences;
		}

		private Version GetNeoVersion(IEnumerable<Difference> diff, Version paleoVersion)
		{
			if (diff.Where(d => d.Reason == Difference.ChangeReason.Removed).Any())
				return new Version(paleoVersion.Major + 1, 0, 0, 0);

			if (diff.Where(d => d.Reason == Difference.ChangeReason.Added).Any())
				return new Version(paleoVersion.Major, paleoVersion.Minor + 1, 0, 0);

			return new Version(paleoVersion.Major, paleoVersion.Minor, paleoVersion.Build + 1, 0);
		}
	}
}
