using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleAudioButtons : MonoBehaviour
{
	private AudioSource audioSource;


	private void Awake()
	{
		audioSource = gameObject.GetComponent<AudioSource>();
	}

	public void PlaySound()
	{
		if (PlayerData.player.isSoundOn)
			audioSource.Play();
	}

	public void StopSound()
	{
		audioSource.Stop();
	}

	public bool IsPlaying()
	{
		return audioSource.isPlaying;
	}
}
