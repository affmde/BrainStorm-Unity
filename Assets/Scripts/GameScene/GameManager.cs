using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	[SerializeField] List<Sprite> images;
	[SerializeField] List<Image> buttons;
	[SerializeField] Image timerBar;
	[SerializeField] private Image taskImage;
	private AnimateTask animTask;
	[SerializeField] TextMeshProUGUI taskDescription;
	float timer;
	float timeLimit = 2.0f;
	public bool gameOver = false;
	private int random;
	private int	activeButton;
	private int totalCorrect;
	private int total;


	private void Awake()
	{
		animTask = taskImage.GetComponentInChildren<AnimateTask>();
	}

	private void Start()
	{
		totalCorrect = 0;
		total = 10;
		animTask.SetIsActive(true);
		StartCoroutine(TaskTransition());
		//UpdateGame();
	}

	public int GetActiveButton() { return activeButton; }
	public void SetActiveButton(int num) { activeButton = num; }
	public int GetRandom() { return random; }
	public int GetTotal() { return total; }
	public void SetTotal(int val) { total = val; }
	public int GetTotalCorrect() { return totalCorrect; }
	public void increaseTotalCorrect() { totalCorrect++; }

	public void UpdateGame()
	{
		total = StaticLevels.config[PlayerData.player.currentGameLevel - 1].total;
		activeButton = 0;
		timer = 0;
		random = Random.Range(0, LevelsData.levelsList.Count);
		while (IsLevelAcceptableForDifficulty(random))
			random = Random.Range(0, LevelsData.levelsList.Count);
		buttons[0].sprite = GetSprite(LevelsData.levelsList[random].button1);
		buttons[1].sprite = GetSprite(LevelsData.levelsList[random].button2);
		buttons[2].sprite = GetSprite(LevelsData.levelsList[random].button3);
		buttons[3].sprite = GetSprite(LevelsData.levelsList[random].button4);
		foreach(Image btn in buttons)
			btn.GetComponent<ColourButton>().UnsetSelectImage();
		taskDescription.text = LevelsData.levelsList[random].task;
		//timeLimit = StaticLevels.config[random].duration;
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
		if (!animTask.GetIsActive())
		{
			timer += Time.deltaTime;
			timerBar.fillAmount = Mathf.Clamp01(timer / timeLimit);
		}
		if (timer >= timeLimit)
		{
			if (LevelsData.levelsList[random].validOptions.Contains(activeButton))
			{
				totalCorrect++;
				animTask.SetIsActive(true);
				StartCoroutine(TaskTransition());
				//UpdateGame();
			}
			else if (LevelsData.levelsList[random].validOptions[0] == 0 && activeButton == 0)
			{
				totalCorrect++;
				animTask.SetIsActive(true);
				StartCoroutine(TaskTransition());
				//UpdateGame();
			}
			else
				gameOver = true;
		}
		if (totalCorrect >= total)
		{
			PlayerData.won = true;
			SceneManager.LoadScene("EndGameScene");
		}
		else if (gameOver)
		{
			PlayerData.won = false;
			SceneManager.LoadScene("EndGameScene");
		}
	}

	private bool	IsLevelAcceptableForDifficulty(int num)
	{
		if (PlayerData.difficultyLevel == 0)
		{
			if (num <= 1)
				return true;
			else
				return false;
		}
		else if (PlayerData.difficultyLevel == 1)
		{
			if (num >= 1 && num <= 3)
				return true;
			else
				return false;
		}
		else
		{
			if (num >= 4)
				return true;
			else
				return false;
		}
	}

	IEnumerator TaskTransition()
	{
		timer = 0;
		taskDescription.text = "";
		while (animTask.GetIsActive())
			yield return null;
		UpdateGame();
	}
}
