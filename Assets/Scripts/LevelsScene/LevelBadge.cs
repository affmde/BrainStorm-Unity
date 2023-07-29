using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelBadge : MonoBehaviour
{
	[SerializeField] private Image	completedImage;
	[SerializeField] private Image	lockImage;
	[SerializeField] private TextMeshProUGUI levelText;

	private void	Start()
	{
		int badgeLevel = int.Parse(gameObject.name);
		levelText.text = gameObject.name;
		if (PlayerData.currentGameLevel == badgeLevel)
		{
			lockImage.gameObject.SetActive(false);
			completedImage.gameObject.SetActive(false);
		}
		else
		{
			if (isLevelCompleted(badgeLevel))
			{
				lockImage.gameObject.SetActive(false);
				completedImage.gameObject.SetActive(true);
				Debug.Log("Level: " + badgeLevel + " - TRUE");
			}
			else
			{
				lockImage.gameObject.SetActive(true);
				completedImage.gameObject.SetActive(false);
				Debug.Log("Level: " + badgeLevel + " - FALSE");
			}
		}
	}

	private bool isLevelCompleted(int level)
	{
		for (int i = 0; i < PlayerData.levelsCompleted.Length; i++)
			if (PlayerData.levelsCompleted[i] == level)
				return true;
		return false;
	}
}
