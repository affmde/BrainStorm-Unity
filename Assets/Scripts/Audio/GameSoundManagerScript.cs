using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSoundManagerScript : MonoBehaviour
{
	private GameObject gameSound;
	private GameObject transitionSound;

	private void	Awake()
	{
		gameSound = GameObject.Find("GameSound");
		transitionSound = GameObject.Find("TransitionSound");
	}

	private void Start()
	{
		gameSound.GetComponent<HandleAudioButtons>().PlaySound();
	}

	public void PlayTransitionSound()
	{
		transitionSound.GetComponent<HandleAudioButtons>().PlaySound();
	}
}
