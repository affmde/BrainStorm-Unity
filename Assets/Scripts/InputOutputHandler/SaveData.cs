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
		string completedLevels = "[CompletedLevelsEasy]0";
		for (int i = 0; i < PlayerData.player.completedLevelsList[0].completedLevels.Count; i++)
			completedLevels += "," + PlayerData.player.completedLevelsList[0].completedLevels[i];
		completedLevels += "[-CompletedLevelsEasy]";
		string completedLevelsMedium = "[CompletedLevelsMedium]0";
		for (int i = 0; i < PlayerData.player.completedLevelsList[1].completedLevels.Count; i++)
			completedLevelsMedium += "," + PlayerData.player.completedLevelsList[1].completedLevels[i];
		completedLevelsMedium += "[-CompletedLevelsMedium]";
		string completedLevelsHard = "[CompletedLevelsHard]0";
		for (int i = 0; i < PlayerData.player.completedLevelsList[2].completedLevels.Count; i++)
			completedLevelsMedium += "," + PlayerData.player.completedLevelsList[2].completedLevels[i];
		completedLevelsHard += "[-CompletedLevelsHard]";
		string currentLevel = "[CurrentLevel]" + PlayerData.player.currentGameLevel + "[-CurrentLevel]";
		
		file.WriteLine(start);
		file.WriteLine(timestamp);
		file.WriteLine(username);
		file.WriteLine(level);
		file.WriteLine(xp);
		file.WriteLine(completedLevels);
		file.WriteLine(completedLevelsMedium);
		file.WriteLine(completedLevelsHard);
		file.WriteLine(currentLevel);

		file.Close();
	}
}
