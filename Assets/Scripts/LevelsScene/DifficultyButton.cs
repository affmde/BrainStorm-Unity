using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DifficultyButton : MonoBehaviour
{
	[SerializeField] private int level;
	[SerializeField] private GameObject panel;
	[SerializeField] private TextMeshProUGUI text;
	private Image[] allImages;
	private List<Image> badges;
	private void Awake()
	{
		allImages = FindObjectsOfType<Image>(true);
		badges = new List<Image>();
		foreach(Image img in allImages)
		{
			if (img.CompareTag("Badges"))
				badges.Add(img);
		}
		Debug.Log("badges found: " + badges.Count);
	}

	public void SetDifficultyLevel() { PlayerData.difficultyLevel = level; }

	public void OpenPanel()
	{
		panel.SetActive(true);
		SetDifficultyLevel();
		Debug.Log("Level choosed: " + PlayerData.difficultyLevel);
		foreach(Image img in badges)
		{
			img.GetComponent<LevelBadge>().UpdateBadgeInfo(level);
		}
		text.text = SetText();
	}

	private string SetText()
	{
		if (level == 0)
			return "Easy";
		else if (level == 1)
			return "Medium";
		else
			return "Hard";
	}
}
