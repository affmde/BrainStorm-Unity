using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class OptionButtons : MonoBehaviour
{
	[SerializeField] string panelName;
	private GameObject panel;
	private GameObject changeUsernamePanel;
	private GameObject removeAccountPanel;
	private GameObject optionsMenuSound;
	private GameObject clickSound;
	private GameObject closePanelSound;
	private GameObject removeAccountSoundButton;
	private void Awake()
	{
		panel = GameObject.Find(panelName);
		if (panel)
		{
			changeUsernamePanel = GameObject.Find("ChangeUsernamePanel");
			removeAccountPanel = GameObject.Find("RemoveAccountPanel");
		}
		optionsMenuSound = GameObject.Find("OptionsButtonSound");
		clickSound = GameObject.Find("ClickButtonSound");
		closePanelSound = GameObject.Find("CloseMenuButtonSound");
		removeAccountSoundButton = GameObject.Find("RemoveAccountButtonSound");
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
		Debug.Log("OPen panel called");
		if (panel)
		{
			Debug.Log("Panel detected. Now will open it");
			panel.SetActive(true);
			optionsMenuSound.GetComponent<HandleAudioButtons>().PlaySound();
		}
	}

	public void OpenChangeUsernamePanel()
	{
		if (changeUsernamePanel)
		{
			changeUsernamePanel.SetActive(true);
			clickSound.GetComponent<HandleAudioButtons>().PlaySound();
		}
	}

	public void CloseChangeUsernamePanel()
	{
		changeUsernamePanel.SetActive(false);
		closePanelSound.GetComponent<HandleAudioButtons>().PlaySound();
	}

	public void RemoveAccount()
	{
		removeAccountSoundButton.GetComponent<HandleAudioButtons>().PlaySound();
		if (PlayerPrefs.HasKey("username"))
			PlayerPrefs.DeleteKey("username");
		string path = Application.persistentDataPath + "/Data";
		if (Directory.Exists(path))
			Directory.Delete(path, true);
		Application.Quit();
	}

	public void OpenRemoveAccountPanel()
	{
		if (removeAccountPanel)
		{
			removeAccountPanel.SetActive(true);
			clickSound.GetComponent<HandleAudioButtons>().PlaySound();
		}
	}

	public void CloseRemoveAccountPanel()
	{
		if (removeAccountPanel)
		{
			removeAccountPanel.SetActive(false);
			closePanelSound.GetComponent<HandleAudioButtons>().PlaySound();
		}
	}

	public void QuitApplication()
	{
		clickSound.GetComponent<HandleAudioButtons>().PlaySound();
		Application.Quit();
	}
}
