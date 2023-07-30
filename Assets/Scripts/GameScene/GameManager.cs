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
	[SerializeField] TextMeshProUGUI taskDescription;
	float timer;
	float timeLimit = 2.0f;
	public bool gameOver;
	private int random;
	private void Start()
	{
		UpdateGame();
	}

	public void UpdateGame()
	{
		timer = 0;
		random = Random.Range(0, LevelsData.levelsList.Count);
		buttons[0].sprite = GetSprite(LevelsData.levelsList[random].button1);
		buttons[1].sprite = GetSprite(LevelsData.levelsList[random].button2);
		buttons[2].sprite = GetSprite(LevelsData.levelsList[random].button3);
		buttons[3].sprite = GetSprite(LevelsData.levelsList[random].button4);
		taskDescription.text = LevelsData.levelsList[random].task;
		timeLimit = LevelsData.levelsList[random].time;
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
		if (timer >= 1 && LevelsData.levelsList[random].validOptions[0] != 0)
			gameOver = true;
		else if (timer >= 1 && LevelsData.levelsList[random].validOptions[0] == 0)
			UpdateGame();
	}

	public int GetRandom()
	{
		return random;
	}
}
