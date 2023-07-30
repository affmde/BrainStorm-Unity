using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReadyToPlay : MonoBehaviour
{
	private void Start()
	{
		gameObject.SetActive(false);
	}

	public void Play()
	{
		SceneManager.LoadScene("GameScene");
	}

	public void ClosePanel()
	{
		gameObject.SetActive(false);
	}
}
