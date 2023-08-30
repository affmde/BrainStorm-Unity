using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class PlayersScoreObjectsUI : NetworkBehaviour
{
	[SerializeField] private GameObject playerScorePrefab;
	[SerializeField] private RectTransform contentTransform;
	private GridLayoutGroup gridLayout;

	private void Start()
	{
		gridLayout = contentTransform.GetComponent<GridLayoutGroup>();
	}

	private void OnEnable()
	{
		MenuManager.onStartSetPlayerData += PopulatePlayerScoreObjects;
	}

	private void OnDisable()
	{
		MenuManager.onStartSetPlayerData -= PopulatePlayerScoreObjects;
	}

	private void PopulatePlayerScoreObjects()
	{
		if (!IsServer) return;
		PopulatePlayerScoreObjectsClientRpc();
	}

	[ClientRpc]
	private void PopulatePlayerScoreObjectsClientRpc()
	{
		NetworkObject client = NetworkManager.Singleton.LocalClient.PlayerObject;
		PlayerAnswer pl = client.GetComponent<PlayerAnswer>();
		PlayerScoreObjectData data = playerScorePrefab.GetComponent<PlayerScoreObjectData>();
		data.PlayerName = pl.Username;
		data.PlayerPoints = "" + 0;
		data.Id = (int)NetworkManager.LocalClientId;
		data.FillAmount = 0;
	}
}
