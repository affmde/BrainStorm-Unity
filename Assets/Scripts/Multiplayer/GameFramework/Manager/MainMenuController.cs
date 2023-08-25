using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Core;
using Unity.Services.Authentication;
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
		[SerializeField] private TextMeshProUGUI codeText;
		[SerializeField] private Button cancelServerCreationButton;
		[SerializeField] GameObject waitingPanel;
		[SerializeField] GameObject joinPanel;
		[SerializeField] GameObject gamePanel;
		[SerializeField] GameObject endGamePanel;
		[SerializeField] int playersToPlay;

		private void Start()
		{
			ShowMenu();
			MenuManager.PlayersToPlay = playersToPlay;
			MenuManager.onGameStateChanged += GameStateChangedCallback;
			MultiplayerActions.onContinueFromEndGame += QuitGameServer;
		}
		private void OnEnable()
		{
			hostButton.onClick.AddListener(OnHostButtonClicked);
			joinButton.onClick.AddListener(OnJoinButtonClicked);
			submitCodeButton.onClick.AddListener(OnSubmitCodeClicked);
			returnButton.onClick.AddListener(OnReturnButtonClicked);
			cancelServerCreationButton.onClick.AddListener(CancelGameCreation);
		}


		private void OnDisable()
		{
			hostButton.onClick.RemoveListener(OnHostButtonClicked);
			joinButton.onClick.RemoveListener(OnJoinButtonClicked);
			submitCodeButton.onClick.RemoveListener(OnSubmitCodeClicked);
			returnButton.onClick.RemoveListener(OnReturnButtonClicked);
			cancelServerCreationButton.onClick.RemoveListener(CancelGameCreation);
		}
		private void OnDestroy()
		{
			MenuManager.onGameStateChanged -= GameStateChangedCallback;
			MultiplayerActions.onContinueFromEndGame -= QuitGameServer;
		}

		private void OnReturnButtonClicked()
		{
			GameObject networkManager = GameObject.Find("NetworkManager");
			if (networkManager)
				Destroy(networkManager);
			if (UnityServices.State == ServicesInitializationState.Initialized)
				AuthenticationService.Instance.SignOut();
			SceneManager.LoadScene("StartScene");
		}

		public void OnHostButtonClicked()
		{
			MenuManager.instance.OnHostButtonClicked();
		}

		private void OnJoinButtonClicked()
		{
			ShowJoinPanel();
		}

		private async void OnSubmitCodeClicked()
		{
			string ipAdress = IpManager.instance.GetInputIp();
			ipAdress = ipAdress.Substring(0, ipAdress.Length - 1);
			UnityTransport utp = NetworkManager.Singleton.GetComponent<UnityTransport>();
			utp.SetConnectionData(ipAdress, 7777);
			NetworkManager.Singleton.StartClient();
			MenuManager.onGameStateChanged(MenuManager.State.Waiting);
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

		public void ShowEndGamePanel(bool winner)
		{
			menuScreen.SetActive(false);
			waitingPanel.SetActive(false);
			joinPanel.SetActive(false);
			gamePanel.SetActive(false);
			endGamePanel.SetActive(true);
			EndGameUIManagerActions.onEndSceneEnter?.Invoke(winner);
		}

		public void QuitGameServer()
		{
			MenuManager.onResetGame?.Invoke();
			MultiplayerActions.onMultiplayerGameReset?.Invoke();
			NetworkManager.Singleton.Shutdown();
			//ShowMenu();
			SceneManager.LoadScene("MultiplayerMenu");
		}

		private void CancelGameCreation()
		{
			MenuManager.instance.OnCancelGameConnection();
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
