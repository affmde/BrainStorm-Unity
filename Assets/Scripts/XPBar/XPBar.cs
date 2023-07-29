using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class XPBar : MonoBehaviour
{
	[SerializeField] private Image fillBar;
	[SerializeField] private TextMeshProUGUI levelText;

	private void Start()
	{
		SetFillBar();
		SetStarLevelText();
	}

	void SetFillBar()
	{
		float xpLimit = 2000.0f * (PlayerData.level + 1);
		float xpToShow = xpLimit - 2000 + PlayerData.xp;
		fillBar.fillAmount = Mathf.Clamp01(xpToShow / 2000);
	}

	void SetStarLevelText()
	{
		levelText.text = PlayerData.level.ToString();
	}

}
