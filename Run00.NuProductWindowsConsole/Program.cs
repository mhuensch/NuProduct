
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using Castle.Windsor.Installer;
using CommandLine;
using Run00.NuProduct;
using System;
using System.IO;

namespace Run00.NuProductWindowsConsole
{
	public class Program
	{
		//TODO: undo the return value from Main
		public static void Main(string[] args)
		{
			Run(args);
			if (Environment.UserInteractive)
				Console.ReadKey();
		}

		public static VersionChange Run(string[] args)
		{
			IRunner runner = null;

			var container = new WindsorContainer();
			container.Kernel.Resolver.AddSubResolver(new CollectionResolver(container.Kernel));
			container.Install(FromAssembly.InDirectory(new AssemblyFilter(Directory.GetCurrentDirectory())));

			var arguments = new Arguments();
			if (Parser.Default.ParseArguments(args, arguments) == false)
				return null;

			container.Register(Component.For<Arguments>().Instance(arguments));

			try
			{
				runner = container.Resolve<IRunner>();
				var result = runner.Execute();
				Console.WriteLine(result);

				return result;
			}
			finally
			{
				if (runner == null)
					container.Release(runner);
			}
		}
	}
}
