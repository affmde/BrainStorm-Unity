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
	[SerializeField] TextMeshProUGUI taskDescription;
	float timer;
	float timeLimit = 2.0f;
	public bool gameOver = false;
	private int random;
	private int	activeButton;
	private int totalCorrect;
	private int total;

	private void Start()
	{
		totalCorrect = 0;
		total = 10; //Change this to load from Level file;
		UpdateGame();
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
		taskDescription.text = LevelsData.levelsList[random].task;
		Debug.Log("id: " + LevelsData.levelsList[random].id);
		Debug.Log("Button 1: " + LevelsData.levelsList[random].button1);
		Debug.Log("Button 2: " + LevelsData.levelsList[random].button2);
		Debug.Log("Button 3: " + LevelsData.levelsList[random].button3);
		Debug.Log("Button 4: " + LevelsData.levelsList[random].button4);
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
		timer += Time.deltaTime;
		timerBar.fillAmount = Mathf.Clamp01(timer / timeLimit);
		if (timer >= timeLimit)
		{
			if (LevelsData.levelsList[random].validOptions.Contains(activeButton))
			{
				totalCorrect++;
				UpdateGame();
			}
			else if (LevelsData.levelsList[random].validOptions[0] == 0)
			{
				totalCorrect++;
				UpdateGame();
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
	
}
