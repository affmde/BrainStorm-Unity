using System.Linq;
using System.Threading.Tasks;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

namespace GameFramework_Core.GameFramework_Manager
{
	public class RelayManager : Singleton<RelayManager>
	{
		private bool isHost = false;
		private string joinCode;
		private string ip;
		private int port;
		private byte[] key;
		private byte[] connectionData;
		private byte[] hostConnectionData;
		private byte[] allocationIdBytes;
		private System.Guid allocationId;

		public string GetAllocationId() { return allocationId.ToString(); }
		public string GetConnectionData() { return connectionData.ToString(); }

		public bool IsHost
		{
			get => isHost;
		}
		public async Task<string> CreateRelay(int maxConnections)
		{
			Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxConnections);
			joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

			RelayServerEndpoint dtlsEndpoint = allocation.ServerEndpoints.First(conn=> conn.ConnectionType == "dtls");
			ip = dtlsEndpoint.Host;
			port = dtlsEndpoint.Port;
			allocationId = allocation.AllocationId;
			allocationIdBytes = allocation.AllocationIdBytes;
			connectionData = allocation.ConnectionData;
			key = allocation.Key;
			allocationIdBytes = allocation.AllocationIdBytes;

			isHost = true;

			return joinCode;
		}

		public async Task<bool> JoinRelay(string joinCode)
		{
			Debug.Log("Join relay code: " + joinCode);
			JoinAllocation allocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
			RelayServerEndpoint dtlsEndpoint = allocation.ServerEndpoints.First(conn=> conn.ConnectionType == "dtls");
			ip = dtlsEndpoint.Host;
			port = dtlsEndpoint.Port;
			allocationId = allocation.AllocationId;
			connectionData = allocation.ConnectionData;
			allocationIdBytes = allocation.AllocationIdBytes;
			hostConnectionData = allocation.HostConnectionData;
			key = allocation.Key;

			return true;
		}

		public (byte[] AllocationId, byte[] Key, byte[] ConnectionData, string _dtlsAddress, int _dtlsPort) GetHostConnectionInfo()
		{
			return (allocationIdBytes, key, connectionData, ip, port);
		}
		
		public (byte[] AllocationId, byte[] Key, byte[] ConnectionData, byte[] HostConnectionData, string _dtlsAddress, int _dtlsPort) GetClientConnectionInfo()
		{
			return (allocationIdBytes, key, connectionData, hostConnectionData, ip, port);
		}
	}
}
