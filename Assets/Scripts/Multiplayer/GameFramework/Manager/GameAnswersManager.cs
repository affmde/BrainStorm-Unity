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
		int winner = 0;
		if (playersAnswer.Count > 1 )
			winner = CheckWinner(LevelsData.levelsList[number].validOptions, playersAnswer[0], playersAnswer[1]);
		else
			winner = CheckOnePlayerWinner(LevelsData.levelsList[number].validOptions, playersAnswer[0]); // This is for debuggin with only one player;
		if (winner == 1)
			HostPlayerAddPoint();
		else if (winner == 2)
			ClientPlayerAddPoint();
		MenuManager.onUpdatePlayerScore?.Invoke(scoreManager.Player1Score, scoreManager.Player2Score);
	}

	private int CheckWinner(List<int> validOptions, PlayerAnswer host, PlayerAnswer client)
	{
		if (validOptions[0] == 0 && host.ActiveButton == 0 && client.ActiveButton != 0)
			return 1;
		else if (validOptions[0] == 0 && host.ActiveButton != 0 && client.ActiveButton == 0)
			return 2;
		else if (validOptions[0] == 0 && host.ActiveButton != 0 && client.ActiveButton != 0)
			return 0;
		else if (validOptions[0] == 0 && host.ActiveButton == 0 && client.ActiveButton == 0)
		{
			if (host.Timestamp == client.Timestamp)
				return 0;
			else if (host.Timestamp < client.Timestamp)
				return 1;
			else
				return 2;
		}
		if (validOptions.Contains(host.ActiveButton) && !validOptions.Contains(client.ActiveButton))
			return 1;
		else if (!validOptions.Contains(host.ActiveButton) && validOptions.Contains(client.ActiveButton))
			return 2;
		else if (!validOptions.Contains(host.ActiveButton) && !validOptions.Contains(client.ActiveButton))
			return 0;
		else
		{
			if (host.Timestamp == client.Timestamp)
				return 0;
			else if (host.Timestamp < client.Timestamp)
				return 1;
			else
				return 2;
		}
	}

	private int CheckOnePlayerWinner(List<int> validOptions, PlayerAnswer host)
	{
		if (validOptions.Contains(host.ActiveButton) || (validOptions[0] == 0 && host.ActiveButton == 0))
			return 1;
		return 0;
	}

	private void HostPlayerAddPoint()
	{
		HostPlayerAddPointClientRpc();
	}
	[ClientRpc]
	private void HostPlayerAddPointClientRpc()
	{
		scoreManager.Player1Score++;
		hostFillAmount = (float)scoreManager.Player1Score / (float)scoreManager.MaxPoints;
		hostFillBar.fillAmount = hostFillAmount;
		clientFillAmount = (float)scoreManager.Player2Score / (float)scoreManager.MaxPoints;
		clientFillBar.fillAmount = clientFillAmount;
		if (IsServer)
		{
			NetworkObject no = NetworkManager.Singleton.LocalClient.PlayerObject;
			PlayerAnswer pl = no.GetComponent<PlayerAnswer>();
			pl.TotalCorrectAnswers = scoreManager.Player1Score;
			getPointSound.PlaySound();
		}
		else
		{
			dontGetPointSound.PlaySound();
		}

	}

	private void ClientPlayerAddPoint()
	{
		ClientPlayerAddPointClientRpc();
	}
	[ClientRpc]
	private void ClientPlayerAddPointClientRpc()
	{
		scoreManager.Player2Score++;
		hostFillAmount = (float)scoreManager.Player1Score / (float)scoreManager.MaxPoints;
		hostFillBar.fillAmount = hostFillAmount;
		clientFillAmount = (float)scoreManager.Player2Score / (float)scoreManager.MaxPoints;
		clientFillBar.fillAmount = clientFillAmount;
		if (!IsServer)
		{
			NetworkObject no = NetworkManager.Singleton.LocalClient.PlayerObject;
			PlayerAnswer pl = no.GetComponent<PlayerAnswer>();
			pl.TotalCorrectAnswers = scoreManager.Player2Score;
			getPointSound.PlaySound();
		}
		else
			dontGetPointSound.PlaySound();
	}
}

