using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ColourButton : MonoBehaviour
{
	private GameObject				manager;
	private GameManager				gm;
	[SerializeField] private int	buttonId;
	[SerializeField] private Image	selectImage;
	private GameObject				clickSound;

	private void Awake()
	{
		manager = GameObject.FindGameObjectWithTag("Manager");
		if (manager)
		{
			Debug.Log("Manager found");
			gm = manager.GetComponent<GameManager>();
		}
		clickSound = GameObject.Find("ClickButtonSound");
	}

	public void UnsetSelectImage() { selectImage.gameObject.SetActive(false); }

	private void Start()
	{
		selectImage.gameObject.SetActive(false);
	}

	public void	HandleClick()
	{
		if (gm)
		{
			clickSound.GetComponent<HandleAudioButtons>().PlaySound();
			if (gm.GetActiveButton() == buttonId)
			{
				gm.SetActiveButton(0);
				selectImage.gameObject.SetActive(false);
			}
			else
			{
				gm.SetActiveButton(buttonId);
				selectImage.gameObject.SetActive(true);
			}
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
