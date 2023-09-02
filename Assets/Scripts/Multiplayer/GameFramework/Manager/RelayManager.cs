using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Relay;
using Unity.Services.Relay.Http;
using Unity.Services.Relay.Models;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport;
using Unity.Networking.Transport.Relay;
using NetworkEvent = Unity.Networking.Transport.NetworkEvent;
using TMPro;
using UnityEngine.SceneManagement;

public class RelayManager : MonoBehaviour
{
	public static RelayManager instance;
	const int m_MaxConnections = 50;

	public string RelayJoinCode;
	[SerializeField] TextMeshProUGUI joinCodeText;
	[SerializeField] private TMP_InputField joinCodeInputField;

	public class RelayException : Exception
	{
		public RelayException() : base() {}
		public RelayException(string message) : base(message) {}
		public RelayException(string message, Exception e) : base(message, e) {}
		private string extraInfo;
		public string ExtraInfo
		{
			get => extraInfo;
			set => extraInfo = value;
		}
	}

	private void Awake()
	{
		if (instance == null)
			instance = this;
		else
			Destroy(gameObject);
	}

	private void Start()
	{
		MenuManager.onLoading?.Invoke(true);
		AuthenticatingAPlayer();
	}

	async void AuthenticatingAPlayer()
	{
		if (UnityServices.State == ServicesInitializationState.Initialized && 
			AuthenticationService.Instance.IsSignedIn &&
				AuthenticationService.Instance.IsAuthorized)
		{
			Debug.Log("Already Signed in and Authenticated");
			MenuManager.onLoading?.Invoke(false);
			return;
		}
		try
		{
			await UnityServices.InitializeAsync();
			#if UNITY_EDITOR
			if (ParrelSync.ClonesManager.IsClone())
			{
				Debug.Log("Changing profile");
				PlayerData.player.username = "Testing player";
				PlayerData.player.level = 3;
				// When using a ParrelSync clone, switch to a different authentication profile to force the clone
				// to sign in as a different anonymous user account.
				string customArgument = ParrelSync.ClonesManager.GetArgument();
				AuthenticationService.Instance.SwitchProfile($"Clone_{customArgument}_Profile");
			}
			#endif
			await AuthenticationService.Instance.SignInAnonymouslyAsync();
			var playerID = AuthenticationService.Instance.PlayerId;
			Debug.Log("Authenticated with id: " + playerID);
			Debug.Log($"Is SignedIn: {AuthenticationService.Instance.IsSignedIn}");
			Debug.Log($"Is Authorized: {AuthenticationService.Instance.IsAuthorized}");
			Debug.Log($"Is Expired: {AuthenticationService.Instance.IsExpired}");
			MenuManager.onLoading?.Invoke(false);
		}
		catch (Exception e)
		{
			Debug.Log(e);
			MenuManager.onClickSound?.Invoke();
			GameObject networkManager = GameObject.Find("NetworkManager");
			if (networkManager)
				Destroy(networkManager);
			SceneManager.LoadScene("StartScene");
		}
	}

	public async Task<RelayServerData> AllocateRelayServerAndGetJoinCode(int maxConnections, string region = null)
	{
		Allocation allocation;
		string createJoinCode;
		try
		{
			allocation = await RelayService.Instance.CreateAllocationAsync(maxConnections, region);
		}
		catch (Exception e)
		{
			Debug.LogError($"Relay create allocation request failed {e.Message}");
			throw new RelayException(e.ToString());
		}

		Debug.Log($"server: {allocation.ConnectionData[0]} {allocation.ConnectionData[1]}");
		Debug.Log($"server: {allocation.AllocationId}");

		try
		{
			createJoinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
			joinCodeText.text = "Share code with your friends\n" + createJoinCode;
		}
		catch
		{
			Debug.LogError("Relay create join code request failed");
			throw new RelayException("Relay create join code request failed");
		}

		return new RelayServerData(allocation, "dtls");
	}

	public IEnumerator ConfigureTransportAndStartNgoAsHost()
	{
		var serverRelayUtilityTask = AllocateRelayServerAndGetJoinCode(m_MaxConnections);
		while (!serverRelayUtilityTask.IsCompleted)
		{
			yield return null;
		}
		if (serverRelayUtilityTask.IsFaulted)
		{
			Debug.LogError("Exception thrown when attempting to start Relay Server. Server not started. Exception: " + serverRelayUtilityTask.Exception.Message);
			MenuManager.onLoading?.Invoke(false);
			//yield break;
			throw new RelayException("Exception thrown when attempting to start Relay Server. Server not started. Exception: " + serverRelayUtilityTask.Exception.Message);
		}

		var relayServerData = serverRelayUtilityTask.Result;

		// Display the joinCode to the user.

		NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
		NetworkManager.Singleton.StartHost();
		MenuManager.onLoading?.Invoke(false);
		MenuManager.instance.GameState = MenuManager.State.Waiting;
		MenuManager.onGameStateChanged?.Invoke(MenuManager.instance.GameState);
		yield return null;
	}

	public async Task<RelayServerData> JoinRelayServerFromJoinCode(string joinCode)
	{
		JoinAllocation allocation;
		try
		{
			allocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
		}
		catch
		{
			Debug.LogError("Relay create join code request failed");
			MenuManager.onLoading?.Invoke(false);
			throw;
		}

		Debug.Log($"client: {allocation.ConnectionData[0]} {allocation.ConnectionData[1]}");
		Debug.Log($"host: {allocation.HostConnectionData[0]} {allocation.HostConnectionData[1]}");
		Debug.Log($"client: {allocation.AllocationId}");

		return new RelayServerData(allocation, "dtls");
	}

	public IEnumerator ConfigureTransportAndStartNgoAsConnectingPlayer()
	{
		// Populate RelayJoinCode beforehand through the UI
		var clientRelayUtilityTask = JoinRelayServerFromJoinCode(joinCodeInputField.text);

		while (!clientRelayUtilityTask.IsCompleted)
		{
			yield return null;
		}

		if (clientRelayUtilityTask.IsFaulted)
		{
			Debug.LogError("Exception thrown when attempting to connect to Relay Server. Exception: " + clientRelayUtilityTask.Exception.Message);
			MenuManager.onLoading?.Invoke(false);
			MenuManager.onRefuseClientConnectionMessage?.Invoke(3.5f, clientRelayUtilityTask.Exception.Message);
			yield break;
		}

		var relayServerData = clientRelayUtilityTask.Result;

		NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

		NetworkManager.Singleton.StartClient();
		MenuManager.onLoading?.Invoke(false);
		MenuManager.onGameStateChanged(MenuManager.State.Waiting);
		yield return null;
	}
}
