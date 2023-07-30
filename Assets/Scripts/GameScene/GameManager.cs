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

	private void Start()
	{
		UpdateGame();
	}

	public int GetActiveButton() { return activeButton; }
	public void SetActiveButton(int num) { activeButton = num; }
	public int GetRandom() { return random; }

	public void UpdateGame()
	{
		timer = 0;
		random = Random.Range(0, LevelsData.levelsList.Count);
		buttons[0].sprite = GetSprite(LevelsData.levelsList[random].button1);
		buttons[1].sprite = GetSprite(LevelsData.levelsList[random].button2);
		buttons[2].sprite = GetSprite(LevelsData.levelsList[random].button3);
		buttons[3].sprite = GetSprite(LevelsData.levelsList[random].button4);
		taskDescription.text = LevelsData.levelsList[random].task;
		//timeLimit = LevelsData.levelsList[random].time;
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
				UpdateGame();
			else
				gameOver = true;
		}
		if (gameOver)
			SceneManager.LoadScene("EndGameScene");
	}
}
