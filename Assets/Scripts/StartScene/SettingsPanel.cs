using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsPanel : MonoBehaviour
{
	public void ReadInputString(string str)
	{
		PlayerData.player.username = str;
	}

	public void SaveNewUsername()
	{
		PlayerPrefs.SetString("username", PlayerData.player.username);
		SaveData.Save();
	}

	public void ClosePanel()
	{
		gameObject.SetActive(false);
	}
}
