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
		Debug.Log("CurrentCLipInfo name: " + currentClipInfo[0].clip.name);
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
				{
					Debug.Log("HERE! CORRECT ANIMATOR STATE NAME");
					audioManagerAnimator.SetTrigger("FadeIn");
				}
				else
				{
					Debug.Log("Current name: " + currentClipInfo[0].clip.name);
				}
			}
		}
	}

	public void Return()
	{
		changeScene.LoadNextScene("StartScene");
		clickSound.PlaySound();
	}
}
