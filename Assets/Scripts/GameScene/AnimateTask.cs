using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateTask : MonoBehaviour
{
	private bool isActive;
	private float time;
	private float rotation;
	private GameSoundManagerScript transitionSound;
	private bool transitionSoundOn;

	private void Awake()
	{
		transitionSound = GameObject.Find("GameSceneAudioManager").GetComponent<GameSoundManagerScript>();
	}

	public bool IsTransitionSoundOn() { return transitionSoundOn; }
	public void SetTransitionSoundOn(bool val) { transitionSoundOn = val; }

	public bool GetIsActive() { return isActive; }
	public void SetIsActive(bool val) { isActive = val; }

	private void Update()
	{
		if (isActive)
		{
			if (!transitionSoundOn)
			{
				transitionSound.PlayTransitionSound();
				transitionSoundOn = true;
			}
			time += Time.deltaTime;
			Debug.Log("rotation time: " + time);
			gameObject.transform.Rotate(Vector3.up, 360f * Time.deltaTime / 1f);
			if (time > 1)
			{
				isActive = false;
				time = 0;
				gameObject.transform.Rotate(new Vector3(0, 0, 0));
			}
		}
	}
}
