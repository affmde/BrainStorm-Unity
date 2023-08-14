using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using GameFramework_Core.Data;
using Game.Events;

namespace Game
{
	public class LobbyUI : MonoBehaviour
	{
		[SerializeField] TextMeshProUGUI joinCode;
		[SerializeField] Button readyButton;
		[SerializeField] Button startButton;
		[SerializeField] Button leftButton;
		[SerializeField] Button rightButton;
		[SerializeField] TextMeshProUGUI choosenDifficulty;
		[SerializeField] DifficultySelectionData difficultySelectionData;

		private int currentDifficultyIndex = 0;

		private void OnEnable()
		{
			if (GameLobbyManager.Instance.IsHost)
			{
				leftButton.onClick.AddListener(OnLeftButtonClicked);
				rightButton.onClick.AddListener(OnRightButtonClicked);

				Events.LobbyEvents.OnLobbyReady += OnLobbyReady;
			}
			readyButton.onClick.AddListener(OnReadyPressed);
			LobbyEvents.OnLobbyUpdated += OnLobbyUpdated;
		}

		private void OnDisable()
		{
			//if (GameLobbyManager.Instance.IsHost)
			//{
				leftButton.onClick.RemoveListener(OnLeftButtonClicked);
				rightButton.onClick.RemoveListener(OnRightButtonClicked);
			//}
			readyButton.onClick.RemoveListener(OnReadyPressed);
			LobbyEvents.OnLobbyUpdated -= OnLobbyUpdated;
			Events.LobbyEvents.OnLobbyReady -= OnLobbyReady;
		}

		public void Start()
		{
			joinCode.text = "Code: " + GameLobbyManager.Instance.GetJoinCode();
			if (!GameLobbyManager.Instance.IsHost)
			{
				leftButton.gameObject.SetActive(false);
				rightButton.gameObject.SetActive(false);
			}
		}

		private async void OnReadyPressed()
		{
			bool succeed = await GameLobbyManager.Instance.SetPlayerReady();
			if (succeed)
				readyButton.gameObject.SetActive(false);
		}

		private async void OnLeftButtonClicked()
		{
			if (currentDifficultyIndex - 1 >= 0)
				currentDifficultyIndex--;
			else
				currentDifficultyIndex = difficultySelectionData.difficulties.Count - 1;
			UpdateDifficulty();
			await GameLobbyManager.Instance.SetSelectedDifficulty(currentDifficultyIndex);
		}

		private async void OnRightButtonClicked()
		{
			if (currentDifficultyIndex + 1 < difficultySelectionData.difficulties.Count)
				currentDifficultyIndex++;
			else
				currentDifficultyIndex = 0;
			UpdateDifficulty();
			await GameLobbyManager.Instance.SetSelectedDifficulty(currentDifficultyIndex);
		}

		private void UpdateDifficulty()
		{
			choosenDifficulty.text = difficultySelectionData.difficulties[currentDifficultyIndex].levelName;
		}

		private void OnLobbyUpdated()
		{
			currentDifficultyIndex = GameLobbyManager.Instance.GetDifficultyIndex();
			UpdateDifficulty();
		}

		private void OnLobbyReady()
		{
			startButton.gameObject.SetActive(true);
		}
		
	}
}
