using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyButton : MonoBehaviour
{
	[SerielizeField] private int level;
	[SerielizeField] private GameObject panel;

	public int SetDifficultyLevel() { PlayerData.difficultyLevel = level; }

	public void OpenPanel()
	{
		panel.SetActive(true);
		SetDifficultyLevel();
	}

	public void ClosePanel()
	{
		panel.SetActive(false);
	}
}
