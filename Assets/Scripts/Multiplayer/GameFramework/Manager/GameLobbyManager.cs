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
		private LobbyData lobbyData;


		public bool IsHost => localLobbyPlayerData.Id == LobbyManager.Instance.GetHostId();
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
			localLobbyPlayerData = new LobbyPlayerData();
			localLobbyPlayerData.Initialize(AuthenticationService.Instance.PlayerId, "HostPlayer", PlayerData.player.username, PlayerData.player.level);
			lobbyData = new LobbyData();
			lobbyData.Initialize(0);
			bool succeeded = await LobbyManager.Instance.CreatLobby(4, true, localLobbyPlayerData.Serialize(), lobbyData.Serialize());
			Debug.Log("Lobby created successfuly");
			return succeeded;
		}

		public async Task<bool> JoinLobby(string joinCode)
		{
			localLobbyPlayerData = new LobbyPlayerData();
			localLobbyPlayerData.Initialize(AuthenticationService.Instance.PlayerId, "JoinPlayer", PlayerData.player.username, PlayerData.player.level);
			bool succeeded = await LobbyManager.Instance.JoinLobby(joinCode, localLobbyPlayerData.Serialize());
			Debug.Log("Joined successfuly to lobby");
			return succeeded;
		}

		private void OnLobbyUpdated(Lobby lobby)
		{
			List<Dictionary<string, PlayerDataObject>> playerData = LobbyManager.Instance.GetPlayersData();
			lobbyPlayersData.Clear();
			int numberOfPlayersReady = 0;
			foreach (Dictionary<string, PlayerDataObject> data in playerData)
			{
				LobbyPlayerData lobbyPlayerData= new LobbyPlayerData();
				lobbyPlayerData.Initialize(data);
				if (lobbyPlayerData.IsReady)
					numberOfPlayersReady++;
				if (lobbyPlayerData.Id == AuthenticationService.Instance.PlayerId)
					localLobbyPlayerData = lobbyPlayerData;

				lobbyPlayersData.Add(lobbyPlayerData);
			}
			lobbyData = new LobbyData();
			lobbyData.Initialize(lobby.Data);
			Events.LobbyEvents.OnLobbyUpdated?.Invoke();
			if (numberOfPlayersReady == lobby.Players.Count)
				Events.LobbyEvents.OnLobbyReady?.Invoke();
		}

		public List<LobbyPlayerData> GetPlayers()
		{
			return lobbyPlayersData;
		}

		public async Task<bool> SetPlayerReady()
		{
			localLobbyPlayerData.IsReady = true;
			return await LobbyManager.Instance.UpdatePlayerData(localLobbyPlayerData.Id, localLobbyPlayerData.Serialize());
		}

		public int GetDifficultyIndex() { return lobbyData.DifficultyIndex; }

		public async Task<bool> SetSelectedDifficulty(int currentDifficultyIndex)
		{
			lobbyData.DifficultyIndex = currentDifficultyIndex;
			return await LobbyManager.Instance.UpdateLobbyData(lobbyData.Serialize());
		}

	}

}
