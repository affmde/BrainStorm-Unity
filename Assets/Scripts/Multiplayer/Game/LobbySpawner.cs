using UnityEngine;
using System.Collections.Generic;
using Game.Events;
using GameFramework_Core.Data;


namespace Game
{
	public class LobbySpawner : MonoBehaviour
	{
		[SerializeField] private List<LobbyPlayer> players;

		private void OnEnable()
		{
			LobbyEvents.OnLobbyUpdated += OnLobbyUpdated;
		}

		private void OnDisable()
		{
			LobbyEvents.OnLobbyUpdated -= OnLobbyUpdated;
		}

		private void OnLobbyUpdated()
		{
			List<LobbyPlayerData> playersData = GameLobbyManager.Instance.GetPlayers();

			for (int i = 0; i < playersData.Count; i++)
			{
				LobbyPlayerData data = playersData[i];
				players[i].SetData(data);
			}
		}
	}
}