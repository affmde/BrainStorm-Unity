using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class XPBar : MonoBehaviour
{
	[SerializeField] private Image fillBar;
	[SerializeField] private TextMeshProUGUI levelText;
	bool isXpUpdated;

	public bool IsXPUpdated() { return isXpUpdated; }
	public void SetXPUpdated(bool val) { isXpUpdated = val; } 
	private void Start()
	{
		SetFillBar();
		SetStarLevelText();
	}

	void SetFillBar()
	{
		float	next = 2000 * Mathf.Pow(PlayerData.player.level, 1.5f);
		float	before = 2000 * Mathf.Pow(PlayerData.player.level - 1, 1.5f);
		float	currXP = (PlayerData.player.xp - before);
		float	targetXP = next - before;
		float	playerLevelProgress = currXP / targetXP;
		fillBar.fillAmount = playerLevelProgress;
	}

	void SetStarLevelText()
	{
		levelText.text = PlayerData.player.level.ToString();
	}


	private IEnumerator AddExperience(int amount)
	{
		float	currXP;
		float	targetXP;
		float	next;
		float	before;
		float	i = 0;
		float	current = PlayerData.player.xp;
		float	timeToWait = 2 / 0.02f;
		float	valToIncrease = amount / timeToWait;
		while (i < amount)
		{
			current += valToIncrease;
			next = 2000 * Mathf.Pow(PlayerData.player.level, 1.5f);
			before = 2000 * Mathf.Pow(PlayerData.player.level - 1, 1.5f);
			currXP = current - before;
			targetXP = next - before;
			float playerLevelProgress = currXP / targetXP;
			fillBar.fillAmount = playerLevelProgress;
			if (playerLevelProgress >= 1)
				PlayerData.player.level++;
			i += valToIncrease;
			yield return (timeToWait);
		}
		PlayerData.player.xp += amount;
		SetXPUpdated(true);
	}

	public void UpdateXP(int amount)
	{
		StartCoroutine(AddExperience(amount));
	}
}
