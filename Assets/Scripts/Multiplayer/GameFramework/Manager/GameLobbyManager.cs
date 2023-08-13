using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;
using GameFramework_Core;
using GameFramework_Core.Data;
using GameFramework_Core.GameFramework_Manager;
using GameFramework.Events;
using Unity.Services.Authentication;
using Unity.Services.Lobbies.Models;
using Game.Events;
using UnityEngine;

namespace Game
{
	public class GameLobbyManager : Singleton<GameLobbyManager>
	{

		private List<LobbyPlayerData> lobbyPlayersData = new List<LobbyPlayerData>();
		private LobbyPlayerData localLobbyPlayerData;
		private void OnEnable()
		{
			GameFramework.Events.LobbyEvents.OnLobbyUpdated += OnLobbyUpdated;
		}

		private void OnDisable()
		{
			GameFramework.Events.LobbyEvents.OnLobbyUpdated -= OnLobbyUpdated;
		}
		public string GetJoinCode() { return LobbyManager.Instance.GetJoinCode(); }

		public async Task<bool> CreateLobby()
		{
			LobbyPlayerData data = new LobbyPlayerData();
			data.Initialize(AuthenticationService.Instance.PlayerId, "HostPlayer", PlayerData.player.username, PlayerData.player.level);
			bool succeeded = await LobbyManager.Instance.CreatLobby(4, true, data.Serialize());
			Debug.Log("Lobby created successfuly");
			return succeeded;
		}

		public async Task<bool> JoinLobby(string joinCode)
		{
			LobbyPlayerData data = new LobbyPlayerData();
			data.Initialize(AuthenticationService.Instance.PlayerId, "JoinPlayer", PlayerData.player.username, PlayerData.player.level);

			bool succeeded = await LobbyManager.Instance.JoinLobby(joinCode, data.Serialize());
			Debug.Log("Joined successfuly to lobby");
			return succeeded;
		}

		private void OnLobbyUpdated(Lobby lobby)
		{
			List<Dictionary<string, PlayerDataObject>> playerData = LobbyManager.Instance.GetPlayersData();
			lobbyPlayersData.Clear();
			foreach (Dictionary<string, PlayerDataObject> data in playerData)
			{
				LobbyPlayerData lobbyPlayerData= new LobbyPlayerData();
				lobbyPlayerData.Initialize(data);
				if (lobbyPlayerData.Id == AuthenticationService.Instance.PlayerId)
					localLobbyPlayerData = lobbyPlayerData;

				lobbyPlayersData.Add(lobbyPlayerData);
			}

			Events.LobbyEvents.OnLobbyUpdated?.Invoke();
		}

		public List<LobbyPlayerData> GetPlayers()
		{
			return lobbyPlayersData;
		}

	}

}
