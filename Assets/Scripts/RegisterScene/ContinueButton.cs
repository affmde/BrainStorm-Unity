using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueButton : MonoBehaviour
{
	private SceneTransitionManager sceneTransition;

	private void Awake()
	{
		sceneTransition = GameObject.Find("LevelLoader").GetComponent<SceneTransitionManager>();
	}

	public void	Continue()
	{
		if (PlayerData.player.username.Length > 0)
		{
			PlayerPrefs.SetString("username", PlayerData.player.username);
			sceneTransition.LoadNextScene("StartScene");
		}
	}
}
