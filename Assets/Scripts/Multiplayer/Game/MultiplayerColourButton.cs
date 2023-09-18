using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode;

public class MultiplayerColourButton : NetworkBehaviour
{
	[SerializeField] private MultiplayerGameManager mgm;
	[SerializeField] private int		buttonId;
	[SerializeField] private Image		selectImage;
	private GameObject					clickSound;
	private PlayerAnswer[]				playersAnswer;

	[SerializeField] private TextMeshProUGUI activeButtonText;

	private void Awake()
	{
		playersAnswer = FindObjectsOfType<PlayerAnswer>();
		clickSound = GameObject.Find("ClickButtonSound");
	}

	public void UnsetSelectImage() { selectImage.gameObject.SetActive(false); }
	public void SetSelectImage() { selectImage.gameObject.SetActive(true); }

	public void SetShowSelectImage(int activeButton)
	{
		List<Image> buttons = mgm.GetButtons();
		foreach (Image button in buttons)
		{
			MultiplayerColourButton mcb = button.GetComponent<MultiplayerColourButton>();
			if (mcb.ButtonId == activeButton)
				mcb.SetSelectImage();
			else
				mcb.UnsetSelectImage();
		}
	}
	public int ButtonId
	{
		get => buttonId;
	}

	private void Start()
	{
		selectImage.gameObject.SetActive(false);
	}

	public void	HandleClick()
	{
		if (!mgm.GameOn) return;
		playersAnswer = FindObjectsOfType<PlayerAnswer>();
		foreach(var pl in playersAnswer)
			pl.ButtonClickedHandler(this, mgm.Timer);
		clickSound.GetComponent<HandleAudioButtons>().PlaySound();
	}
}
