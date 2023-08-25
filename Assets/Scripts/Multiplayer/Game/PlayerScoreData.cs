using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
using UnityEngine.UI;

public class PlayerScoreData : NetworkBehaviour
{
	[Header("Elements")]
	[SerializeField] private TextMeshProUGUI hostName;
	[SerializeField] private TextMeshProUGUI clientName;
	[SerializeField] private TextMeshProUGUI hostCorrectAnswersText;
	[SerializeField] private TextMeshProUGUI clientCorrectAnswersText;
	[SerializeField] private ScoreManager scoreManager;
	[SerializeField] private List<PlayerAnswer> players;
	
	public override void OnNetworkSpawn()
	{
		base.OnNetworkSpawn();
		NetworkManager.OnServerStarted += NetworkManager_OnServerStarted;
	}



	private void NetworkManager_OnServerStarted()
	{
		if (!IsServer) return;
		MenuManager.onStartSetPlayerData += onUpdatePlayersNameCallback;
		MenuManager.onUpdatePlayerScore += OnUpdatePlayerScore;
		NetworkManager.OnServerStopped += NetworkManager_OnServerStopped;
	}

	private void NetworkManager_OnServerStopped(bool unused)
	{
		NetworkManager.OnServerStarted -= NetworkManager_OnServerStarted;
		MenuManager.onStartSetPlayerData -= onUpdatePlayersNameCallback;
		MenuManager.onUpdatePlayerScore -= OnUpdatePlayerScore;
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

	private void onUpdatePlayersNameCallback()
	{
		players.Clear();
		foreach(var client in NetworkManager.Singleton.ConnectedClientsList)
		{
			NetworkObject no = client.PlayerObject;
			PlayerAnswer pl = no.GetComponent<PlayerAnswer>();
			players.Add(pl);
		}
		if (players.Count > 1)
			UpdatePlayersNameClientRpc(players[0].Username, players[1].Username);
		else
			UpdatePlayersNameClientRpc(players[0].Username, "Client player");
	}


	private void OnUpdatePlayerScore(int hostScore, int clientScore)
	{
		OnUpdatePlayerScoreClientRpc(hostScore, clientScore);
		UpdateScoreTextsClientRpc();
	}

	[ClientRpc]
	private void UpdatePlayersNameClientRpc(string name1, string name2)
	{
		scoreManager.Player1Name = name1;
		scoreManager.Player2Name = name2;
		hostName.text = scoreManager.Player1Name;
		clientName.text = scoreManager.Player2Name;
	}

	[ClientRpc]
	private void OnUpdatePlayerScoreClientRpc(int hostScore, int clientScore)
	{
		scoreManager.Player1Score = hostScore;
		scoreManager.Player2Score = clientScore;

	}

	[ClientRpc]
	private void UpdateScoreTextsClientRpc()
	{
		hostCorrectAnswersText.text = "" + scoreManager.Player1Score;
		clientCorrectAnswersText.text = "" + scoreManager.Player2Score;
	}
}
