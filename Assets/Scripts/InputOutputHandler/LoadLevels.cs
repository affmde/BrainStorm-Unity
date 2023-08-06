using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class LoadLevels
{
	public static void LoadLevelsConfig()
	{
		StaticLevels.config = new List<LevelsConfig>();
		LoadLevelsData(StaticLevels.config);
	}
	private static void LoadLevelsData(List<LevelsConfig> levels)
	{
		//string fileName = "levels.txt";
		//string path = "Data/" + fileName;

		StreamReader file = new StreamReader("Assets/Resources/Data/levels.txt");
		//TextAsset textAsset = Resources.Load<TextAsset>(path);
		//string []lines = textAsset.text.Split("\n");
		int currentItem = 0;
		while (!file.EndOfStream)
		{
			string line = file.ReadLine();
			if (line.Contains(StaticLevels.startLevel))
			{
				LevelsConfig l = new LevelsConfig();
				levels.Add(l);
			}
			else if (line.Contains(StaticLevels.levelStart))
				levels[currentItem].level = int.Parse(ParseInputBetweenTags(line, StaticLevels.levelStart, StaticLevels.levelEnd));
			else if (line.Contains(StaticLevels.totalStart))
				levels[currentItem].total = int.Parse(ParseInputBetweenTags(line, StaticLevels.totalStart, StaticLevels.totalEnd));
			//else if (line.Contains(StaticLevels.timeStart))
			//	levels[currentItem].duration = float.Parse(ParseInputBetweenTags(line, StaticLevels.timeStart, StaticLevels.timeEnd));
			else if (line.Contains(StaticLevels.end))
				currentItem++;
		}
		file.Close();
	}

	private static string	ParseInputBetweenTags(string line, string startTag, string endTag)
	{
		string input = line.Substring(startTag.Length, line.Length - startTag.Length - endTag.Length);
		return input;
	}
}
