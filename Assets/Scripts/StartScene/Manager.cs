using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
	private GameObject audioManager;
	private GameObject clickSound;
	private SceneTransitionManager sceneTransition;
	private void Awake()
	{
		audioManager = GameObject.Find("AudioManager");
		clickSound = GameObject.Find("ClickButtonSound");
		sceneTransition = GameObject.Find("LevelLoader").GetComponent<SceneTransitionManager>();
		LoadPlayerData();
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

	public void Play()
	{
		clickSound.GetComponent<HandleAudioButtons>().PlaySound();
		sceneTransition.LoadNextScene("LevelsScene");
	}

	public void PlayMultiplayer()
	{
		clickSound.GetComponent<HandleAudioButtons>().PlaySound();
		sceneTransition.LoadNextScene("MultiplayerMenu");
	}
	void LoadPlayerData()
	{
		LoadData.Load(PlayerData.player);
	}
}