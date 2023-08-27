using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class GameAnswersManager : NetworkBehaviour
{
	private List<PlayerAnswer> playersAnswer;
	[SerializeField] ScoreManager scoreManager;
	[SerializeField] Image hostFillBar;
	[SerializeField] Image clientFillBar;
	[SerializeField] float hostFillAmount;
	[SerializeField] float clientFillAmount;
	[SerializeField] private HandleAudioButtons getPointSound;
	[SerializeField] private HandleAudioButtons dontGetPointSound;
	[SerializeField] private PlayerScoreObjectData playerScoreData;
	private void Start()
	{
		playersAnswer = new List<PlayerAnswer>();
	}

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
		playersAnswer.Clear();
		foreach(var client in NetworkManager.Singleton.ConnectedClientsList)
		{
			NetworkObject no = client.PlayerObject;
			PlayerAnswer pl = no.GetComponent<PlayerAnswer>();
			playersAnswer.Add(pl);
		}
		string winner = CheckWinner(LevelsData.levelsList[number].validOptions, playersAnswer[0], playersAnswer[1]);
		if (winner == "-1")
			return;
		AddPointToPlayer(winner);
	}

	private string CheckWinner(List<int> validOptions, PlayerAnswer host, PlayerAnswer client)
	{

		List<NetworkClient> playersWithCorrectAnswer = new List<NetworkClient>();
		
		foreach(var clientObject in NetworkManager.Singleton.ConnectedClientsList)
		{
			PlayerAnswer pl = clientObject.PlayerObject.GetComponent<PlayerAnswer>();
			if (validOptions.Contains(pl.ActiveButton))
				playersWithCorrectAnswer.Add(clientObject);
		}
		if (playersWithCorrectAnswer.Count <= 0)
			return "-1";

		List<int> fasterClients = new List<int>();
		long fasterTime;

		fasterTime = playersWithCorrectAnswer[0].PlayerObject.GetComponent<PlayerAnswer>().Timestamp;
		fasterClients.Add(playersWithCorrectAnswer[0].PlayerObject.GetComponent<PlayerAnswer>().Id);
		for (int i = 1; i < playersWithCorrectAnswer.Count; i++)
		{
			PlayerAnswer pl = playersWithCorrectAnswer[i].PlayerObject.GetComponent<PlayerAnswer>();
			if (pl.Timestamp == fasterTime)
				fasterClients.Add(pl.Id);
			else if (pl.Timestamp < fasterTime)
			{
				fasterTime = pl.Timestamp;
				fasterClients.Clear();
				fasterClients.Add(pl.Id);
			}
		}
		return UtilsClass.instance.SerializeInt(fasterClients);
	}

	private int CheckOnePlayerWinner(List<int> validOptions, PlayerAnswer host)
	{
		if (validOptions.Contains(host.ActiveButton) || (validOptions[0] == 0 && host.ActiveButton == 0))
			return 1;
		return 0;
	}

	private void AddPointToPlayer(string ids)
	{
		List<int> winningIds = UtilsClass.instance.DeserializeInt(ids);
		foreach (var winnerID in winningIds)
		{
			PlayerAnswer pl = NetworkManager.Singleton.ConnectedClients[(ulong)winnerID].PlayerObject.GetComponent<PlayerAnswer>();
			pl.TotalCorrectAnswers++;
		}
		AddPointToPlayerClientRpc(ids);
	}

	[ClientRpc]
	private void AddPointToPlayerClientRpc(string ids)
	{
		List<int> winningIds = UtilsClass.instance.DeserializeInt(ids);
		int localPlayerId = (int)NetworkManager.LocalClientId;
		if (!winningIds.Contains(localPlayerId))
			return;
		MultiplayerActions.onIncreasePlayerScore?.Invoke();
		NetworkObject no = NetworkManager.Singleton.LocalClient.PlayerObject;
		PlayerAnswer pl = no.GetComponent<PlayerAnswer>();
		pl.TotalCorrectAnswers = pl.TotalCorrectAnswers;
	}
	
}

