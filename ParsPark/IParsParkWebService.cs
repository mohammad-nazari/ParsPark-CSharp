using System.ServiceModel;
using ToolsLib;

namespace ParsPark
{
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IParsParkWebService" in both code and config file together.
	[ServiceContract]
	public interface IParsParkWebService
	{
		[OperationContract]
		string TestServer(string request);
		[OperationContract]
		string DetectLp(string Input);
	}
}
