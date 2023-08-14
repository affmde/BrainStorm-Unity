using System.Linq;
using System.Threading.Tasks;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;

namespace GameFramework_Core.GameFramework_Manager
{
	public class RelayManager : Singleton<RelayManager>
	{
		private string joinCode;
		private string ip;
		private int port;
		private byte[] connectionData;
		private System.Guid allocationId;
		public async Task<string> CreateRelay(int maxConnections)
		{
			Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxConnections);
			joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

			RelayServerEndpoint dtlsEndpoint = allocation.ServerEndpoints.First(conn=> conn.ConnectionType == "dtls");
			ip = dtlsEndpoint.Host;
			port = dtlsEndpoint.Port;
			allocationId = allocation.AllocationId;
			connectionData = allocation.ConnectionData;

			return joinCode;
		}

		public async Task<bool> JoinRelay(string joinCode)
		{
			JoinAllocation allocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

			RelayServerEndpoint dtlsEndpoint = allocation.ServerEndpoints.First(conn=> conn.ConnectionType == "dtls");
			ip = dtlsEndpoint.Host;
			port = dtlsEndpoint.Port;
			allocationId = allocation.AllocationId;
			connectionData = allocation.ConnectionData;

			return true;
		}
	}
}
