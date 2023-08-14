using System;
using UnityEngine;
using System.Collections.Generic;

namespace GameFramework_Core.Data
{
	[CreateAssetMenu(menuName = "Data: DifficultySelectionData", fileName = "DifficultySelectionData")]
	public class DifficultySelectionData : ScriptableObject
	{
		public List<DifficultyInfo> difficulties;
	}
}

[Serializable]
public struct DifficultyInfo
{
	public int difficultyLevel;
	public int minDifficultyAcceptable;
	public int maxDifficultyAcceptable;
	public string levelName;

}

