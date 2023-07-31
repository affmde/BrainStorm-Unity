using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class SaveData
{
	public static void Save()
	{
		StreamWriter file = new StreamWriter("Assets/Data/save.txt");
		string start = "[DataSaved]";
		string timestamp = "[Timestamp]" + System.DateTime.Now + "[-Timestamp]";
		string username = "[Username]" + PlayerData.player.username + "[-Username]";
		string level = "[Level]" + PlayerData.player.level + "[-Level]";
		string xp = "[XP]" + PlayerData.player.xp + "[-XP]";
		string completedLevels = "[CompletedLevels]0";
		for (int i = 0; i < PlayerData.player.levelsCompleted.Count; i++)
			completedLevels += "," + PlayerData.player.levelsCompleted[i];
		completedLevels += "[-CompletedLevels]";
		string currentLevel = "[CurrentLevel]" + PlayerData.player.currentGameLevel + "[-CurrentLevel]";
		
		file.WriteLine(start);
		file.WriteLine(timestamp);
		file.WriteLine(username);
		file.WriteLine(level);
		file.WriteLine(xp);
		file.WriteLine(completedLevels);
		file.WriteLine(currentLevel);

		file.Close();
	}
}
