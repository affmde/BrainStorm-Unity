using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelBadge : MonoBehaviour
{
	[SerializeField] private Image				completedImage;
	[SerializeField] private Image				lockImage;
	[SerializeField] private TextMeshProUGUI	levelText;
	[SerializeField ]private GameObject			readyPanel;

	private void	Start()
	{
		readyPanel.SetActive(false);
		UpdateBadgeInfo(0);
	}

	private bool isLevelCompleted(int level)
	{
		for (int i = 0; i < PlayerData.player.completedLevelsList[PlayerData.difficultyLevel].completedLevels.Count; i++)
			if (PlayerData.player.completedLevelsList[PlayerData.difficultyLevel].completedLevels[i] == level)
				return true;
		return false;
	}

	public void OpenReadyPanel()
	{
		readyPanel.SetActive(true);
	}

	public void UpdateBadgeInfo(int choosedDifficulty)
	{
		int badgeLevel = int.Parse(gameObject.name);
		levelText.text = gameObject.name;
		if (PlayerData.player.completedLevelsList[choosedDifficulty].currentLevel == badgeLevel)
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
			}
			else
			{
				lockImage.gameObject.SetActive(true);
				completedImage.gameObject.SetActive(false);
			}
		}
	}
}
