using System.Collections.Generic;

namespace Run00.NuProductRoslyn
{
	public interface IContractItem
	{
		string Name { get; }
		bool IsPrivate { get; }
		bool IsCodeBlock { get; }
		IEnumerable<IContractItem> Children { get; }

		string ToFullString();
		bool IsMatchedWith(IContractItem item);
		bool IsEquivalentTo(IContractItem node);

		string Title { get; }
	}
}