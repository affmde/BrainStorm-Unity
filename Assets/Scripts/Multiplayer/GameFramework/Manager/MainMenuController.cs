using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine.UI;
using TMPro;

namespace Game
{
	public class MainMenuController : MonoBehaviour
	{
		[SerializeField] private GameObject menuScreen;
		[SerializeField] private Button hostButton;
		[SerializeField] private Button joinButton;
		[SerializeField] private Button submitCodeButton;
		[SerializeField] private Button returnButton;
		[SerializeField] private Button settingsButton;
		[SerializeField] private Button cancelServerCreationButton;
		[SerializeField] GameObject waitingPanel;
		[SerializeField] GameObject joinPanel;
		[SerializeField] GameObject gamePanel;
		[SerializeField] GameObject endGamePanel;
		[SerializeField] int playersToPlay;
		[SerializeField] GameObject messagePanel;
		[Header("Sounds")]
		HandleAudioButtons clickSound;
		HandleAudioButtons cancelSound;


		private void Awake()
		{
			clickSound = GameObject.Find("ClickButtonSound").GetComponent<HandleAudioButtons>();
			cancelSound = GameObject.Find("CloseMenuButtonSound").GetComponent<HandleAudioButtons>();
		}

		private void Start()
		{
			ShowMenu();
			messagePanel.SetActive(false);
			MenuManager.PlayersToPlay = playersToPlay;
			MenuManager.onGameStateChanged += GameStateChangedCallback;
			MultiplayerActions.onContinueFromEndGame += QuitGameServer;
			MenuManager.onClickSound += clickSound.PlaySound;
			MenuManager.onCancelSound += cancelSound.PlaySound;
			MenuManager.onRefuseClientConnectionMessage += RefuseClientConnectionMessage;
		}
		private void OnEnable()
		{
			hostButton.onClick.AddListener(OnHostButtonClicked);
			joinButton.onClick.AddListener(OnJoinButtonClicked);
			submitCodeButton.onClick.AddListener(OnSubmitCodeClicked);
			returnButton.onClick.AddListener(OnReturnButtonClicked);
			cancelServerCreationButton.onClick.AddListener(CancelGameCreation);
			settingsButton.onClick.AddListener(SettingsButtonClicked);
		}


		private void OnDisable()
		{
			hostButton.onClick.RemoveListener(OnHostButtonClicked);
			joinButton.onClick.RemoveListener(OnJoinButtonClicked);
			submitCodeButton.onClick.RemoveListener(OnSubmitCodeClicked);
			returnButton.onClick.RemoveListener(OnReturnButtonClicked);
			cancelServerCreationButton.onClick.RemoveListener(CancelGameCreation);
			settingsButton.onClick.RemoveListener(SettingsButtonClicked);
		}
		private void OnDestroy()
		{
			MenuManager.onGameStateChanged -= GameStateChangedCallback;
			MultiplayerActions.onContinueFromEndGame -= QuitGameServer;
			MenuManager.onClickSound -= clickSound.PlaySound;
			MenuManager.onCancelSound -= cancelSound.PlaySound;
			MenuManager.onRefuseClientConnectionMessage -= RefuseClientConnectionMessage;
		}

		private void OnReturnButtonClicked()
		{
			MenuManager.onClickSound?.Invoke();
			GameObject networkManager = GameObject.Find("NetworkManager");
			if (networkManager)
				Destroy(networkManager);
			SceneManager.LoadScene("StartScene");
		}

		public void OnHostButtonClicked()
		{
			MenuManager.onClickSound?.Invoke();
			MenuManager.instance.OnHostButtonClicked();
		}

		private void OnJoinButtonClicked()
		{
			MenuManager.onClickSound?.Invoke();
			ShowJoinPanel();
		}

		private void OnSubmitCodeClicked()
		{
			MenuManager.onClickSound?.Invoke();
			string ipAdress = IpManager.instance.GetInputIp();
			ipAdress = ipAdress.Substring(0, ipAdress.Length - 1);
			UnityTransport utp = NetworkManager.Singleton.GetComponent<UnityTransport>();
			utp.SetConnectionData(ipAdress, 7777);
			if (NetworkManager.Singleton.StartClient())
				MenuManager.onGameStateChanged(MenuManager.State.Waiting);
			else
				MenuManager.onRefuseClientConnectionMessage?.Invoke(1.5f, "Couldn't connect to server.");
			
		}

		public void ShowMenu()
		{
			menuScreen.SetActive(true);
			waitingPanel.SetActive(false);
			joinPanel.SetActive(false);
			gamePanel.SetActive(false);
			endGamePanel.SetActive(false);
		}

		public void ShowWaiting()
		{
			DiffcultyOptionsManager.SetShowSettingsPanel?.Invoke(false);
			menuScreen.SetActive(false);
			waitingPanel.SetActive(true);
			joinPanel.SetActive(false);
			gamePanel.SetActive(false);
			endGamePanel.SetActive(false);
		}

		public void ShowJoinPanel()
		{
			menuScreen.SetActive(false);
			waitingPanel.SetActive(false);
			joinPanel.SetActive(true);
			gamePanel.SetActive(false);
			endGamePanel.SetActive(false);
		}

		public void ShowGame()
		{
			menuScreen.SetActive(false);
			waitingPanel.SetActive(false);
			joinPanel.SetActive(false);
			gamePanel.SetActive(true);
			endGamePanel.SetActive(false);
			MultiplayerGameManager mgm = GameObject.Find("GamePanelManager").GetComponent<MultiplayerGameManager>();
			mgm.StartGame();
		}

		public void ShowEndGamePanel()
		{
			menuScreen.SetActive(false);
			waitingPanel.SetActive(false);
			joinPanel.SetActive(false);
			gamePanel.SetActive(false);
			endGamePanel.SetActive(true);
		}

		public void QuitGameServer()
		{
			MenuManager.onClickSound?.Invoke();
			MenuManager.onResetGame?.Invoke();
			MultiplayerActions.onMultiplayerGameReset?.Invoke();
			NetworkManager.Singleton.Shutdown();
			SceneManager.LoadScene("MultiplayerMenu");
		}

		private void CancelGameCreation()
		{
			MenuManager.onCancelSound?.Invoke();
			MenuManager.instance.OnCancelGameConnection();
		}

		private void SettingsButtonClicked()
		{
			DiffcultyOptionsManager.SetShowSettingsPanel?.Invoke(true);
		}

		private void RefuseClientConnectionMessage(float duration, string message)
		{
			ErrorMessageHandler error = messagePanel.GetComponent<ErrorMessageHandler>();
			error.Message = message;
			StartCoroutine(error.ShowMessage(duration));
		}

		private void GameStateChangedCallback(MenuManager.State gameState)
		{
			switch(gameState)
			{
				case MenuManager.State.Game:
					ShowGame();
					break;
				case MenuManager.State.Menu:
					ShowMenu();
					break;
				case MenuManager.State.Waiting:
					ShowWaiting();
					break;
			}
		}
	}

}
