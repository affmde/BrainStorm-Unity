using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;
using TMPro;

public class MenuManager : NetworkBehaviour
{
	public static MenuManager instance;

	public enum State {Menu, Game, Win, Lose, Waiting }
	private State gameState;

	[Header("Events")]
	public static Action<State> onGameStateChanged;
	public static Action onStartSetPlayerData;
	public static Action onGameUpdate;
	public static Action<int> onAnalizeAnswers;
	public static Action<int, int> onUpdatePlayerScore;
	public static Action onResetTask;
	public static Action onResetGame;
	public static Action<int> onUpdateTotalPlayersConnected;
	public static Action onClickSound;
	public static Action onCancelSound;
	public static Action<float, string> onRefuseClientConnectionMessage;
	public static Action<int> onUpdateLobbyAfterDisconnect;

	[SerializeField] private static int playersToPlay;
	[SerializeField] private Button startGameButton;
	[SerializeField] private Button cancelGameButton;
	[SerializeField] private Button settingsButton;
	[SerializeField] private TextMeshProUGUI totalConnectedPlayersText;
	[SerializeField] private MultiplayerGameManager mgm;
	[SerializeField] GameObject startHostErrorPanel;

	public static int PlayersToPlay
	{
		get => playersToPlay;
		set => playersToPlay = value;
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
		startGameButton.gameObject.SetActive(false);
		gameState = State.Menu;
		startHostErrorPanel.SetActive(false);
		cancelGameButton.gameObject.SetActive(false);
		settingsButton.gameObject.SetActive(false);
		startGameButton.gameObject.SetActive(false);

	}

	private void OnEnable()
	{
		startGameButton.onClick.AddListener(StartGame);
	}

	private void OnDisable()
	{
		startGameButton.onClick.RemoveListener(StartGame);
	}

	public override void OnNetworkSpawn()
	{
		base.OnNetworkSpawn();

		NetworkManager.OnServerStarted += NetworkManager_OnServerStarted;
		NetworkManager.OnServerStopped += NetworkManager_OnServerStopped;
	}


	private void NetworkManager_OnServerStarted()
	{
		if (!IsServer) return;
		NetworkManager.Singleton.OnClientConnectedCallback += Singleton_OnClientConnectedCallback;
		NetworkManager.Singleton.OnClientDisconnectCallback += Singleton_OnClientDisconnectedCallback;
		onUpdateTotalPlayersConnected += UpdateTotalPlayersConnectedCallback;
		NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
	}

	private void NetworkManager_OnServerStopped(bool unused)
	{
		NetworkManager.Singleton.OnClientConnectedCallback -= Singleton_OnClientConnectedCallback;
		NetworkManager.Singleton.OnClientDisconnectCallback -= Singleton_OnClientDisconnectedCallback;
		onUpdateTotalPlayersConnected -= UpdateTotalPlayersConnectedCallback;
		NetworkManager.Singleton.ConnectionApprovalCallback -= ApprovalCheck;
	}

	public override void OnDestroy()
	{
		if (NetworkManager)
		{
			NetworkManager.OnServerStarted -= NetworkManager_OnServerStarted;
			NetworkManager.OnServerStopped -= NetworkManager_OnServerStopped;
		}
		base.OnDestroy();
	}
	private void Singleton_OnClientConnectedCallback(ulong obj)
	{
		Debug.Log("Player connected: " + obj);
		Debug.Log("Players on connected List: " + NetworkManager.Singleton.ConnectedClientsList.Count);
		totalConnectedPlayersText.text = "" + NetworkManager.Singleton.ConnectedClientsList.Count;
		onUpdateTotalPlayersConnected?.Invoke(NetworkManager.Singleton.ConnectedClientsList.Count);
		cancelGameButton.gameObject.SetActive(true);
		settingsButton.gameObject.SetActive(true);
		if (NetworkManager.Singleton.ConnectedClientsList.Count >= playersToPlay && IsServer)
			ToggleStartButtonToServerShow(true);
		PlayerAnswer pl = NetworkManager.Singleton.ConnectedClients[obj].PlayerObject.GetComponent<PlayerAnswer>();
		pl.Id = (int)obj;
		SetButtonsPositionForClientsClientRpc();
	}

	[ClientRpc]
	private void SetButtonsPositionForClientsClientRpc()
	{
		if (IsServer) return;
		startGameButton.gameObject.SetActive(false);
		settingsButton.gameObject.SetActive(false);
		cancelGameButton.gameObject.SetActive(true);
		RectTransform buttonRectTransform = cancelGameButton.GetComponent<RectTransform>();
		

		Vector2 currentAnchorMin = buttonRectTransform.anchorMin;
		Vector2 currentAnchorMax = buttonRectTransform.anchorMax;
		
		var anchorPoint = new Vector2(0.5f, 0);
		buttonRectTransform.anchorMax = anchorPoint;
		buttonRectTransform.anchorMin = anchorPoint;
		
		Vector2 newPosition = new Vector2(buttonRectTransform.anchoredPosition.x, 
			buttonRectTransform.anchoredPosition.y + currentAnchorMin.y - 0f * buttonRectTransform.sizeDelta.y);
		newPosition.x = 0f;
		buttonRectTransform.anchoredPosition = newPosition;
	}

	private void Singleton_OnClientDisconnectedCallback(ulong obj)
	{
		Debug.Log("Client " + obj + " has disconnected");
		Debug.Log("Now there are " + NetworkManager.Singleton.ConnectedClientsList.Count + " in the ConnectedClientsList");
		onUpdateTotalPlayersConnected?.Invoke(NetworkManager.Singleton.ConnectedClientsList.Count - 1);
		onUpdateLobbyAfterDisconnect?.Invoke((int)obj);
		if (NetworkManager.Singleton.ConnectedClientsList.Count - 1 < 2)
			ToggleStartButtonToServerShow(false);
		if (!IsServer && NetworkManager.Singleton.DisconnectReason != string.Empty)
		{
			Debug.Log($"Approval Declined Reason: {NetworkManager.Singleton.DisconnectReason}");
			MenuManager.onRefuseClientConnectionMessage?.Invoke(1.5f, NetworkManager.Singleton.DisconnectReason);
		}
	}

	private void StartGame()
	{
		StartGameClientRpc();
	}

	[ClientRpc]
	private void StartGameClientRpc()
	{
		gameState = State.Game;
		onGameStateChanged?.Invoke(gameState);
	}

	public void OnHostButtonClicked()
	{
		if (NetworkManager.Singleton.StartHost())
		{
			gameState = State.Waiting;
			onGameStateChanged?.Invoke(gameState);
		}
		else
		{
			ErrorMessageHandler error = startHostErrorPanel.GetComponent<ErrorMessageHandler>();
			error.Message = "Cant create game now.";
			StartCoroutine(error.ShowMessage(2f));
		}
	}

	public void OnCancelGameConnection()
	{
		SaveData.Save();
		if (IsServer)
		{
			NetworkManager.Singleton.Shutdown();
			CancelGameClientRpc();
		}
		else
		{
			NetworkManager.Singleton.Shutdown();
			SceneManager.LoadScene("MultiplayerMenu");
		}
	}

	[ClientRpc]
	private void CancelGameClientRpc()
	{
		SceneManager.LoadScene("MultiplayerMenu");
	}

	private void UpdateTotalPlayersConnectedCallback(int currentAmountOfPlayersConnected)
	{
		UpdateTotalPlayersConnectedClientRpc(currentAmountOfPlayersConnected);
	}

	[ClientRpc]
	private void UpdateTotalPlayersConnectedClientRpc(int currentAmountOfPlayersConnected)
	{
		totalConnectedPlayersText.text = "" + currentAmountOfPlayersConnected;
	}

	private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
	{
		Debug.Log("ApprovalCheck callback called");
		// The client identifier to be authenticated
		var clientId = request.ClientNetworkId;
		Debug.Log("client trying to connect: " + clientId);
		// Additional connection data defined by user code
		var connectionData = request.Payload;

		if (mgm.GameOver == false)
		{
			response.Approved = false;
			response.Reason = "Game is already going on. Please wait that it finishes";
			response.Pending = false;
			return;
		}
		// Your approval logic determines the following values
		response.Approved = true;
		response.CreatePlayerObject = true;

		// The Prefab hash value of the NetworkPrefab, if null the default NetworkManager player Prefab is used
		response.PlayerPrefabHash = null;

		// Position to spawn the player object (if null it uses default of Vector3.zero)
		response.Position = Vector3.zero;

		// Rotation to spawn the player object (if null it uses the default of Quaternion.identity)
		response.Rotation = Quaternion.identity;
		
		// If response.Approved is false, you can provide a message that explains the reason why via ConnectionApprovalResponse.Reason
		// On the client-side, NetworkManager.DisconnectReason will be populated with this message via DisconnectReasonMessage
		response.Reason = "Some reason for not approving the client";

		// If additional approval steps are needed, set this to true until the additional steps are complete
		// once it transitions from true to false the connection approval response will be processed.
		response.Pending = false;
	}

	private void ToggleStartButtonToServerShow(bool val)
	{
		startGameButton.gameObject.SetActive(val);
	}
}
