using TMPro;
using UnityEngine;
using GameFramework_Core.Data;
using UnityEngine.UI;

namespace Game
{
	public class LobbyPlayer : MonoBehaviour
	{
		[SerializeField] TextMeshProUGUI playerName;
		[SerializeField] TextMeshProUGUI playerLevel;
		[SerializeField] Image isReadyImage;
		LobbyPlayerData data;

		public void SetData(LobbyPlayerData lobbyPlayerData)
		{
			data = lobbyPlayerData;
			playerName.text = lobbyPlayerData.Name;
			playerLevel.text = lobbyPlayerData.Level.ToString();
			if (data.IsReady)
				isReadyImage.gameObject.SetActive(true);
			gameObject.SetActive(true);
		}
	}
}
