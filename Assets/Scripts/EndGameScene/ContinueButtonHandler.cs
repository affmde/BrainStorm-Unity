using UnityEngine.SceneManagement;
using UnityEngine;

public class ContinueButtonHandler : MonoBehaviour
{
	private GameObject saveSound;

	private void Awake()
	{
		saveSound = GameObject.Find("SaveButtonSound");
	}
	public void Continue()
	{
		saveSound.GetComponent<HandleAudioButtons>().PlaySound();
		PlayerData.Reset();
		SaveData.Save();
		SceneManager.LoadScene("LevelsScene");
	}
}
