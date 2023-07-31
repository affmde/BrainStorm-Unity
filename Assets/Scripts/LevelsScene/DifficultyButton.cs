using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyButton : MonoBehaviour
{
	[SerializeField] private int level;
	[SerializeField] private GameObject panel;

	public void SetDifficultyLevel() { PlayerData.difficultyLevel = level; }

	public void OpenPanel()
	{
		panel.SetActive(true);
		SetDifficultyLevel();
		Debug.Log("LEvel choosed: " + PlayerData.difficultyLevel);
	}
}
