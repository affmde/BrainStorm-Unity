using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsManagerScript : MonoBehaviour
{
	private GameObject audioManager;

	private void Awake()
	{
		audioManager = GameObject.Find("AudioManager");
	}

	private void Start()
	{
		if (PlayerData.player.isSoundOn)
		{
			if (audioManager)
			{
				AudioManagerScript audio = audioManager.GetComponent<AudioManagerScript>();
				if (audio && !audio.IsPlaying())
					audio.PlaySound();
			}
		}
	}
}
