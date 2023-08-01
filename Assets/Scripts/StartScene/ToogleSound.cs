using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToogleSound : MonoBehaviour
{
	[SerializeField] private List<Sprite> soundSprites;
	[SerializeField] private Image obj;
	private void Start()
	{
		UpdateSoundSprite();
	}

	public void SoundToggle()
	{
		PlayerData.player.isSoundOn = !PlayerData.player.isSoundOn;
		UpdateSoundSprite();
	}

	private void UpdateSoundSprite()
	{
		obj.sprite = PlayerData.player.isSoundOn ? soundSprites[1] : soundSprites[0];
	}
}
