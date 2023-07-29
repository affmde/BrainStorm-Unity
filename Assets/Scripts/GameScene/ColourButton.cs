using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourButton : MonoBehaviour
{
	public bool			isValid = true;
	private GameObject	manager;
	private GameManager gm;

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
		if (gm)
		{
			if (isValid)
				gm.PopulateButtons();
			else
				gm.gameOver = true;
		}
	}
}
