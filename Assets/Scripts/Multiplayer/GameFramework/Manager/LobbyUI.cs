using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Game
{
	public class LobbyUI : MonoBehaviour
	{
		[SerializeField] TextMeshProUGUI joinCode;

		public void Start()
		{
			joinCode.text = "Code: " + GameLobbyManager.Instance.GetJoinCode();
		}
	}
}
