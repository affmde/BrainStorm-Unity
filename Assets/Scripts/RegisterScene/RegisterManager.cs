using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RegisterManager : MonoBehaviour
{
	void Start()
	{
		string username = PlayerPrefs.GetString("username");
		if (username.Length > 0)
		{
			PlayerData.player.username = username;
			SceneManager.LoadScene("StartScene");
		}
	}
}
