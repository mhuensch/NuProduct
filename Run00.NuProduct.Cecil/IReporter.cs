
namespace Run00.NuProduct.Cecil
{
	public interface IReporter
	{
		void Write(string message);

		void WriteLine(string message);

		void WriteWarning(string message);

		void WriteError(string message);
	}
}
