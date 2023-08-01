using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerScript : MonoBehaviour
{
	private static AudioManagerScript instance;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
			Destroy(gameObject);
	}

	public void PlaySound()
	{
		if (PlayerData.player.isSoundOn)
			gameObject.GetComponent<AudioSource>().Play();
	}

	public void StopSound()
	{
		gameObject.GetComponent<AudioSource>().Stop();
	}

	public void PauseSound()
	{
		gameObject.GetComponent<AudioSource>().Pause();
	}

	public bool IsPlaying()
	{
		return gameObject.GetComponent<AudioSource>().isPlaying;
	}

}
