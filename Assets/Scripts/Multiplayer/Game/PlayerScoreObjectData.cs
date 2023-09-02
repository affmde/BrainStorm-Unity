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
	private MultiplayerGameManager mgm;
	private float timer;
	[SerializeField] bool isAnimated;


	private void Awake()
	{
		mgm = GameObject.Find("GamePanelManager").GetComponent<MultiplayerGameManager>();
	}
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

	public bool IsAnimated
	{
		get => isAnimated;
		set => isAnimated = value;
	}
	private void IncreaseScore()
	{
		score++;
		playerPoints.text = "" + score;
		fillAmount = (float)score / (float)DiffcultyOptionsManager.instance.MaxPointsToWin;
		fillCircle.fillAmount = fillAmount;
	}

	public IEnumerator AnimatePointsBar(float duration, int totalPoints)
	{
		float currentFill = 0;
		float maxFill = (float)totalPoints / (float)DiffcultyOptionsManager.instance.MaxPointsToWin;
		while (timer < duration)
		{
			timer += Time.deltaTime;
			currentFill = timer * maxFill;
			fillCircle.fillAmount = currentFill;
			yield return null;
		}
		isAnimated = true;
	}
}
