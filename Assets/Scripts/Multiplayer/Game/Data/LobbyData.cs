using System.Collections.Generic;
using Unity.Services.Lobbies.Models;


namespace GameFramework_Core.Data
{
	public class LobbyData
	{
		private int difficultyIndex;
		private string relayJoinCode;

		public int DifficultyIndex { get => difficultyIndex; set => difficultyIndex = value; }

		public void Initialize(int index)
		{
			difficultyIndex = index;
		}

		public string RelayJoinCode
		{
			get => relayJoinCode;
			set => relayJoinCode = value;
		}

		public void Initialize(Dictionary<string, DataObject> lobbyData)
		{
			UpdateState(lobbyData);
		}

		public void UpdateState(Dictionary<string, DataObject> lobbyData)
		{
			if (lobbyData.ContainsKey("DifficultyIndex"))
				difficultyIndex = int.Parse(lobbyData["DifficultyIndex"].Value);
			if (lobbyData.ContainsKey("RelayJoinCode"))
				relayJoinCode = lobbyData["RelayJoinCode"].Value;
		}

		public Dictionary<string, string> Serialize()
		{
			return new Dictionary<string, string>()
			{
				{"DifficultyIndex", difficultyIndex.ToString()},
				{"RelayJoinCode", relayJoinCode}
			};
		}
	}
}
