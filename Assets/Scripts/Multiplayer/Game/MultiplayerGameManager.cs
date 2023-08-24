using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using Game;

public class MultiplayerGameManager : NetworkBehaviour
{
	[SerializeField] List<Sprite> images;
	[SerializeField] List<Image> buttons;
	[SerializeField] Image timerBar;
	[SerializeField] private Image taskImage;
	[SerializeField] private MultiplayerTaskAnimator animTask;
	[SerializeField] TextMeshProUGUI taskDescription;
	private GameObject audioManager;
	private GameObject gameOverSound;
	private GameObject correctAnswerSound;
	private GameObject gameSound;
	private HandleAudioButtons levelWonSound;
	private HandleAudioButtons fireworkSound;
	private HandleAudioButtons explosionSound;
	[SerializeField] private GameObject fireworksParticles;
	[SerializeField] private GameObject explosionParticles;
	[SerializeField] private Image background;
	[SerializeField] ScoreManager scoreManager;
	[SerializeField] MainMenuController menuController;
	float timer;
	float timeLimit = 2.5f;
	public bool gameOver = false;
	private bool gameOn;
	private int index;
	private int	activeButton;
	private int totalCorrect;
	private int total;
	[SerializeField] private int pointsToWin;
	[SerializeField] GameAnswersManager gameAnswersManager;

	public List<Sprite> GetImages() { return images; }
	public List<Image> GetButtons() { return buttons; }
	public float GetTimer() { return timer; }
	public TextMeshProUGUI GetTaskDescription() { return taskDescription; }

	public void OnEnable()
	{
		MultiplayerActions.onMultiplayerGameReset += ResetGameStatsCallback;
	}

	public void OnDisable()
	{
		MultiplayerActions.onMultiplayerGameReset -= ResetGameStatsCallback;
	}
	public bool GameOn
	{
		get => gameOn;
		set => gameOn = value;
	}

	public int Index
	{
		get => index;
		set => index = value;
	}

	public int ActiveButton
	{
		get => activeButton;
		set => activeButton = value;
	}

	public float Timer
	{
		get => timer;
		set => timer = value;
	}

	private void Start()
	{
		totalCorrect = 0;
		if (audioManager)
			audioManager.GetComponent<AudioManagerScript>().StopSound();
		timeLimit = 1.8f;
		total = 10;
		scoreManager.MaxPoints = total;
		gameOn = false;
		taskDescription.text = "";
		background.color = GenerateRandomColor();
	}

	public void StartGame()
	{
		gameOver = false;
		background.color = GenerateRandomColor();
		ResetGameDataClientRpc();
		MenuManager.onStartSetPlayerData?.Invoke();
		animTask.PlayAnimation();
	}
	[ClientRpc]
	private void ResetGameDataClientRpc()
	{
		MenuManager.onResetGame?.Invoke();
	}

	public int Total
	{
		get => total;
		set => total = value;
	}
	public int GetTotalCorrect() { return totalCorrect; }

	private Sprite GetSprite(string color)
	{
		if (color == "Blue")
			return images[0];
		else if (color == "Green")
			return images[1];
		else if (color == "Red")
			return images[2];
		else if (color == "White")
			return images[3];
		else
			return images[4];
	}

	private void Update()
	{
		if (!IsServer) return;
		if (gameOver) return;

		UpdateClientRpc();
		CheckWinner();
	}
	[ClientRpc]
	private void UpdateClientRpc()
	{
		if (gameOver) return;
		if (gameOn)
		{
			timer += Time.deltaTime;
			timerBar.fillAmount = Mathf.Clamp01(timer / timeLimit);
		}
		if (timer > timeLimit)
		{
			timer = 0;
			activeButton = 0;
			gameOn = false;
			if (IsServer)
			{
				MenuManager.onAnalizeAnswers?.Invoke(index);
				MenuManager.onResetTask?.Invoke();
			}
			animTask.PlayAnimation();
		}
	}

	private Color GenerateRandomColor()
	{
		float red = Random.Range(0f, 1f);
		float blue = Random.Range(0f, 1f);
		float green = Random.Range(0f, 1f);
		return new Color(red, blue, green, 1);
	}

	private void SetTrigger(bool won)
	{
		/*if (won)
			fireworkSound.GetComponent<Animator>().SetTrigger("FadeOut");
		else
			explosionSound.GetComponent<Animator>().SetTrigger("FadeOut");*/
		//gameSound.GetComponent<Animator>().SetTrigger("FadeOut");
	}

	[ClientRpc]
	private void HandleEndGameClientRpc()
	{
		NetworkObject no = NetworkManager.LocalClient.PlayerObject;
		PlayerAnswer pl = no.GetComponent<PlayerAnswer>();
		menuController.ShowEndGamePanel(pl.Winner);
	}

	private void CheckWinner()
	{
		if (!IsServer) return;

		if (scoreManager.Player1Score >= pointsToWin || scoreManager.Player2Score >= pointsToWin)
		{
			bool hostWinner = false;
			if (scoreManager.Player1Score >= pointsToWin)
			{
				hostWinner = true;
				Debug.Log("Player 1 won!");
			}
			else
			{
				hostWinner = false;
				Debug.Log("Player 2 won!");
			}
			gameOver = true;
			UpdateWinningInfoClientRpc(hostWinner);
			HandleEndGameClientRpc();
		}
	}

	[ClientRpc]
	private void UpdateWinningInfoClientRpc(bool hostWinner)
	{
		NetworkObject no = NetworkManager.LocalClient.PlayerObject;
		PlayerAnswer pl = no.GetComponent<PlayerAnswer>();
		if (IsHost)
		{
			if (hostWinner)
				pl.Winner = true;
			else
				pl.Winner = false;
		}
		else
		{
			if (hostWinner)
				pl.Winner = false;
			else
				pl.Winner = true;
		}
		gameOver = true;
	}

	private void ResetGameStatsCallback()
	{
		timer = 0;
	}


}

public static class MultiplayerActions
{
	public static System.Action onMultiplayerGameReset;
	public static System.Action onContinueFromEndGame;
}
