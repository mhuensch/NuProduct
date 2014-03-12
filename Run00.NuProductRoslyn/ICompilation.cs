namespace Run00.NuProductRoslyn
{
	public interface ICompilation : IContractItem
	{
		string GetVersion();
		void SetVersion(string value);
	}
}