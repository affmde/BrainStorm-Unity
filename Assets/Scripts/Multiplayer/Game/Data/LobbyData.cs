using System.Collections.Generic;
using Unity.Services.Lobbies.Models;


namespace GameFramework_Core.Data
{
	public class LobbyData
	{
		private int difficultyIndex;

		public int DifficultyIndex { get => difficultyIndex; set => difficultyIndex = value; }

		public void Initialize(int index)
		{
			difficultyIndex = index;
		}

		public void Initialize(Dictionary<string, DataObject> lobbyData)
		{
			UpdateState(lobbyData);
		}

		public void UpdateState(Dictionary<string, DataObject> lobbyData)
		{
			if (lobbyData.ContainsKey("difficultyIndex"))
				difficultyIndex = int.Parse(lobbyData["difficultyIndex"].Value);
		}

		public Dictionary<string, string> Serialize()
		{
			return new Dictionary<string, string>()
			{
				{"difficultyIndex", difficultyIndex.ToString()}
			};
		}
	}
}
