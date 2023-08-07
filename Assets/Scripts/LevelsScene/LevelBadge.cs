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
	[SerializeField]private GameObject			readyPanel;
	private GameObject clickSound;
	int badgeLevel;

	private void Awake()
	{
		clickSound = GameObject.Find("ClickButtonSound");
	}
	private void	Start()
	{
		badgeLevel = int.Parse(gameObject.name);
		readyPanel.SetActive(false);
		UpdateBadgeInfo(PlayerData.difficultyLevel);
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
		if (PlayerData.player.completedLevelsList[PlayerData.difficultyLevel].currentLevel == badgeLevel)
		{
			PlayerData.player.currentGameLevel = badgeLevel;
			clickSound.GetComponent<HandleAudioButtons>().PlaySound();
			readyPanel.SetActive(true);
		}
	}

	public void UpdateBadgeInfo(int choosedDifficulty)
	{
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
