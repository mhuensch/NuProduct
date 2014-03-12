using Roslyn.Compilers.Common;
using System.Collections.Generic;
using System.Linq;

namespace Run00.NuProductRoslyn
{
	public class RoslynType : IContractItem
	{
		public RoslynType(INamedTypeSymbol type)
		{
			_type = type;
		}

		bool IContractItem.IsPrivate
		{
			get
			{
				return _type.DeclaredAccessibility == CommonAccessibility.Private
					|| _type.DeclaredAccessibility == CommonAccessibility.Internal
					|| _type.DeclaredAccessibility == CommonAccessibility.ProtectedAndInternal;
			}
		}

		bool IContractItem.IsCodeBlock { get { return false; } }

		IEnumerable<IContractItem> IContractItem.Children
		{
			get
			{
				return _type.DeclaringSyntaxNodes.AsEnumerable().Select(n => new RoslynSyntaxNode(n));
			}
		}

		string IContractItem.Name
		{
			get { return _type.ToDisplayString(); }
		}

		bool IContractItem.IsMatchedWith(IContractItem item)
		{
			return ((IContractItem)this).Name.Equals(item.Name);
		}

		string IContractItem.ToFullString()
		{
			return _type.ToDisplayString();
		}

		bool IContractItem.IsEquivalentTo(IContractItem node)
		{
			return false;
		}

		string IContractItem.Title
		{
			get { return _type.ToDisplayString(); }
		}

		private readonly INamedTypeSymbol _type;
	}
}
