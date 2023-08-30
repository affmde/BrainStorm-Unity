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
	[SerializeField] private MultiplayerTaskAnimator animTask;
	[SerializeField] TextMeshProUGUI taskDescription;
	private GameObject audioManager;
	[SerializeField] private GameObject gameSound;

	[SerializeField] private Image background;
	[SerializeField] ScoreManager scoreManager;
	[SerializeField] MainMenuController menuController;
	[SerializeField] float timer;
	[SerializeField] float timeLimit;
	public bool gameOver = false;
	private bool gameOn;
	private int index;
	private int	activeButton;
	[SerializeField] private int total;

	public List<Sprite> GetImages() { return images; }
	public List<Image> GetButtons() { return buttons; }
	public float GetTimer() { return timer; }
	public TextMeshProUGUI GetTaskDescription() { return taskDescription; }

	public void OnEnable()
	{
		MultiplayerActions.onMultiplayerGameReset += ResetGameStatsCallback;
		if (IsServer)
			MultiplayerActions.onUpdateMultiplayerGameData += UpdateMultiplayerGameData;
	}

	public void OnDisable()
	{
		MultiplayerActions.onMultiplayerGameReset -= ResetGameStatsCallback;
		if (IsServer)
			MultiplayerActions.onUpdateMultiplayerGameData -= UpdateMultiplayerGameData;
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

	public bool GameOver
	{
		get => gameOver;
		set => gameOver = value;
	}
	private void Awake()
	{
		audioManager = GameObject.Find("AudioManager");
	}

	private void Start()
	{
		gameSound.GetComponent<HandleAudioButtons>().StopSound();
		if (!audioManager.GetComponent<HandleAudioButtons>().IsPlaying())
			audioManager.GetComponent<HandleAudioButtons>().PlaySound();
		total = DiffcultyOptionsManager.instance.MaxPointsToWin;
		timeLimit = DiffcultyOptionsManager.instance.MaxTimeToAnswer;
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
		total = DiffcultyOptionsManager.instance.MaxPointsToWin;
		timeLimit = DiffcultyOptionsManager.instance.MaxTimeToAnswer;
		MultiplayerActions.onUpdateMultiplayerGameData?.Invoke();
		MenuManager.onStartSetPlayerData?.Invoke();
		animTask.PlayAnimation();
	}
	[ClientRpc]
	private void ResetGameDataClientRpc()
	{
		gameSound.GetComponent<HandleAudioButtons>().PlaySound();
		audioManager.GetComponent<HandleAudioButtons>().StopSound();
		if (audioManager)
			audioManager.GetComponent<AudioManagerScript>().StopSound();
		MenuManager.onResetGame?.Invoke();
	}

	public int Total
	{
		get => total;
		set => total = value;
	}

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
		UpdateClientRpc(Time.deltaTime);
		CheckWinner();
	}
	[ClientRpc]
	private void UpdateClientRpc(float deltaTime)
	{
		if (gameOver) return;
		if (gameOn)
		{
			timer += deltaTime;
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

	[ClientRpc]
	private void HandleEndGameClientRpc()
	{
		gameSound.GetComponent<Animator>().SetTrigger("FadeOut");
		gameOver = true;
		NetworkObject no = NetworkManager.LocalClient.PlayerObject;
		PlayerAnswer pl = no.GetComponent<PlayerAnswer>();
		menuController.ShowEndGamePanel();
		//EndGameUIManagerActions.onEndSceneEnter?.Invoke();
	}

	private void CheckWinner()
	{
		if (!IsServer) return;
		foreach(var client in NetworkManager.Singleton.ConnectedClientsList)
		{
			PlayerAnswer pl = client.PlayerObject.GetComponent<PlayerAnswer>();
			if (pl.TotalCorrectAnswers >= total)
			{
				pl.Winner = true;
				gameOver = true;
				HandleEndGameClientRpc();
				Debug.Log("Going to call now EndGameUIManagerActions.onEndSceneEnter");
				EndGameUIManagerActions.onEndSceneEnter?.Invoke();
			}
		}
	}


	private void ResetGameStatsCallback()
	{
		timer = 0;
	}

	private void UpdateMultiplayerGameData()
	{
		UpdateMultiplayerGameDataClientRpc();
	}

	[ClientRpc]
	private void UpdateMultiplayerGameDataClientRpc()
	{
		DiffcultyOptionsManager.instance.MaxPointsToWin = DiffcultyOptionsManager.instance.MaxPointsToWin;
		DiffcultyOptionsManager.instance.MaxLimitTime = DiffcultyOptionsManager.instance.MaxLimitTime;
		total = DiffcultyOptionsManager.instance.MaxPointsToWin;
		timeLimit = DiffcultyOptionsManager.instance.MaxTimeToAnswer;
	}
}

public static class MultiplayerActions
{
	public static System.Action onMultiplayerGameReset;
	public static System.Action onContinueFromEndGame;
	public static System.Action onIncreasePlayerScore;
	public static System.Action onUpdateMultiplayerGameData;
}
