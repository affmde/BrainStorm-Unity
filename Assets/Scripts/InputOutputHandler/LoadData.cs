using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class LoadData
{
	public static void Load(Player player)
	{
		StreamReader file = new StreamReader("Assets/Data/save.txt");
		while (!file.EndOfStream)
		{
			string line = file.ReadLine();
			if (line.Contains("[Level]"))
				player.level = int.Parse(ParseInputBetweenTags(line, "[Level]", "[-Level]"));
			else if (line.Contains("[XP]"))
				player.xp = int.Parse(ParseInputBetweenTags(line, "[XP]", "[-XP]"));
			else if (line.Contains("[CurrentLevel]"))
				player.currentGameLevel = int.Parse(ParseInputBetweenTags(line, "[CurrentLevel]", "[-CurrentLevel]"));
			else if (line.Contains("[CompletedLevels]"))
			{
				string input = ParseInputBetweenTags(line, "[CompletedLevels]", "[-CompletedLevels]");
				string[] arr = input.Split(",");
				for(int i = 0; i < arr.Length; i++)
					player.levelsCompleted.Add(int.Parse(arr[i]));
			}
			player.username = PlayerPrefs.GetString("username");
		}

		file.Close();
	}

	private static string	ParseInputBetweenTags(string line, string startTag, string endTag)
	{
		string input = line.Substring(startTag.Length, line.Length - startTag.Length - endTag.Length);
		return input;
	}
}
