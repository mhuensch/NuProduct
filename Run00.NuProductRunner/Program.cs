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
		public static VersionChange Execute(string[] args)
		{
			IRunner runner = null;

			var dir = Path.GetDirectoryName(typeof(Program).Assembly.Location);
			var container = new WindsorContainer();
			container.Kernel.Resolver.AddSubResolver(new CollectionResolver(container.Kernel));
			container.Install(FromAssembly.InDirectory(new AssemblyFilter(dir)));

			var arguments = new Arguments();
			if (Parser.Default.ParseArguments(args, arguments) == false)
				return null;

			container.Register(Component.For<IArguments>().Instance(arguments).LifestyleSingleton());

			try
			{
				runner = container.Resolve<IRunner>();
				return runner.Execute();
			}
			finally
			{
				if (runner == null)
					container.Release(runner);
			}
		}
	}
}
