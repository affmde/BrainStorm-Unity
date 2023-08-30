using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class PlayersConnectedLobbyUI : NetworkBehaviour
{
	[SerializeField] private GameObject playerLobbyPrefab;
	[SerializeField] private RectTransform contentTransform;

	private void OnEnable()
	{
		MenuManager.onUpdateTotalPlayersConnected += UpdatePlayersLobby;
		MenuManager.onUpdateLobbyAfterDisconnect += UpdateLobbyAfterDisconnect;
	}

	private void OnDisable()
	{
		MenuManager.onUpdateTotalPlayersConnected -= UpdatePlayersLobby;
		MenuManager.onUpdateLobbyAfterDisconnect -= UpdateLobbyAfterDisconnect;
	}

	private void UpdatePlayersLobby(int totalPlayersConnected)
	{
		List<string> playersName = new List<string>();
		List<int>playersLevel = new List<int>();
		foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
		{
			PlayerAnswer pl = client.PlayerObject.GetComponent<PlayerAnswer>();
			playersName.Add(pl.Username);
			playersLevel.Add(pl.Level);
		}
		UpdatePlayersLobbyClientRpc(UtilsClass.instance.SerializeString(playersName), UtilsClass.instance.SerializeInt(playersLevel));
	}

	[ClientRpc]
	private void UpdatePlayersLobbyClientRpc(string names, string levels)
	{
		int childCount = contentTransform.childCount;
		for (int i = childCount - 1; i >= 0; i--)
		{
			GameObject child = contentTransform.GetChild(i).gameObject;
			Destroy(child);
		}

		List<string> playersName = UtilsClass.instance.DeserializeString(names); 
		List<int>playersLevel = UtilsClass.instance.DeserializeInt(levels);
		for(int i = 0; i < playersName.Count; i++)
		{
			GameObject newElement = Instantiate(playerLobbyPrefab, contentTransform);
			PlayerLobbyPrefabData data = newElement.GetComponent<PlayerLobbyPrefabData>();
			data.PlayerName = playersName[i];
			data.PlayerLevel = "" + playersLevel[i];
		}
	}

	private void UpdateLobbyAfterDisconnect(int disconnectedId)
	{
		List<string> playersName = new List<string>();
		List<int>playersLevel = new List<int>();
		foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
		{
			PlayerAnswer pl = client.PlayerObject.GetComponent<PlayerAnswer>();
			if (pl.Id == disconnectedId)
				continue;
			playersName.Add(pl.Username);
			playersLevel.Add(pl.Level);
		}
		UpdateLobbyAfterDisconnectClientRpc(UtilsClass.instance.SerializeString(playersName),
			UtilsClass.instance.SerializeInt(playersLevel));
	}

	[ClientRpc]
	private void UpdateLobbyAfterDisconnectClientRpc(string names, string levels)
	{
		int childCount = contentTransform.childCount;
		for (int i = childCount - 1; i >= 0; i--)
		{
			GameObject child = contentTransform.GetChild(i).gameObject;
			Destroy(child);
		}

		List<string> playersName = UtilsClass.instance.DeserializeString(names); 
		List<int>playersLevel = UtilsClass.instance.DeserializeInt(levels);
		for(int i = 0; i < playersName.Count; i++)
		{

			GameObject newElement = Instantiate(playerLobbyPrefab, contentTransform);
			PlayerLobbyPrefabData data = newElement.GetComponent<PlayerLobbyPrefabData>();
			data.PlayerName = playersName[i];
			data.PlayerLevel = "" + playersLevel[i];
		}
	}
}
