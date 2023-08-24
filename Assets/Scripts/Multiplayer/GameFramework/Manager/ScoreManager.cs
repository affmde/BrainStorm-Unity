using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ScoreManager : NetworkBehaviour
{
	[SerializeField] private int player1Score;
	[SerializeField] private int player2Score;
	private string player1Name;
	private string player2Name;
	[SerializeField] int round;
	[SerializeField] int maxPoints;

	public int MaxPoints
	{
		get => maxPoints;
		set => maxPoints = value;
	}

	public void OnEnable()
	{
		MultiplayerActions.onMultiplayerGameReset += ResetGameStatsCallback;
	}

	public void OnDisable()
	{
		MultiplayerActions.onMultiplayerGameReset -= ResetGameStatsCallback;
	}

	private void Start()
	{
		player1Score = 0;
		player2Score = 0;
		round = 0;
	}

	public int Round
	{
		get => round;
		set => round = value;
	}

	public int Player1Score
	{
		get => player1Score;
		set => player1Score = value;
	}

	public int Player2Score
	{
		get => player2Score;
		set => player2Score = value;
	}

	public string Player1Name
	{
		get => player1Name;
		set => player1Name = value;
	}

	public string Player2Name
	{
		get => player2Name;
		set => player2Name = value;
	}

	private void ResetGameStatsCallback()
	{
		player1Score = 0;
		player2Score = 0;
		round = 0;
	}
}
