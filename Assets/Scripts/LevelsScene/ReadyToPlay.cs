using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReadyToPlay : MonoBehaviour
{
	private GameObject playSound;
	private GameObject cancelSound;

	private void Awake()
	{
		playSound = GameObject.Find("PlayButtonSound");
		cancelSound = GameObject.Find("CloseMenuButtonSound");
	}
	private void Start()
	{
		gameObject.SetActive(false);
	}

	public void Play()
	{
		playSound.GetComponent<HandleAudioButtons>().PlaySound();
		SceneManager.LoadScene("GameScene");
	}

	public void ClosePanel()
	{
		cancelSound.GetComponent<HandleAudioButtons>().PlaySound();
		gameObject.SetActive(false);
	}
}
