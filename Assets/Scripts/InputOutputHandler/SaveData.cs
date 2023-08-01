using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class SaveData
{
	public static void Save()
	{
		string path = Application.persistentDataPath + "/Data/save.txt";
		StreamWriter file = new StreamWriter(path);
		string start = "[DataSaved]";
		string timestamp = "[Timestamp]" + System.DateTime.Now + "[-Timestamp]";
		string username = "[Username]" + PlayerData.player.username + "[-Username]";
		string level = "[Level]" + PlayerData.player.level + "[-Level]";
		string xp = "[XP]" + PlayerData.player.xp + "[-XP]";
		string completedLevels = "[CompletedLevelsEasy]";
		for (int i = 0; i < PlayerData.player.completedLevelsList[0].completedLevels.Count; i++)
			completedLevels += (i == 0 ? "" : ",") + PlayerData.player.completedLevelsList[0].completedLevels[i];
		completedLevels += "[-CompletedLevelsEasy]";
		string completedLevelsMedium = "[CompletedLevelsMedium]";
		for (int i = 0; i < PlayerData.player.completedLevelsList[1].completedLevels.Count; i++)
			completedLevelsMedium += (i == 0 ? "" : ",") + PlayerData.player.completedLevelsList[1].completedLevels[i];
		completedLevelsMedium += "[-CompletedLevelsMedium]";
		string completedLevelsHard = "[CompletedLevelsHard]";
		for (int i = 0; i < PlayerData.player.completedLevelsList[2].completedLevels.Count; i++)
			completedLevelsHard += (i == 0 ? "" : ",") + PlayerData.player.completedLevelsList[2].completedLevels[i];
		completedLevelsHard += "[-CompletedLevelsHard]";
		string easyCurrentLevel =  "[EasyCurrentLevel]" + PlayerData.player.completedLevelsList[0].currentLevel + "[-EasyCurrentLevel]";
		string mediumCurrentLevel =  "[MediumCurrentLevel]" + PlayerData.player.completedLevelsList[1].currentLevel + "[-MediumCurrentLevel]";
		string hardCurrentLevel =  "[HardCurrentLevel]" + PlayerData.player.completedLevelsList[2].currentLevel + "[-HardCurrentLevel]";
		string soundOn = "[SoundOn]" + (PlayerData.player.isSoundOn ? 1 : 0) + "[-SoundOn]";
		string end = "[EndData]";
		
		file.WriteLine(start);
		file.WriteLine(timestamp);
		file.WriteLine(username);
		file.WriteLine(level);
		file.WriteLine(xp);
		file.WriteLine(completedLevels);
		file.WriteLine(easyCurrentLevel);
		file.WriteLine(completedLevelsMedium);
		file.WriteLine(mediumCurrentLevel);
		file.WriteLine(completedLevelsHard);
		file.WriteLine(hardCurrentLevel);
		file.WriteLine(end);

		file.Close();
	}
}
