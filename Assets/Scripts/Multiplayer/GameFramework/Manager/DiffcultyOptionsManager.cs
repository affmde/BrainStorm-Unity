using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiffcultyOptionsManager : MonoBehaviour
{
	public static DiffcultyOptionsManager instance;
	public static Action onUpdateDifficultySettings;
	public enum TypeOfGame {BestOf, TimeBase, LimitOfQuestions}
	public static Action<bool> SetShowSettingsPanel;
	[SerializeField] private TypeOfGame gameType;
	[SerializeField] private float timeToAnswer = 2.0f;

	[SerializeField] private int maxPointsToWin = 10;
	[SerializeField] private int maxQuestions = 20;
	[SerializeField] private float maxLimitTime = 60;

	public TypeOfGame GameType
	{
		get => gameType;
		set => gameType = value;
	}

	public float MaxTimeToAnswer
	{
		get => timeToAnswer;
		set => timeToAnswer = value;
	}

	public int MaxPointsToWin
	{
		get => maxPointsToWin;
		set => maxPointsToWin = value;
	}

	public int MaxQuestions
	{
		get => maxQuestions;
		set => maxQuestions = value;
	}
	public float MaxLimitTime
	{
		get => maxLimitTime;
		set => maxLimitTime = value;
	}
	
	private void Awake()
	{
		if (instance == null)
			instance = this;
		else
			Destroy(gameObject);
	}

	private void Start()
	{
		maxPointsToWin = 10;
		maxQuestions = 20;
		maxLimitTime = 60f;
		timeToAnswer = 2f;
	}
}
