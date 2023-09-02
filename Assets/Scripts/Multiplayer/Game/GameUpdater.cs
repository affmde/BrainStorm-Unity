using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
using UnityEngine.UI;

public class GameUpdater : NetworkBehaviour
{
	[SerializeField] private MultiplayerGameManager mgm;
	private List<Sprite> images;
	private List<Image> buttons;
	private float timer;
	private TextMeshProUGUI taskDescription;
	[SerializeField] TextMeshProUGUI roundText;
	[SerializeField] ScoreManager scoreManager;

	private void Start()
	{
		images = mgm.GetImages();
		buttons = mgm.GetButtons();
		timer = mgm.GetTimer();
		taskDescription = mgm.GetTaskDescription();
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
		MenuManager.onResetGame += ResetFullGameStatsCallback;
		MenuManager.onGameUpdate += GameUpdateCallback;
		MenuManager.onResetTask += ResetTaskCallback;
	}
	private void NetworkManager_OnServerStopped(bool unused)
	{
		MenuManager.onGameUpdate -= GameUpdateCallback;
		MenuManager.onResetTask -= ResetTaskCallback;
		MenuManager.onResetGame -= ResetFullGameStatsCallback;
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


	private void GameUpdateCallback()
	{
		int random = Random.Range(0, LevelsData.levelsList.Count);
		mgm.Index = random;
		UpdateGameClientRpc(random);
	}

	[ClientRpc]
	private void UpdateGameClientRpc(int random)
	{
		mgm.GameOn = true;
		
		buttons[0].sprite = GetSprite(LevelsData.levelsList[random].button1);
		buttons[1].sprite = GetSprite(LevelsData.levelsList[random].button2);
		buttons[2].sprite = GetSprite(LevelsData.levelsList[random].button3);
		buttons[3].sprite = GetSprite(LevelsData.levelsList[random].button4);
		foreach(Image btn in buttons)
			btn.GetComponent<MultiplayerColourButton>().UnsetSelectImage();
		
		taskDescription.text = LevelsData.levelsList[random].task;
		scoreManager.Round++;
		roundText.text = "Round " + scoreManager.Round;
	}

	private void ResetTaskCallback()
	{
		foreach(var client in NetworkManager.Singleton.ConnectedClientsList)
		{
			NetworkObject no = client.PlayerObject;
			PlayerAnswer pl = no.GetComponent<PlayerAnswer>();
			pl.ActiveButton = 0;
			pl.Timestamp = 0;
		}
		ResetPlayerOptionClientRpc();
	}

	[ClientRpc]
	private void ResetPlayerOptionClientRpc()
	{
		NetworkObject no = NetworkManager.LocalClient.PlayerObject;
		PlayerAnswer pl = no.GetComponent<PlayerAnswer>();
		pl.ActiveButton = 0;
		pl.Timestamp = 0;
		foreach(Image button in buttons)
			button.GetComponent<MultiplayerColourButton>().UnsetSelectImage();
		taskDescription.text = "";
	}

	private Sprite GetSprite(string color)
	{
		if (color == "Blue")
			return images[0];
		else if (color == "Green")
			return images[1];
		else if (color == "Red")
			return images[2];
		else if (color == "White")
			return images[3];
		else
			return images[4];
	}

	private void ResetFullGameStatsCallback()
	{
		ResetFullGameStatsCallbackClientRpc();
	}

	[ClientRpc]
	private void ResetFullGameStatsCallbackClientRpc()
	{
		roundText.text = "";
		MultiplayerActions.onMultiplayerGameReset?.Invoke();
	}
}
