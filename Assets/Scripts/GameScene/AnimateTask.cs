using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateTask : MonoBehaviour
{
	private bool isActive;
	private float time;
	private float rotation;
	private GameSoundManagerScript transitionSound;
	GameManager gm;
	private bool transitionSoundOn;
	[SerializeField] Animator taskAnimator;

	private void Awake()
	{
		gm = GameObject.Find("Manager").GetComponent<GameManager>();
		transitionSound = GameObject.Find("GameSceneAudioManager").GetComponent<GameSoundManagerScript>();
	}

	public bool IsTransitionSoundOn() { return transitionSoundOn; }

	public void SetTransitionSoundOn(bool val) { transitionSoundOn = val; }

	public bool GetIsActive() { return isActive; }
	public void SetIsActive(bool val) { isActive = val; }
	public void PlaySound() { transitionSound.PlayTransitionSound(); }
	public void ResetAnimationState()
	{
		taskAnimator.SetTrigger("ResetState");
		gm.UpdateGame();
	}
}
