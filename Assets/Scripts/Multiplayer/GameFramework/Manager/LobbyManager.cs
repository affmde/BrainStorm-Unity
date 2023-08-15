using System.Collections.Generic;
using System.Collections;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Game.Events;
using GameFramework.Events;
using UnityEngine;

namespace GameFramework_Core.GameFramework_Manager
{
	public class LobbyManager : Singleton<LobbyManager>
	{
		private Lobby lobby;
		private Coroutine hearBeatCoroutine;
		private Coroutine refreshLobbyCoroutine;

		public string GetJoinCode() { return lobby?.LobbyCode; }
		public string GetHostId() { return lobby.HostId; }

		public async Task<bool> CreatLobby(int maxPlayers, bool isPrivate, Dictionary<string, string> data, Dictionary<string, string> lobbyData)
		{
			Dictionary<string, PlayerDataObject> playerData = SerializePlayerData(data);
			Player player = new Player(AuthenticationService.Instance.PlayerId, null, playerData);
			CreateLobbyOptions options = new CreateLobbyOptions()
			{
				Data = SerializeLobbyData(lobbyData),
				IsPrivate = isPrivate,
				Player = player
			};
			try {
				lobby = await LobbyService.Instance.CreateLobbyAsync("Brainstorm Lobby", maxPlayers, options);
			} catch (System.Exception e) {
				Debug.Log(e);
				return false;
			}
			Debug.Log("Lobby created with lobby ID: " + lobby.Id);
			hearBeatCoroutine = StartCoroutine(HeartBeatLobbyCoroutine(lobby.Id, 6f));
			refreshLobbyCoroutine = StartCoroutine(RefreshLobbyLobbyCoroutine(lobby.Id, 1f));
			return true;
		}


		public async Task<bool> JoinLobby(string joinCode, Dictionary<string, string> playerData)
		{
			JoinLobbyByCodeOptions options = new JoinLobbyByCodeOptions();
			Player player = new Player(AuthenticationService.Instance.PlayerId, null, SerializePlayerData(playerData));
			options.Player = player;
			try {
				lobby = await LobbyService.Instance.JoinLobbyByCodeAsync(joinCode, options);
			} catch (System.Exception e) {
				Debug.Log(e);
				return false;
			}
			refreshLobbyCoroutine = StartCoroutine(RefreshLobbyLobbyCoroutine(lobby.Id, 1f));
			return true;
		}

		private IEnumerator HeartBeatLobbyCoroutine(string lobbyId, float waitTimeSeconds)
		{
			while (true)
			{
				Debug.Log("HeatBeat");
				LobbyService.Instance.SendHeartbeatPingAsync(lobbyId);
				yield return new WaitForSeconds(waitTimeSeconds);
			}
		}

		private IEnumerator RefreshLobbyLobbyCoroutine(string lobbyId, float waitTimeSeconds)
		{
			while (true)
			{
				Debug.Log("HeatBeat");
				Task<Lobby> task = LobbyService.Instance.GetLobbyAsync(lobbyId);
				yield return new WaitUntil(() => task.IsCompleted);
				Lobby newLobby = task.Result;
				if (newLobby.LastUpdated > lobby.LastUpdated)
				{
					lobby = newLobby;
					GameFramework.Events.LobbyEvents.OnLobbyUpdated?.Invoke(lobby);
				}
				yield return new WaitForSeconds(waitTimeSeconds);
			}
		}

		private Dictionary<string, PlayerDataObject> SerializePlayerData(Dictionary<string, string> data)
		{
			Dictionary<string, PlayerDataObject> playerData = new Dictionary<string, PlayerDataObject>();
			foreach (var (key, value) in data )
			{
				playerData.Add(key, new PlayerDataObject(
					visibility: PlayerDataObject.VisibilityOptions.Member,
					value: value
				));
			}
			return playerData;
		}

		private Dictionary<string, DataObject> SerializeLobbyData(Dictionary<string, string> data)
		{
			Dictionary<string, DataObject> lobbyData = new Dictionary<string, DataObject>();
			foreach(var (key, value) in data)
			{
				lobbyData.Add(key, new DataObject(
					visibility: DataObject.VisibilityOptions.Member,
					value: value
				));
			}
			return lobbyData;
		}

		public void OnApplicationQuit()
		{
			if (lobby != null && lobby.HostId == AuthenticationService.Instance.PlayerId)
			{
				LobbyService.Instance.DeleteLobbyAsync(lobby.Id);
			}
		}

		public List<Dictionary<string, PlayerDataObject>> GetPlayersData()
		{
			List<Dictionary<string, PlayerDataObject>> data = new List<Dictionary<string, PlayerDataObject>>();

			foreach (Player player in lobby.Players)
				data.Add(player.Data);
			return data;
		}

		public async Task<bool> UpdatePlayerData(string id, Dictionary<string, string> data, string allocationId = default, string connectionData = default)
		{
			Dictionary<string, PlayerDataObject> playerData = SerializePlayerData(data);
			UpdatePlayerOptions options = new UpdatePlayerOptions()
			{
				Data = playerData,
				AllocationId = allocationId,
				ConnectionInfo = connectionData
			};
			try {
				await LobbyService.Instance.UpdatePlayerAsync(lobby.Id, id, options);
			} catch (System.Exception e) {
				Debug.Log(e);
				return false;
			}
			GameFramework.Events.LobbyEvents.OnLobbyUpdated(lobby);
			return true;
		}

		public async Task<bool> UpdateLobbyData(Dictionary<string, string> data)
		{
			Dictionary<string, DataObject> lobbyData = SerializeLobbyData(data);
			UpdateLobbyOptions options = new UpdateLobbyOptions()
			{
				Data = lobbyData
			};
			try {
				lobby = await LobbyService.Instance.UpdateLobbyAsync(lobby.Id, options);
			} catch (System.Exception e) {
				Debug.Log(e);
				return false;
			}
			GameFramework.Events.LobbyEvents.OnLobbyUpdated(lobby);
			return true;
		}

	}

}