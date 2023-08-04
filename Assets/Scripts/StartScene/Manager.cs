using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
public class Manager : MonoBehaviour
{
	private GameObject audioManager;
	private GameObject clickSound;
	
	private void Awake()
	{
		audioManager = GameObject.Find("AudioManager");
		clickSound = GameObject.Find("ClickButtonSound");
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
		SceneManager.LoadScene("LevelsScene");
	}

	
	void LoadPlayerData()
	{
		LoadData.Load(PlayerData.player);
	}
}