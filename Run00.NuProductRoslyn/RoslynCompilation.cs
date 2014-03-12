using Roslyn.Compilers.Common;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Run00.NuProductRoslyn
{
	public class RoslynCompilation : ICompilation
	{
		public RoslynCompilation(CommonCompilation compilation)
		{
			Contract.Requires(compilation == null);

			_compilation = compilation;
		}

		string ICompilation.GetVersion()
		{
			return (
				from a in _compilation.Assembly.GetAttributes().AsEnumerable()
				let ca = a.ConstructorArguments.FirstOrDefault()
				where
					a.AttributeClass.Name == "AssemblyVersionAttribute"
					&& ca.Value != null
				select ca.Value.ToString()
			).SingleOrDefault();
		}

		void ICompilation.SetVersion(string value)
		{
			var syntaxTree = _compilation.SyntaxTrees.Where(t => Path.GetFileName(t.FilePath).Equals(_assemblyFileName)).Single();
			var contents = syntaxTree.GetRoot().ToFullString();
			var newContents = Regex.Replace(contents, _assemblyRegexPattern, "[assembly: AssemblyVersion(\"" + value + "\")]");
			newContents = Regex.Replace(newContents, _assemblyFileRegexPattern, "[assembly: AssemblyFileVersion(\"" + value + "\")]");
			File.WriteAllText(syntaxTree.FilePath, newContents);
		}

		bool IContractItem.IsPrivate { get { return false; } }

		bool IContractItem.IsCodeBlock { get { return false; } }

		IEnumerable<IContractItem> IContractItem.Children
		{
			get
			{
				return _compilation.Assembly.GlobalNamespace.GetTypeMembers().AsEnumerable().Select(n => (IContractItem)new RoslynType(n)).Union(
							_compilation.Assembly.GlobalNamespace.GetNamespaceMembers().AsEnumerable().Select(m => (IContractItem)new RoslynNamespace(m)));
			}
		}

		string IContractItem.Name
		{
			get
			{
				var name = _compilation.GlobalNamespace.Name;
				if (string.IsNullOrWhiteSpace(name))
					name = _compilation.Assembly.BaseName;
				return name;
			}
		}

		bool IContractItem.IsMatchedWith(IContractItem item)
		{
			return ((IContractItem)this).Name.Equals(item.Name);
		}

		string IContractItem.ToFullString()
		{
			return _compilation.ToString();
		}

		bool IContractItem.IsEquivalentTo(IContractItem node)
		{
			return false;
		}

		string IContractItem.Title
		{
			get { return ((IContractItem)this).Name; }
		}

		private readonly CommonCompilation _compilation;
		private const string _assemblyFileName = @"AssemblyInfo.cs";
		private const string _assemblyRegexPattern = @"\[assembly\: AssemblyVersion\(""(\d{1,})\.(\d{1,})\.(\d{1,})\.(\d{1,})""\)\]";
		private const string _assemblyFileRegexPattern = @"\[assembly\: AssemblyFileVersion\(""(\d{1,})\.(\d{1,})\.(\d{1,})\.(\d{1,})""\)\]";
	}
}
