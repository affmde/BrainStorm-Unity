using UnityEngine.SceneManagement;
using UnityEngine;

public class ContinueButtonHandler : MonoBehaviour
{
	private GameObject saveSound;
	private SceneTransitionManager sceneTransition;

	private void Awake()
	{
		saveSound = GameObject.Find("SaveButtonSound");
		sceneTransition = GameObject.Find("LevelLoader").GetComponent<SceneTransitionManager>();
	}
	public void Continue()
	{
		saveSound.GetComponent<HandleAudioButtons>().PlaySound();
		PlayerData.Reset();
		SaveData.Save();
		sceneTransition.LoadNextScene("LevelsScene");
	}
}
