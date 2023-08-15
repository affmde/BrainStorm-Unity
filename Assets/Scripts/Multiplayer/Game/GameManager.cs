using GameFramework_Core.GameFramework_Manager;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;

public class MultiplayerGameManager : MonoBehaviour
{
	private void Start()
	{
		NetworkManager.Singleton.NetworkConfig.ConnectionApproval = true;
		if (RelayManager.Instance.IsHost)
		{
			NetworkManager.Singleton.ConnectionApprovalCallback = ConnectionApproval;
			(byte[] allocationId, byte[] key, byte[] connectionData, string ip, int port) = RelayManager.Instance.GetHostConnectionInfo();
			NetworkManager.Singleton.GetComponent<UnityTransport>().SetHostRelayData(ip, (ushort)port, allocationId, key, connectionData, true);
			NetworkManager.Singleton.StartHost();
		}
		else
		{
			NetworkManager.Singleton.ConnectionApprovalCallback = ConnectionApproval;
			(byte[] allocationId, byte[] key, byte[] connectionData, byte[] hostConnectionData, string ip, int port) = RelayManager.Instance.GetClientConnectionInfo();
			NetworkManager.Singleton.GetComponent<UnityTransport>().SetClientRelayData(ip, (ushort)port, allocationId, key, connectionData, hostConnectionData, true);
			NetworkManager.Singleton.StartClient();
		}
	}

	private void ConnectionApproval(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
	{
		Debug.Log($"Player connection: {request.ClientNetworkId}");
		response.Approved = true;
		response.CreatePlayerObject = true;
		response.Pending = false;
	}
}