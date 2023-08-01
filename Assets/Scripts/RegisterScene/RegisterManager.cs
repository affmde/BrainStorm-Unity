using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RegisterManager : MonoBehaviour
{
	void Start()
	{
		PlayerData.player = new Player();
		PlayerData.player.completedLevelsList = new List<Lvl>();
		for(int i = 0; i < 3; i++)
		{
			Lvl l = new Lvl();
			PlayerData.player.completedLevelsList.Add(l);
		}
		PlayerData.player.completedLevelsList[0].completedLevels = new List<int>();
		PlayerData.player.completedLevelsList[1].completedLevels = new List<int>();
		PlayerData.player.completedLevelsList[2].completedLevels = new List<int>();
		string username = PlayerPrefs.GetString("username");
		if (username.Length > 0)
		{
			PlayerData.player.username = username;
			SceneManager.LoadScene("StartScene");
		}
	}
}
