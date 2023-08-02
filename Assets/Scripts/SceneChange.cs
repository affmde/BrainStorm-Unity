using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
	private GameObject clickSound;

	private void Awake()
	{
		clickSound = GameObject.Find("ClickButtonSound");
	}
	public void ChangeScene()
	{
		clickSound.GetComponent<HandleAudioButtons>().PlaySound();
		SceneManager.LoadScene("StartScene");
	}
}
