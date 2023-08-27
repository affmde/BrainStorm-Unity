using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerLobbyPrefabData : MonoBehaviour
{
	[SerializeField] TextMeshProUGUI playerName;
	[SerializeField] TextMeshProUGUI playerLevel;

	public string PlayerName
	{
		get => playerName.text;
		set => playerName.text = value;
	}

	public string PlayerLevel
	{
		get => playerLevel.text;
		set => playerLevel.text = value;
	}
}
