using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using Unity.Netcode;

public class PlayerAnswer : NetworkBehaviour
{
	[SerializeField] int activeButton;
	[SerializeField] float timestamp;
	[SerializeField] int totalCorrectAnswers = 0;
	[SerializeField] bool winner;
	[SerializeField] private string username;
	[SerializeField] private int level;
	[SerializeField] private int id;

	public override void OnNetworkSpawn()
	{
		if (!IsOwner) return ;
		base.OnNetworkSpawn();

		string name = PlayerData.player.username;
		username = name;
		level = PlayerData.player.level;
		id = (int)NetworkManager.Singleton.LocalClientId;
		UpdateNewPlayerDataServerRpc(name, level);
	}

	[ServerRpc(RequireOwnership = false)]
	private void UpdateNewPlayerDataServerRpc(string name, int lvl, ServerRpcParams serverRpcParams = default)
	{
		var clientId = serverRpcParams.Receive.SenderClientId;
		if (NetworkManager.ConnectedClients.ContainsKey(clientId))
		{
			var client = NetworkManager.Singleton.ConnectedClients[clientId];
			PlayerAnswer pl = client.PlayerObject.GetComponent<PlayerAnswer>();
			pl.username = name;
			pl.level = lvl;
			pl.Id = (int)clientId;
		}
	}
	public void OnEnable()
	{
		MultiplayerActions.onMultiplayerGameReset += ResetGameStatsCallback;
	}

	public void OnDisable()
	{
		MultiplayerActions.onMultiplayerGameReset -= ResetGameStatsCallback;
	}
	public string Username
	{
		get => username;
	}

	public int Id
	{
		get => id;
		set => id = value;
	}

	public int TotalCorrectAnswers
	{
		get => totalCorrectAnswers;
		set => totalCorrectAnswers = value;
	}

	public int Level
	{
		get => level;
		set => level = value;
	}

	public void IncreaseTotalCorrectAnswers() { totalCorrectAnswers++; }

	public float Timestamp
	{
		get => timestamp;
		set => timestamp = value;
	}

	public bool Winner
	{
		get => winner;
		set => winner = value;
	}

	public int ActiveButton
	{
		get => activeButton;
		set => activeButton = value;
	}

	public void SetActiveButton(int button)
	{
		if (!IsLocalPlayer)
			return;
		if (button < 1 || button > 4)
			return;
		activeButton = button;
	}

	public void ButtonClickedHandler(MultiplayerColourButton button, float clickTime)
	{
		if (!IsOwner) return;

		if (button.ButtonId == activeButton)
			activeButton = 0;
		else
			activeButton = button.ButtonId;
		//timestamp = GetMicrosecondsTimestamp();
		timestamp = clickTime;
		button.SetShowSelectImage(activeButton);
		UpdatePlayerOptionServerRpc(activeButton, timestamp);
	}

	[ServerRpc]
	private void UpdatePlayerOptionServerRpc(int button, float time)
	{
		activeButton = button;
		timestamp = time;
	}

	private void ResetGameStatsCallback()
	{
		TotalCorrectAnswers = 0;
		Winner = false;
		ActiveButton = 0;
		Timestamp = 0;
	}
}
