using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourButton : MonoBehaviour
{
	private GameObject				manager;
	private GameManager				gm;
	[SerializeField] private int	buttonId;

	private void Awake()
	{
		manager = GameObject.FindGameObjectWithTag("Manager");
		if (manager)
		{
			Debug.Log("Manager found");
			gm = manager.GetComponent<GameManager>();
		}
	}

	public void	HandleClick()
	{
		Debug.Log("total correct: " + gm.GetTotalCorrect() + " - Total: " + gm.GetTotal());
		if (gm)
		{
			if (gm.GetActiveButton() == buttonId)
				gm.SetActiveButton(0);
			else
				gm.SetActiveButton(buttonId);
		}
	}

	private bool IsButtonValid()
	{
		for(int i = 0; i < LevelsData.levelsList[gm.GetRandom()].validOptions.Count; i++)
		{
			if (LevelsData.levelsList[gm.GetRandom()].validOptions[i] == buttonId)
			{
				return true;
			}
		}
		return false;
	}
}
