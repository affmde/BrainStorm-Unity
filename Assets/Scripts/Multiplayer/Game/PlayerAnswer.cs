using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using Unity.Netcode;

public class PlayerAnswer : NetworkBehaviour
{
	[SerializeField] int activeButton;
	[SerializeField] long timestamp;
	[SerializeField] int totalCorrectAnswers = 0;
	[SerializeField] bool winner;
	MultiplayerGameManager mgm;
	[SerializeField] private string username;
	[SerializeField] private int level;
	[SerializeField] private int id;

	public override void OnNetworkSpawn()
	{
		base.OnNetworkSpawn();

		string name = PlayerData.player.username;
		if (name.Length < 1)
			username = "Player";
		else
			username = name;
		level = PlayerData.player.level;
		//id = (int)NetworkManager.Singleton.LocalClientId;
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

	public long Timestamp
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

	public void ButtonClickedHandler(MultiplayerColourButton button)
	{
		if (!IsOwner) return;

		if (button.ButtonId == activeButton)
			activeButton = 0;
		else
			activeButton = button.ButtonId;
		timestamp = GetMicrosecondsTimestamp();
		button.SetShowSelectImage(activeButton);
		UpdatePlayerOptionServerRpc(activeButton, timestamp);
	}

	[ServerRpc]
	private void UpdatePlayerOptionServerRpc(int button, long time)
	{
		activeButton = button;
		timestamp = time;
	}

	private long ToUnixTimestamp()
	{
		return (long)(DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
	}

	private long GetMicrosecondsTimestamp()
	{
		DateTime now = DateTime.UtcNow;
		long microseconds = now.Ticks / (TimeSpan.TicksPerMillisecond / 1000);
		return microseconds;
	}

	private void ResetGameStatsCallback()
	{
		TotalCorrectAnswers = 0;
		Winner = false;
		ActiveButton = 0;
		Timestamp = 0;
	}
}
