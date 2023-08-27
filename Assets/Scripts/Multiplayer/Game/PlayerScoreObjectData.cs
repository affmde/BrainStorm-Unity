using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerScoreObjectData : MonoBehaviour
{
	[SerializeField] private int score;
	[SerializeField] private TextMeshProUGUI playerName;
	[SerializeField] private TextMeshProUGUI playerPoints;
	[SerializeField] private Image fillCircle;
	[SerializeField] private float fillAmount;
	[SerializeField] private int id;
	[SerializeField] private MultiplayerGameManager mgm;

	private void OnEnable()
	{
		MultiplayerActions.onIncreasePlayerScore += IncreaseScore;
	}

	private void OnDisable()
	{
		MultiplayerActions.onIncreasePlayerScore -= IncreaseScore;
	}

	public string PlayerName
	{
		get => playerName.text;
		set => playerName.text = value;
	}

	public int Score
	{
		get => score;
		set => score = value;
	}

	public string PlayerPoints
	{
		get => playerPoints.text;
		set => playerPoints.text = value;
	}

	public float FillAmount
	{
		get => fillAmount;
		set => fillAmount = value;
	}

	public int Id
	{
		get => id;
		set => id = value;
	}

	private void IncreaseScore()
	{
		score++;
		playerPoints.text = "" + score;
		fillAmount = (float)score / (float)mgm.Total;
		fillCircle.fillAmount = fillAmount;
	}
}
