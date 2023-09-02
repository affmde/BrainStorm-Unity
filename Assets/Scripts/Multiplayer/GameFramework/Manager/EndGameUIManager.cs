using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using TMPro;

public class EndGameUIManager : NetworkBehaviour
{
	[SerializeField] private bool winner;
	[SerializeField] private GameObject continueButton;
	[SerializeField] private XPBar xpBar;
	
	[SerializeField] private GameObject winMusic;
	[SerializeField] private GameObject looseMusic;
	[SerializeField] private HandleAudioButtons gameOverSound;
	[SerializeField] private HandleAudioButtons congratsSound;
	[SerializeField] private RectTransform contentPaneltransform;
	[SerializeField] private GameObject playerPrefab;
	[SerializeField] private int idWinner;
	[SerializeField] private TextMeshProUGUI winnerUsername;
	private float timer;

	public bool Winner
	{
		get => winner;
		set => winner = value;
	}

	private void OnEnable()
	{
		continueButton.SetActive(false);
		xpBar.gameObject.SetActive(true);
		xpBar.SetXPUpdated(false);
	}

	private void Start()
	{
		continueButton.SetActive(false);
		winnerUsername.enabled = false;
	}

	public override void OnNetworkSpawn()
	{
		base.OnNetworkSpawn();

		NetworkManager.OnServerStarted += NetworkManager_OnServerStarted;
		NetworkManager.OnServerStopped += NetworkManager_OnServerStopped;
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

	private void NetworkManager_OnServerStarted()
	{
		if (!IsServer) return;
		EndGameUIManagerActions.onEndSceneEnter += EndSceneHandler;
	}

	private void NetworkManager_OnServerStopped(bool unused)
	{
		EndGameUIManagerActions.onEndSceneEnter -= EndSceneHandler;
	}

	private void EndSceneHandler()
	{
		if (!IsServer) return;
		List<string> playersName = new List<string>();
		List<int> playersPoints = new List<int>();
		
		foreach(var client in NetworkManager.Singleton.ConnectedClientsList)
		{
			PlayerAnswer pl = client.PlayerObject.GetComponent<PlayerAnswer>();
			playersName.Add(pl.Username);
			playersPoints.Add(pl.TotalCorrectAnswers);
			if (pl.Winner)
			{
				idWinner = pl.Id;
				winnerUsername.text = pl.Username;
			}
		}
		CreateAndShowAllPlayersPrefabsClientRpc(UtilsClass.instance.SerializeString(playersName), UtilsClass.instance.SerializeInt(playersPoints), idWinner, winnerUsername.text);
	}

	[ClientRpc]
	private void CreateAndShowAllPlayersPrefabsClientRpc(string names, string points, int winnerId, string winnerName)
	{
		if ((int)NetworkManager.LocalClientId == winnerId)
			winMusic.GetComponent<HandleAudioButtons>().PlaySound();
		else
			looseMusic.GetComponent<HandleAudioButtons>().PlaySound();
		winnerUsername.text = winnerName + " won!";

		List<string> playersNames = new List<string>();
		List<int> playersPoints = new List<int>();

		playersNames = UtilsClass.instance.DeserializeString(names);
		playersPoints = UtilsClass.instance.DeserializeInt(points);
		for (int i = 0; i < playersNames.Count; i++)
		{
			GameObject newElement = Instantiate(playerPrefab, contentPaneltransform);
			PlayerScoreObjectData data = newElement.GetComponent<PlayerScoreObjectData>();
			data.PlayerName = playersNames[i];
			data.PlayerPoints = "" + playersPoints[i];
			StartCoroutine(data.AnimatePointsBar(2.5f, playersPoints[i]));
		}
		StartCoroutine(WaitForSummary(winnerId));
	}

	private IEnumerator XPBarFill(int xpAmount)
	{
		xpBar.UpdateXP(xpAmount);
		while (!xpBar.IsXPUpdated())
			yield return null;
		xpBar.gameObject.SetActive(false);
		continueButton.SetActive(true);
	}

	private IEnumerator WaitForSummary(int winnerId)
	{
		while (timer < 2.5f)
		{
			timer += Time.deltaTime;
			yield return null;
		}
		winnerUsername.enabled = true;
		if ((int)NetworkManager.LocalClientId == winnerId)
		{
			congratsSound.PlaySound();
			StartCoroutine(XPBarFill(150));
		}
		else
		{
			gameOverSound.PlaySound();
			StartCoroutine(XPBarFill(15));
		}
	}
}

public static class EndGameUIManagerActions
{
	public static Action onEndSceneEnter;
}
