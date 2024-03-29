using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
	[SerializeField] List<Sprite> images;
	[SerializeField] List<Image> buttons;
	[SerializeField] Image timerBar;
	[SerializeField] private Image taskImage;
	private SceneTransitionManager sceneTransition;
	private AnimateTask animTask;
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
	private Image background;
	float timer;
	float timeLimit = 2.5f;
	public bool gameOver = false;
	private bool gameOn;
	private int random;
	private int	activeButton;
	private int totalCorrect;
	private int total;


	private void Awake()
	{
		animTask = taskImage.GetComponentInChildren<AnimateTask>();
		audioManager = GameObject.Find("AudioManager");
		sceneTransition = GameObject.Find("LevelLoader").GetComponent<SceneTransitionManager>();
		gameOverSound = GameObject.Find("GameOverSound");
		correctAnswerSound = GameObject.Find("CorrectAnswerSound");
		levelWonSound = GameObject.Find("WonLevelSound").GetComponent<HandleAudioButtons>();
		background = GameObject.Find("BackgroundPanel").GetComponent<Image>();
		fireworkSound = GameObject.Find("FireworksSound").GetComponent<HandleAudioButtons>();
		explosionSound = GameObject.Find("ExplosionSound").GetComponent<HandleAudioButtons>();
		gameSound = GameObject.Find("GameSound");
	}

	private void Start()
	{
		totalCorrect = 0;
		if (audioManager)
			audioManager.GetComponent<AudioManagerScript>().StopSound();
		timeLimit = GetTimeLimit();
		total = 10;
		gameOn = false;
		//UpdateGame();
		animTask.PlayAnimation();
		taskDescription.text = "";
		background.color = GenerateRandomColor();
	}

	public int GetActiveButton() { return activeButton; }
	public void SetActiveButton(int num)
	{
		activeButton = num; 
		for(int i = 0; i < buttons.Count; i++)
			buttons[i].GetComponent<ColourButton>().SetShowSelectImage();
	}
	public int GetTotal() { return total; }
	public int GetTotalCorrect() { return totalCorrect; }

	public void UpdateGame()
	{
		gameOn = true;
		if (PlayerData.difficultyLevel == 1)
			total = 15;
		else
			total = 10;
		activeButton = 0;
		timer = 0;
		random = Random.Range(0, LevelsData.levelsList.Count);
		while (!IsLevelAcceptableForDifficulty(LevelsData.levelsList[random].difficulty))
			random = Random.Range(0, LevelsData.levelsList.Count);
		buttons[0].sprite = GetSprite(LevelsData.levelsList[random].button1);
		buttons[1].sprite = GetSprite(LevelsData.levelsList[random].button2);
		buttons[2].sprite = GetSprite(LevelsData.levelsList[random].button3);
		buttons[3].sprite = GetSprite(LevelsData.levelsList[random].button4);
		foreach(Image btn in buttons)
			btn.GetComponent<ColourButton>().UnsetSelectImage();
		taskDescription.text = LevelsData.levelsList[random].task;
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
		if (gameOn)
		{
			timer += Time.deltaTime;
			timerBar.fillAmount = Mathf.Clamp01(timer / timeLimit);
		}
		if (timer >= timeLimit)
		{
			timer = 0;
			gameOn = false;
			if (LevelsData.levelsList[random].validOptions.Contains(activeButton) ||
			(LevelsData.levelsList[random].validOptions[0] == 0 && activeButton == 0))
			{
				taskDescription.text = "";
				correctAnswerSound.GetComponent<HandleAudioButtons>().PlaySound();
				totalCorrect++;
				if (totalCorrect < total)
					animTask.PlayAnimation();
				else
				{
					levelWonSound.PlaySound();
					fireworkSound.PlaySound();
				}
			}
			else
			{
				gameOverSound.GetComponent<HandleAudioButtons>().PlaySound();
				explosionSound.PlaySound();
				gameOver = true;
			}
		}
		if (totalCorrect >= total)
		{
			PlayerData.won = true;
			ParticleSystem ps = fireworksParticles.GetComponent<ParticleSystem>();
			ParticleSystem p_s = Instantiate(ps, gameObject.transform.position, gameObject.transform.rotation);
			p_s.transform.localScale = new Vector3(0.05f,0.05f,0.05f);
			if (!p_s.isPlaying)
				p_s.Play();
			SetTrigger(true);
			sceneTransition.LoadNextScene("EndGameScene");
		}
		else if (gameOver)
		{
			ParticleSystem ps = explosionParticles.GetComponent<ParticleSystem>();
			var screenBottomCenter = new Vector3(Screen.width/2, 0, 0);
			var inWorld = Camera.main.ScreenToWorldPoint(screenBottomCenter);
			ParticleSystem p_s = Instantiate(ps, inWorld, gameObject.transform.rotation);
			p_s.transform.localScale = new Vector3(5,5,5);
			if (!p_s.isPlaying)
				p_s.Play();
			PlayerData.won = false;
			SetTrigger(false);
			sceneTransition.LoadNextScene("EndGameScene");
		}
	}

	private bool	IsLevelAcceptableForDifficulty(int num)
	{
		if (PlayerData.difficultyLevel == 0)
		{
			if (num <= 2)
				return true;
			else
				return false;
		}
		else if (PlayerData.difficultyLevel == 1)
		{
			if (num >= 2 && num <= 5)
				return true;
			else
				return false;
		}
		else
		{
			if (num > 4)
				return true;
			else
				return false;
		}
	}

	private float GetTimeLimit()
	{
		if (PlayerData.difficultyLevel == 0)
		{
			if (PlayerData.player.currentGameLevel < 12)
				return 2.0f;
			else if (PlayerData.player.currentGameLevel < 24)
				return 1.8f;
			else if (PlayerData.player.currentGameLevel < 40)
				return 1.5f;
			else if (PlayerData.player.currentGameLevel < 50)
				return 1.4f;
			else if (PlayerData.player.currentGameLevel < 60)
				return 1.3f;
			else if (PlayerData.player.currentGameLevel < 70)
				return 1.2f;
			else
				return 1.1f;
		}
		else if (PlayerData.difficultyLevel == 1)
		{
			if (PlayerData.player.currentGameLevel < 12)
				return 2.0f;
			else if (PlayerData.player.currentGameLevel < 24)
				return 1.8f;
			else if (PlayerData.player.currentGameLevel < 40)
				return 1.7f;
			else if (PlayerData.player.currentGameLevel < 50)
				return 1.6f;
			else if (PlayerData.player.currentGameLevel < 60)
				return 1.5f;
			else if (PlayerData.player.currentGameLevel < 70)
				return 1.4f;
			else
				return 1.3f;
		}
		else
		{
			if (PlayerData.player.currentGameLevel < 12)
				return 1.8f;
			else if (PlayerData.player.currentGameLevel < 24)
				return 1.6f;
			else if (PlayerData.player.currentGameLevel < 40)
				return 1.5f;
			else if (PlayerData.player.currentGameLevel < 50)
				return 1.4f;
			else if (PlayerData.player.currentGameLevel < 60)
				return 1.3f;
			else if (PlayerData.player.currentGameLevel < 70)
				return 1.2f;
			else
				return 1.1f;
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
		if (won)
			fireworkSound.GetComponent<Animator>().SetTrigger("FadeOut");
		else
			explosionSound.GetComponent<Animator>().SetTrigger("FadeOut");
		gameSound.GetComponent<Animator>().SetTrigger("FadeOut");
	}
}
