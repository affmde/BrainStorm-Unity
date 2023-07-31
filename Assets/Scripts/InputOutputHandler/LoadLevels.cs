using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class LoadLevels
{
	private void LoadLevelsData(Level level)
	{
		StreamReader file = new StreamReader("Assets/Data/levels.txt");
		while (!file.EndOfStream)
		{
			string line = file.ReadLine();
			if (line.Contains(StaticLevels.startLevel))
			{
				
			}
		}
	}
}
