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
	private XPBar xpBar;
	private bool xpIsUpdated;
	float timer = 0;

	private void Awake()
	{
		xpBar = xpPanel.GetComponent<XPBar>();
	}
	private void Start()
	{
		if (PlayerData.won)
		{
			gameOverText.gameObject.SetActive(false);
			congratsText.gameObject.SetActive(true);
			PlayerData.player.completedLevelsList[PlayerData.difficultyLevel].completedLevels.Add(PlayerData.difficultyLevel);
			//Continue from here tomorrow!!!!
		}
		else
		{
			gameOverText.gameObject.SetActive(true);
			congratsText.gameObject.SetActive(false);
		}
		continueButton.gameObject.SetActive(false);
		xpBar.UpdateXP(300);
	}

	private void Update()
	{
		timer += Time.deltaTime;
		if (timer >= 1.0f && continueButton.gameObject.activeInHierarchy == false)
			continueButton.gameObject.SetActive(true);
		if (xpBar.IsXPUpdated() && !xpIsUpdated)
		{
			continueButton.gameObject.SetActive(true);
			xpIsUpdated = true;
		}
	}
}
