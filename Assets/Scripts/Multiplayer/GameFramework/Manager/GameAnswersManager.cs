using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class GameAnswersManager : NetworkBehaviour
{
	[SerializeField] private HandleAudioButtons getPointSound;
	[SerializeField] private HandleAudioButtons dontGetPointSound;

	public override void OnNetworkSpawn()
	{
		base.OnNetworkSpawn();

		NetworkManager.OnServerStarted += NetworkManager_OnServerStarted;
		NetworkManager.OnServerStopped += NetworkManager_OnServerStopped;
	}

	private void NetworkManager_OnServerStarted()
	{
		if (!IsServer) return;

		MenuManager.onAnalizeAnswers += OnAnalizeAnswers;
	}

	private void NetworkManager_OnServerStopped(bool unused)
	{
		MenuManager.onAnalizeAnswers -= OnAnalizeAnswers;
	}

	public override void OnDestroy()
	{
		if (NetworkManager)
		{
			NetworkManager.OnServerStarted -= NetworkManager_OnServerStarted;
			NetworkManager.OnServerStopped -= NetworkManager_OnServerStopped;
		}

		base.OnDestroy();
	}

	private void OnAnalizeAnswers(int number)
	{
		CheckAnswers(number);
	}
	public void CheckAnswers(int number)
	{
		string winner = CheckWinner(LevelsData.levelsList[number].validOptions);
		if (winner == "-1")
			return;
		AddPointToPlayer(winner);
	}

	private string CheckWinner(List<int> validOptions)
	{

		List<NetworkClient> playersWithCorrectAnswer = new List<NetworkClient>();
		
		foreach(var clientObject in NetworkManager.Singleton.ConnectedClientsList)
		{
			PlayerAnswer pl = clientObject.PlayerObject.GetComponent<PlayerAnswer>();
			if (validOptions.Contains(pl.ActiveButton))
			{
				Debug.Log("Client " + pl.Id + " has correct answer. Added to list.");
				playersWithCorrectAnswer.Add(clientObject);
			}
		}
		if (playersWithCorrectAnswer.Count <= 0)
			return "-1";

		List<int> fasterClients = new List<int>();
		float fasterTime;

		fasterTime = playersWithCorrectAnswer[0].PlayerObject.GetComponent<PlayerAnswer>().Timestamp;
		fasterClients.Add(playersWithCorrectAnswer[0].PlayerObject.GetComponent<PlayerAnswer>().Id);
		Debug.Log("Faster time: " + fasterTime);
		Debug.Log("Client " + playersWithCorrectAnswer[0].PlayerObject.GetComponent<PlayerAnswer>().Id + " has time: " + playersWithCorrectAnswer[0].PlayerObject.GetComponent<PlayerAnswer>().Timestamp);
		for (int i = 1; i < playersWithCorrectAnswer.Count; i++)
		{
			PlayerAnswer pl = playersWithCorrectAnswer[i].PlayerObject.GetComponent<PlayerAnswer>();
			Debug.Log("Client " + pl.Id + " has time: " + pl.Timestamp);
			if (pl.Timestamp == fasterTime)
			{
				Debug.Log("CLient " + pl.Id + " has also faster time: " + pl.Timestamp + " Added to List");
				fasterClients.Add(pl.Id);
			}
			else if (pl.Timestamp < fasterTime)
			{
				Debug.Log("CLient " + pl.Id + " has faster time: " + pl.Timestamp);
				fasterTime = pl.Timestamp;
				fasterClients.Clear();
				Debug.Log("fasterClients list was cliered. Now has " + fasterClients.Count + " clients on.");
				fasterClients.Add(pl.Id);
				Debug.Log("Client " + pl.Id + " added to fasterClients list. List has now " + fasterClients.Count + " clients");
			}
		}
		return UtilsClass.instance.SerializeInt(fasterClients);
	}

	private void AddPointToPlayer(string ids)
	{
		Debug.Log("winnersID string: " + ids);
		List<int> winningIds = UtilsClass.instance.DeserializeInt(ids);
		foreach (var winnerID in winningIds)
		{
			Debug.Log("Client " + winnerID + " is winner");
			PlayerAnswer pl = NetworkManager.Singleton.ConnectedClients[(ulong)winnerID].PlayerObject.GetComponent<PlayerAnswer>();
			pl.TotalCorrectAnswers++;
			Debug.Log("Point added to client " + pl.Id + ". Now that client has " + pl.TotalCorrectAnswers + " points.");
		}
		AddPointToPlayerClientRpc(ids);
	}

	[ClientRpc]
	private void AddPointToPlayerClientRpc(string ids)
	{
		List<int> winningIds = UtilsClass.instance.DeserializeInt(ids);
		int localPlayerId = (int)NetworkManager.Singleton.LocalClientId;
		if (!winningIds.Contains(localPlayerId))
		{
			dontGetPointSound.PlaySound();
			return;
		}
		getPointSound.PlaySound();
		MultiplayerActions.onIncreasePlayerScore?.Invoke();
		NetworkObject no = NetworkManager.Singleton.LocalClient.PlayerObject;
		PlayerAnswer pl = no.GetComponent<PlayerAnswer>();
		pl.TotalCorrectAnswers = pl.TotalCorrectAnswers;
	}
	
}

