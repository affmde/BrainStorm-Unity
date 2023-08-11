using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsManagerScript : MonoBehaviour
{
	private GameObject audioManager;
	private HandleAudioButtons clickSound;
	SceneTransitionManager changeScene;
	private Animator audioManagerAnimator;
	AnimatorClipInfo[] currentClipInfo;
	private void Awake()
	{
		audioManager = GameObject.Find("AudioManager");
		audioManagerAnimator =audioManager.GetComponent<Animator>();
		clickSound = GameObject.Find("ClickButtonSound").GetComponent<HandleAudioButtons>();
		changeScene = GameObject.Find("Manager").GetComponent<SceneTransitionManager>();
	}

	private void Start()
	{
		currentClipInfo = audioManagerAnimator.GetCurrentAnimatorClipInfo(0);
		if (PlayerData.player.isSoundOn)
		{
			if (audioManager)
			{
				AudioManagerScript audio = audioManager.GetComponent<AudioManagerScript>();
				if (audio && !audio.IsPlaying())
				{
					audio.PlaySound();
					audioManagerAnimator.SetTrigger("FadeIn");
				}
				else if (currentClipInfo[0].clip.name == "Audio_out")
					audioManagerAnimator.SetTrigger("FadeIn");
			}
		}
	}

	public void Return()
	{
		changeScene.LoadNextScene("StartScene");
		clickSound.PlaySound();
	}
}
