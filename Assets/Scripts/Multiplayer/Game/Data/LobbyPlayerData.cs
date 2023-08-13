using System.Collections.Generic;
using Unity.Services.Lobbies.Models;

namespace GameFramework_Core.Data
{
	public class LobbyPlayerData
	{
		private string id;
		private string gamerTag;
		private string name;
		private int level;
		private bool isReady;

		public string Id => id;
		public string GamerTag => gamerTag;
		public bool IsReady
		{
			get => isReady;
			set => isReady = value;
		}

		public void Initialize(string identification, string gamertag, string playerName, int lvl)
		{
			id = identification;
			gamerTag = gamertag;
			name = playerName;
			level = lvl;
			isReady = false;
		}

		public void Initialize(Dictionary<string, PlayerDataObject> playerData)
		{
			UpdateState(playerData);
		}

		public void UpdateState(Dictionary<string, PlayerDataObject> playerData)
		{
			if (playerData.ContainsKey("id"))
				id = playerData["id"].Value;
			if (playerData.ContainsKey("gamerTag"))
				gamerTag = playerData["gamerTag"].Value;
			if (playerData.ContainsKey("name"))
				name = playerData["name"].Value;
			if (playerData.ContainsKey("level"))
				level = int.Parse(playerData["level"].Value);
			if (playerData.ContainsKey("isReady"))
				isReady = playerData["isReady"].Value == "True" ? true : false;
		}

		public Dictionary<string, string> Serialize()
		{
			return new Dictionary<string, string>()
			{
				{"id", id},
				{"gamerTag", gamerTag},
				{"name", name},
				{"level", level.ToString()},
				{"isReady", isReady.ToString()}
			};
		}
	}
}
