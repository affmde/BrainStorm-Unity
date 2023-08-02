using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EndGameManager : MonoBehaviour
{
	[SerializeField] TextMeshProUGUI gameOverText;
	[SerializeField] TextMeshProUGUI congratsText;
	[SerializeField] Image continueButton;
	[SerializeField] GameObject xpPanel;
	private GameObject xpSound;
	private GameObject gameOverSound;
	private GameObject gameWonSound;
	private XPBar xpBar;
	private bool xpIsUpdated;
	float timer = 0;
	int xp;

	private void Awake()
	{
		xpBar = xpPanel.GetComponent<XPBar>();
		xpSound = GameObject.Find("XpButtonSound");
		gameOverSound = GameObject.Find("GameOverSound");
		gameWonSound = GameObject.Find("GameWonSound");
	}
	private void Start()
	{
		xpSound.GetComponent<HandleAudioButtons>().PlaySound();
		if (PlayerData.won)
		{
			gameWonSound.GetComponent<HandleAudioButtons>().PlaySound();
			gameOverText.gameObject.SetActive(false);
			congratsText.gameObject.SetActive(true);
			PlayerData.player.completedLevelsList[PlayerData.difficultyLevel].completedLevels.Add(PlayerData.player.completedLevelsList[PlayerData.difficultyLevel].currentLevel);
			PlayerData.player.completedLevelsList[PlayerData.difficultyLevel].currentLevel++;
			int xp = 100 * (PlayerData.difficultyLevel + 1) + (PlayerData.difficultyLevel + 1) * 20;
			PlayerData.player.xp += xp;
		}
		else
		{
			gameOverSound.GetComponent<HandleAudioButtons>().PlaySound();
			gameOverText.gameObject.SetActive(true);
			congratsText.gameObject.SetActive(false);
			int xp = 15 * (PlayerData.difficultyLevel + 1);
			PlayerData.player.xp += xp;
		}
		continueButton.gameObject.SetActive(false);
		xpBar.UpdateXP(xp);
	}

	private void Update()
	{
		timer += Time.deltaTime;
		if (timer >= 1.0f && continueButton.gameObject.activeInHierarchy == false)
			continueButton.gameObject.SetActive(true);
		if (xpBar.IsXPUpdated() && !xpIsUpdated)
		{
			xpSound.GetComponent<HandleAudioButtons>().StopSound();
			continueButton.gameObject.SetActive(true);
			xpIsUpdated = true;
		}
	}
}
