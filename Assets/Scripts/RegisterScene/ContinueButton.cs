using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ContinueButton : MonoBehaviour
{
	public void	Continue()
	{
		if (PlayerData.username.Length > 0)
		{
			PlayerPrefs.SetString("username", PlayerData.username);
			SceneManager.LoadScene("StartScene");
		}
	}
}
