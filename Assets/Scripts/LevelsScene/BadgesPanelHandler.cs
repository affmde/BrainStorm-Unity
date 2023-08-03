using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BadgesPanelHandler : MonoBehaviour
{
	private GameObject closeSound;

	private void Awake()
	{
		closeSound = GameObject.Find("CloseMenuButtonSound");
	}
	public void Close()
	{
		if (closeSound)
			closeSound.GetComponent<HandleAudioButtons>().PlaySound();
		gameObject.SetActive(false);
	}
}
