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
	float timeLimit = 1.0f;
	private List<int> list;
	public bool gameOver;
	private void Start()
	{
		list = new List<int>();
		PopulateButtons();
	}

	public void PopulateButtons()
	{
		Debug.Log("Populate called");
		for (int i = 0; i < 4; i++)
		{
			int random = Random.Range(0, images.Count - 1);
			while(isRepeated(random))
				random = Random.Range(0, images.Count - 1);
			list.Add(random);
		}
		for (int i = 0; i < buttons.Count; i++)
			buttons[i].sprite = images[list[i]];
	}

	bool isRepeated(int num)
	{
		for(int i = 0; i < list.Count; i++)
			if (list[i] == num)
				return true;
		return false;
	}

	private void Update()
	{
		timer += Time.deltaTime;
		timerBar.fillAmount = Mathf.Clamp01(timer / timeLimit);
		if (timer >= 1)
			gameOver = true;
	}
}
