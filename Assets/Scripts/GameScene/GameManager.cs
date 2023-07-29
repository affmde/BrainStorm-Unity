using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	[SerializeField] List<Sprite> images;
	[SerializeField] List<Image> buttons;
	[SerializeField] Image timerBar;
	float timer;
	float timeLimit = 1.5f;
	private List<int> list;
	public bool gameOver;
	private void Start()
	{
		list = new List<int>();
		PopulateButtons();
	}

	public void PopulateButtons()
	{
		int random = Random.Range(0, LevelsData.levelsList.Count - 1);
		buttons[0].sprite = GetSprite(LevelsData.levelsList[random].button1);
		buttons[1].sprite = GetSprite(LevelsData.levelsList[random].button2);
		buttons[2].sprite = GetSprite(LevelsData.levelsList[random].button3);
		buttons[3].sprite = GetSprite(LevelsData.levelsList[random].button4);
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
		if (timer >= 1)
			gameOver = true;
	}
}
