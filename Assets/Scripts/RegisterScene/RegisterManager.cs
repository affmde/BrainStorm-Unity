using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterManager : MonoBehaviour
{
	[SerializeField] GameObject loadingPanel;
	private SceneTransitionManager sceneTransition;

	private void Awake()
	{
		sceneTransition = GameObject.Find("LevelLoader").GetComponent<SceneTransitionManager>();
	}
	void Start()
	{
		loadingPanel.SetActive(true);
		StartCoroutine(LoadData());
		AllocatePlayerData();
	}

	private IEnumerator LoadData()
	{
		yield return StartCoroutine(LoadLevelItems.ReadTextFileAndroid());
		yield return StartCoroutine(LoadLevelItems.BuildLevelItem());
		//LoadLevels.LoadLevelsConfig();
		//yield return StartCoroutine(LoadLevelItems.BuildLevelItem());
		if (PlayerPrefs.HasKey("username"))
		{
			string username = PlayerPrefs.GetString("username");
			PlayerData.player.username = username;
			sceneTransition.LoadNextScene("StartScene");
		}
		else
			loadingPanel.SetActive(false);
	}

	private void AllocatePlayerData()
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
	}
}
