using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsPanel : MonoBehaviour
{
	private GameObject saveSound;
	private GameObject closeMenuSound;

	private void Awake()
	{
		saveSound = GameObject.Find("SaveButtonSound");
		closeMenuSound = GameObject.Find("CloseMenuButtonSound");
	}
	public void ReadInputString(string str)
	{
		PlayerData.player.username = str;
	}

	public void SaveNewUsername()
	{
		saveSound.GetComponent<HandleAudioButtons>().PlaySound();
		PlayerPrefs.SetString("username", PlayerData.player.username);
		SaveData.Save();
	}

	public void ClosePanel()
	{
		closeMenuSound.GetComponent<HandleAudioButtons>().PlaySound();
		gameObject.SetActive(false);
	}
}
