using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class Manager : MonoBehaviour
{
	private void Awake()
	{
		LoadPlayerData();
		LoadLevelItems.LoadInfo();
		LoadLevels.LoadLevelsConfig();
	}

	public void Play()
	{
		SceneManager.LoadScene("LevelsScene");
	}

	
	void LoadPlayerData()
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
		LoadData.Load(PlayerData.player);
	}
}