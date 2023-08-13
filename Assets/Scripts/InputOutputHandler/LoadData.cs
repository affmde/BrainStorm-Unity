using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class LoadData
{
	public static void Load(PlayerDataInfo player)
	{
		string path = Application.persistentDataPath + "/Data";
		string fileName = "save.txt";
		if (!File.Exists(path + "/" + fileName))
			CreateFile(path, fileName);
		ReadFile(path, fileName, player);
	}

	private static string	ParseInputBetweenTags(string line, string startTag, string endTag)
	{
		string input = line.Substring(startTag.Length, line.Length - startTag.Length - endTag.Length);
		return input;
	}

	private static void ReadFile(string path, string filename, PlayerDataInfo player)
	{
		StreamReader file = new StreamReader(path + "/" + filename);
		while (!file.EndOfStream)
		{
			string line = file.ReadLine();
			if (line.Contains("[Level]"))
				player.level = int.Parse(ParseInputBetweenTags(line, "[Level]", "[-Level]"));
			else if (line.Contains("[XP]"))
				player.xp = int.Parse(ParseInputBetweenTags(line, "[XP]", "[-XP]"));
			else if (line.Contains("[CurrentLevel]"))
				player.currentGameLevel = int.Parse(ParseInputBetweenTags(line, "[CurrentLevel]", "[-CurrentLevel]"));
			else if (line.Contains("[CompletedLevelsEasy]"))
			{
				string input = ParseInputBetweenTags(line, "[CompletedLevelsEasy]", "[-CompletedLevelsEasy]");
				string[] arr = input.Split(",");
				for(int i = 0; i < arr.Length; i++)
					player.completedLevelsList[0].completedLevels.Add(int.Parse(arr[i]));
			}
			else if (line.Contains("[EasyCurrentLevel]"))
				player.completedLevelsList[0].currentLevel = int.Parse(ParseInputBetweenTags(line, "[EasyCurrentLevel]", "[-EasyCurrentLevel]"));
			else if (line.Contains("[CompletedLevelsMedium]"))
			{
				string input = ParseInputBetweenTags(line, "[CompletedLevelsMedium]", "[-CompletedLevelsMedium]");
				string[] arr = input.Split(",");
				for(int i = 0; i < arr.Length; i++)
					player.completedLevelsList[1].completedLevels.Add(int.Parse(arr[i]));
			}
			else if (line.Contains("[MediumCurrentLevel]"))
				player.completedLevelsList[1].currentLevel = int.Parse(ParseInputBetweenTags(line, "[MediumCurrentLevel]", "[-MediumCurrentLevel]"));
			else if (line.Contains("[CompletedLevelsHard]"))
			{
				string input = ParseInputBetweenTags(line, "[CompletedLevelsHard]", "[-CompletedLevelsHard]");
				string[] arr = input.Split(",");
				for(int i = 0; i < arr.Length; i++)
					player.completedLevelsList[2].completedLevels.Add(int.Parse(arr[i]));
			}
			else if (line.Contains("[HardCurrentLevel]"))
				player.completedLevelsList[2].currentLevel = int.Parse(ParseInputBetweenTags(line, "[HardCurrentLevel]", "[-HardCurrentLevel]"));
			else if (line.Contains("[SoundOn]"))
			{
				int soundOn = int.Parse(ParseInputBetweenTags(line, "[SoundOn]", "[-SoundOn]"));
				PlayerData.player.isSoundOn = soundOn == 0 ? false : true;
			}
			player.username = PlayerPrefs.GetString("username");
		}
		file.Close();
	}

	private static void CreateFile(string path, string fileName)
	{
		System.IO.Directory.CreateDirectory(path);
		StreamWriter file = File.AppendText(path + "/" + fileName);
		string dataSave = "[DataSaved]";
		string timeStamp = "[TimeStamp]" + System.DateTime.Now + "[-Timestamp]";
		string username = "[Username]" + PlayerPrefs.GetString("username") + "[-Username]";
		string level = "[Level]1[-Level]";
		string xp = "[XP]0[-XP]";
		string completedLevelsEasy = "[CompletedLevelsEasy]0[-CompletedLevelsEasy]";
		string easyCurrentLevel = "[EasyCurrentLevel]1[-EasyCurrentLevel]";
		string completedLevelsMedium = "[CompletedLevelsMedium]0[-CompletedLevelsMedium]";
		string mediumCurrentLevel = "[MediumCurrentLevel]1[-MediumCurrentLevel]";
		string completedLevelsHard = "[CompletedLevelsHard]0[-CompletedLevelsHard]";
		string hardCurrentLevel = "[HardCurrentLevel]1[-HardCurrentLevel]";
		string soundOn = "[SoundOn]1[-SoundOn]";
		string end = "[EndData]";
		file.WriteLine(dataSave);
		file.WriteLine(timeStamp);
		file.WriteLine(level);
		file.WriteLine(xp);
		file.WriteLine(completedLevelsEasy);
		file.WriteLine(easyCurrentLevel);
		file.WriteLine(completedLevelsMedium);
		file.WriteLine(mediumCurrentLevel);
		file.WriteLine(completedLevelsHard);
		file.WriteLine(hardCurrentLevel);
		file.WriteLine(soundOn);
		file.WriteLine(end);
		file.Close();
	}
}
