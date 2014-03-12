using Roslyn.Compilers.Common;
using Roslyn.Compilers.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Run00.NuProductRoslyn
{
	public class RoslynSyntaxNode : IContractItem
	{
		public RoslynSyntaxNode(CommonSyntaxNode node)
		{
			_node = node;
		}

		bool IContractItem.IsPrivate
		{
			get
			{
				switch (((SyntaxNode)_node).Kind)
				{
					case SyntaxKind.ClassDeclaration:
						return ((ClassDeclarationSyntax)(_node)).Modifiers.Any(m => m.Kind == SyntaxKind.PrivateKeyword || m.Kind == SyntaxKind.InternalKeyword);
					case SyntaxKind.InterfaceDeclaration:
						return ((InterfaceDeclarationSyntax)(_node)).Modifiers.Any(m => m.Kind == SyntaxKind.PrivateKeyword || m.Kind == SyntaxKind.InternalKeyword);
					case SyntaxKind.StructDeclaration:
						return ((StructDeclarationSyntax)(_node)).Modifiers.Any(m => m.Kind == SyntaxKind.PrivateKeyword || m.Kind == SyntaxKind.InternalKeyword);
					case SyntaxKind.EnumDeclaration:
						return ((EnumDeclarationSyntax)(_node)).Modifiers.Any(m => m.Kind == SyntaxKind.PrivateKeyword || m.Kind == SyntaxKind.InternalKeyword);
					case SyntaxKind.DelegateDeclaration:
						return ((DelegateDeclarationSyntax)(_node)).Modifiers.Any(m => m.Kind == SyntaxKind.PrivateKeyword || m.Kind == SyntaxKind.InternalKeyword);
					case SyntaxKind.DestructorDeclaration:
						return ((DestructorDeclarationSyntax)(_node)).Modifiers.Any(m => m.Kind == SyntaxKind.PrivateKeyword || m.Kind == SyntaxKind.InternalKeyword);
					case SyntaxKind.EventDeclaration:
						return ((EventDeclarationSyntax)(_node)).Modifiers.Any(m => m.Kind == SyntaxKind.PrivateKeyword || m.Kind == SyntaxKind.InternalKeyword);
					case SyntaxKind.MethodDeclaration:
						return ((MethodDeclarationSyntax)(_node)).Modifiers.Any(m => m.Kind == SyntaxKind.PrivateKeyword || m.Kind == SyntaxKind.InternalKeyword);
					case SyntaxKind.PropertyDeclaration:
						return ((PropertyDeclarationSyntax)(_node)).Modifiers.Any(m => m.Kind == SyntaxKind.PrivateKeyword || m.Kind == SyntaxKind.InternalKeyword);
				}

				return false;
			}
		}

		bool IContractItem.IsCodeBlock
		{
			get { return (SyntaxKind)_node.Kind == SyntaxKind.Block; }
		}

		IEnumerable<IContractItem> IContractItem.Children
		{
			get { return _node.ChildNodes().Select(c => new RoslynSyntaxNode(c)); }
		}

		string IContractItem.Name
		{
			get
			{
				switch (((SyntaxNode)_node).Kind)
				{
					case SyntaxKind.ClassDeclaration:
						return ((ClassDeclarationSyntax)(_node)).Identifier.Value.ToString();
					case SyntaxKind.InterfaceDeclaration:
						return ((InterfaceDeclarationSyntax)(_node)).Identifier.Value.ToString();
					case SyntaxKind.StructDeclaration:
						return ((StructDeclarationSyntax)(_node)).Identifier.Value.ToString();
					case SyntaxKind.EnumDeclaration:
						return ((EnumDeclarationSyntax)(_node)).Identifier.Value.ToString();
					default:
						return Guid.NewGuid().ToString();
				}
			}
		}

		bool IContractItem.IsMatchedWith(IContractItem item)
		{
			var privateNode = item as RoslynSyntaxNode;
			if (privateNode != null && _node.IsEquivalentTo(privateNode._node, true))
				return true;

			return ((IContractItem)this).Name.Equals(item.Name);
		}

		string IContractItem.ToFullString()
		{
			return _node.ToFullString();
		}

		bool IContractItem.IsEquivalentTo(IContractItem node)
		{
			var privateNode = node as RoslynSyntaxNode;
			if (privateNode == null)
				return false;

			return _node.IsEquivalentTo(privateNode._node, false);
		}

		string IContractItem.Title
		{
			get
			{
				var guid = Guid.Empty;
				if (Guid.TryParse(((IContractItem)this).Name, out guid) == false)
					return ((IContractItem)this).Name;

				var x = _node.Parent as BaseTypeDeclarationSyntax;
				if (x == null)
					return ((IContractItem)this).Name;

				return _node.Parent.GetType().Name + ": " +
					x.Identifier.Value +
					" line: " + _node.Span.Start + " to " + _node.Span.End;
			}
		}

		private readonly CommonSyntaxNode _node;
	}
}
