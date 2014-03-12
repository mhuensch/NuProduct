
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Run00.NuProduct;
using System.IO;

namespace Run00.NuProductWindowsConsole
{
	public class Program
	{
		public static void Main(string[] args)
		{
			Run(args);
		}

		public static VersionChange Run(string[] args)
		{
			VersionChange result = null;
			IRunner runner = null;

			var container = new WindsorContainer();
			container.Kernel.Resolver.AddSubResolver(new CollectionResolver(container.Kernel));
			try
			{
				container.Install(FromAssembly.InDirectory(new AssemblyFilter(Directory.GetCurrentDirectory())));
				runner = container.Resolve<IRunner>();
				result = runner.Execute(args);
			}
			finally
			{
				if (runner == null)
					container.Release(runner);
			}

			return result;
		}
	}
}
