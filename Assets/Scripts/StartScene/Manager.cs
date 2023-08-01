using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class Manager : MonoBehaviour
{

	private void Awake()
	{
		LoadInfo();
		LoadPlayerData();
		LoadLevels.LoadLevelsConfig();
	}
	public void Play()
	{
		SceneManager.LoadScene("LevelsScene");
	}

	private void LoadInfo()
	{
		StreamReader file = new StreamReader("Assets/Data/levelSets.txt");
		BuildLevelItem(file);
		file.Close();
	}

	private void BuildLevelItem(StreamReader reader)
	{
		List<Level> list = new List<Level>();
		LevelsData.levelsList = list;
		int currentItem = 0;
		while (!reader.EndOfStream)
		{
			string line = reader.ReadLine();
			if (line.Contains("[StartItem]"))
			{
				Level l = new Level();
				l.validOptions = new List<int>();
				LevelsData.levelsList.Add(l);
			}
			if (line.Contains("[Task]"))
				LevelsData.levelsList[currentItem].task = ParseInputBetweenTags(line, "[Task]", "[-Task]");
			else if (line.Contains("[Button1]"))
				LevelsData.levelsList[currentItem].button1 = ParseInputBetweenTags(line, "[Button1]", "[-Button1]");
			else if (line.Contains("[Button2]"))
				LevelsData.levelsList[currentItem].button2 = ParseInputBetweenTags(line, "[Button2]", "[-Button2]");
			else if (line.Contains("[Button3]"))
				LevelsData.levelsList[currentItem].button3 = ParseInputBetweenTags(line, "[Button3]", "[-Button3]");
			else if (line.Contains("[Button4]"))
				LevelsData.levelsList[currentItem].button4 = ParseInputBetweenTags(line, "[Button4]", "[-Button4]");
			else if (line.Contains("[ValidOptions]"))
			{
				string input = ParseInputBetweenTags(line, "[ValidOptions]", "[-ValidOptions]");
				string[] arr = input.Split(",");
				for(int i = 0; i < arr.Length; i++)
					LevelsData.levelsList[currentItem].validOptions.Add(int.Parse(arr[i]));
			}
			//else if (line.Contains("[TimeLimit]"))
			//	LevelsData.levelsList[currentItem].time = float.Parse(ParseInputBetweenTags(line, "[TimeLimit]", "[-TimeLimit]"));
			else if (line.Contains("[Difficulty]"))
				LevelsData.levelsList[currentItem].difficulty = int.Parse(ParseInputBetweenTags(line, "[Difficulty]", "[-Difficulty]"));
			else if (line.Contains("[ID]"))
				LevelsData.levelsList[currentItem].id = int.Parse(ParseInputBetweenTags(line, "[ID]", "[-ID]"));
			else if (line.Contains("[EndItem]"))
				currentItem++;
		}
	}

	string	ParseInputBetweenTags(string line, string startTag, string endTag)
	{
		string input = line.Substring(startTag.Length, line.Length - startTag.Length - endTag.Length);
		return input;
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