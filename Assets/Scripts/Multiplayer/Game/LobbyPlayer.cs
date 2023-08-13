using TMPro;
using UnityEngine;
using GameFramework_Core.Data;

namespace Game
{
	public class LobbyPlayer : MonoBehaviour
	{
		[SerializeField] TextMeshProUGUI playerName;
		LobbyPlayerData data;

		public void SetData(LobbyPlayerData lobbyPlayerData)
		{
			data = lobbyPlayerData;
			playerName.text = lobbyPlayerData.GamerTag;
			gameObject.SetActive(true);
		}
	}
}
