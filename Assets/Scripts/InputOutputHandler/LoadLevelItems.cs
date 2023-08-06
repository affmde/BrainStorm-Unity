using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class LoadLevelItems
{
	private static TextAsset	levelItemsText;
	public static IEnumerator ReadTextFileAndroid()
	{
		string fileName = "file";
		string path = "Data/" + fileName;
		string pathToSave = Application.persistentDataPath + "/Data/levelItems.txt";

		levelItemsText = Resources.Load<TextAsset>(path);
		//byte[] fileBytes = levelItemsText.bytes;
		//File.WriteAllBytes(pathToSave, fileBytes);
		if (!Directory.Exists(Application.persistentDataPath + "/Data"))
			Directory.CreateDirectory(Application.persistentDataPath + "/Data");
		File.WriteAllText(pathToSave, levelItemsText.text);
		yield return null;
		
	}
	public static IEnumerator BuildLevelItem()
	{
		string pathToRead = Application.persistentDataPath + "/Data/levelItems.txt";
		StreamReader reader = new StreamReader(pathToRead);
		//string [] lines = levelItemsText.text.Split("\n");
		//Debug.Log("total lines: " + lines.Length);
		LevelsData.levelsList = new List<Level>();
		int currentItem = 0;
		while (!reader.EndOfStream)
		{
			//Debug.Log("line: " + line);
			string line = reader.ReadLine();
			if (line.Contains("[StartItem]"))
			{
				Level l = new Level();
				l.validOptions = new List<int>();
				LevelsData.levelsList.Add(l);
			}
			else if (line.Contains("[Task]"))
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
		Debug.Log("total levels loaded: " + LevelsData.levelsList.Count);
		reader.Close();
		yield return null;
	}

	private static string	ParseInputBetweenTags(string line, string startTag, string endTag)
	{
		string input = line.Substring(startTag.Length, line.Length - startTag.Length - endTag.Length);
		return input;
	}
}
