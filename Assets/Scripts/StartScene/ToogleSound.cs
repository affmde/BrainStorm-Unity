using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToogleSound : MonoBehaviour
{
	[SerializeField] private List<Sprite> soundSprites;
	[SerializeField] private Image obj;
	[SerializeField] GameObject audioManager;

	private void Awake()
	{
		audioManager = GameObject.Find("AudioManager");
	}
	private void Start()
	{
		UpdateSoundSprite();
	}

	public void SoundToggle()
	{
		PlayerData.player.isSoundOn = !PlayerData.player.isSoundOn;
		if (PlayerData.player.isSoundOn)
			audioManager.GetComponent<AudioManagerScript>().PlaySound();
		else
			audioManager.GetComponent<AudioManagerScript>().StopSound();
		UpdateSoundSprite();
	}

	private void UpdateSoundSprite()
	{
		obj.sprite = PlayerData.player.isSoundOn ? soundSprites[1] : soundSprites[0];
	}
}
