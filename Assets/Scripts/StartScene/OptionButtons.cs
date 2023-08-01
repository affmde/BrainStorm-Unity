using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class OptionButtons : MonoBehaviour
{
	[SerializeField] string panelName;
	private GameObject panel;
	private GameObject changeUsernamePanel;
	private GameObject removeAccountPanel;
	private void Awake()
	{
		panel = GameObject.Find(panelName);
		if (panel)
		{
			changeUsernamePanel = GameObject.Find("ChangeUsernamePanel");
			removeAccountPanel = GameObject.Find("RemoveAccountPanel");
		}
	}

	private void Start()
	{
		if (panel)
			panel.SetActive(false);
		if (changeUsernamePanel)
			changeUsernamePanel.SetActive(false);
		if (removeAccountPanel)
			removeAccountPanel.SetActive(false);
	}

	public void OpenPanel()
	{
		if (panel)
			panel.SetActive(true);
	}

	public void OpenChangeUsernamePanel()
	{
		if (changeUsernamePanel)
			changeUsernamePanel.SetActive(true);
	}

	public void CloseChangeUsernamePanel()
	{
		changeUsernamePanel.SetActive(false);
	}

	public void RemoveAccount()
	{
		PlayerPrefs.SetString("username", "");
		string path = Application.persistentDataPath + "/Data";
		FileUtil.DeleteFileOrDirectory(path);
		Application.Quit();
	}

	public void OpenRemoveAccountPanel()
	{
		if (removeAccountPanel)
			removeAccountPanel.SetActive(true);
	}

	public void CloseRemoveAccountPanel()
	{
		if (removeAccountPanel)
			removeAccountPanel.SetActive(false);
	}
}
