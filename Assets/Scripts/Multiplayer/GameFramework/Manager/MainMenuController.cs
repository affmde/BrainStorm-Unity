using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

namespace Game
{
	public class MainMenuController : MonoBehaviour
	{
		[SerializeField] private GameObject menuScreen;
		[SerializeField] private GameObject joinScreen;
		[SerializeField] private Button hostButton;
		[SerializeField] private Button joinButton;
		[SerializeField] private Button submitCodeButton;
		[SerializeField] private TextMeshProUGUI codeText;

		private void OnEnable()
		{
			joinScreen.SetActive(false);
			hostButton.onClick.AddListener(OnHostButtonClicked);
			joinButton.onClick.AddListener(OnJoinButtonClicked);
			submitCodeButton.onClick.AddListener(OnSubmitCodeClicked);
		}

		private void OnDisable()
		{
			hostButton.onClick.RemoveListener(OnHostButtonClicked);
			joinButton.onClick.RemoveListener(OnJoinButtonClicked);
			submitCodeButton.onClick.RemoveListener(OnSubmitCodeClicked);
		}


		private async void OnHostButtonClicked()
		{
			bool succeeded = await GameLobbyManager.Instance.CreateLobby();
			Debug.Log("Host button clicked");
			if (succeeded)
				SceneManager.LoadScene("Lobby");
		}

		private void OnJoinButtonClicked()
		{
			menuScreen.SetActive(false);
			joinScreen.SetActive(true);
			Debug.Log("Join button clicked");
		}

		private async void OnSubmitCodeClicked()
		{
			string code = codeText.text;
			code = code.Substring(0, code.Length - 1);
			bool succeeded = await GameLobbyManager.Instance.JoinLobby(code);
			if (succeeded)
				SceneManager.LoadScene("Lobby");
		}
	}

}
