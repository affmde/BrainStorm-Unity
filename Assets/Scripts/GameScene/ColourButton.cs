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
			gm = manager.GetComponent<GameManager>();
		clickSound = GameObject.Find("ClickButtonSound");
	}

	public void UnsetSelectImage() { selectImage.gameObject.SetActive(false); }

	private void Start()
	{
		selectImage.gameObject.SetActive(false);
	}

	public void SetShowSelectImage()
	{
		if (gm.GetActiveButton() == buttonId)
			selectImage.gameObject.SetActive(true);
		else
			selectImage.gameObject.SetActive(false);
	}

	public void	HandleClick()
	{
		if (gm)
		{

			clickSound.GetComponent<HandleAudioButtons>().PlaySound();
			if (gm.GetActiveButton() == buttonId)
				gm.SetActiveButton(0);
			else
				gm.SetActiveButton(buttonId);
		}
	}
}
