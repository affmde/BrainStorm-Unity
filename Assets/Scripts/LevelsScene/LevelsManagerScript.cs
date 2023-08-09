using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsManagerScript : MonoBehaviour
{
	private GameObject audioManager;
	private HandleAudioButtons clickSound;
	SceneTransitionManager changeScene;
	private void Awake()
	{
		audioManager = GameObject.Find("AudioManager");
		clickSound = GameObject.Find("ClickButtonSound").GetComponent<HandleAudioButtons>();
		changeScene = GameObject.Find("Manager").GetComponent<SceneTransitionManager>();
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

	public void Return()
	{
		changeScene.LoadNextScene("StartScene");
		clickSound.PlaySound();
	}
}
