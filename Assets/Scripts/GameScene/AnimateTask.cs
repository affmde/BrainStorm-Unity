using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateTask : MonoBehaviour
{
	GameManager gm;

	Animator taskAnimator;
	ParticleSystem ps;

	private void Awake()
	{
		gm = GameObject.Find("Manager").GetComponent<GameManager>();
		taskAnimator = gameObject.GetComponent<Animator>();
		ps = gameObject.GetComponentInChildren<ParticleSystem>();
	}
	public void PlayAnimation()
	{
		taskAnimator.GetComponent<HandleAudioButtons>().PlaySound();
		ps.Play();
		taskAnimator.SetTrigger("Start");
	}
	public void ResetAnimationState()
	{
		taskAnimator.SetTrigger("ResetState");
		gm.UpdateGame();
	}
}
