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

	[SerializeField] private static int playersToPlay;
	[SerializeField] private Button startGameButton;
	[SerializeField] private Button cancelGameButton;
	[SerializeField] private Button settingsButton;
	[SerializeField] private TextMeshProUGUI totalConnectedPlayersText;
	

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
	}

	private void NetworkManager_OnServerStopped(bool unused)
	{
		NetworkManager.Singleton.OnClientConnectedCallback -= Singleton_OnClientConnectedCallback;
		NetworkManager.Singleton.OnClientDisconnectCallback -= Singleton_OnClientDisconnectedCallback;
		onUpdateTotalPlayersConnected -= UpdateTotalPlayersConnectedCallback;
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
		if (NetworkManager.Singleton.ConnectedClientsList.Count >= playersToPlay && IsServer)
		{
			startGameButton.gameObject.SetActive(true);
			settingsButton.gameObject.SetActive(true);
		}
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
		gameState = State.Waiting;
		onGameStateChanged?.Invoke(gameState);
		NetworkManager.Singleton.StartHost();
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
}
