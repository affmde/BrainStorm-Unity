using UnityEngine.SceneManagement;
using UnityEngine;

public class ContinueButtonHandler : MonoBehaviour
{
	private GameObject saveSound;
	private SceneTransitionManager sceneTransition;
	private EndGameManager manager;
	private void Awake()
	{
		saveSound = GameObject.Find("SaveButtonSound");
		sceneTransition = GameObject.Find("LevelLoader").GetComponent<SceneTransitionManager>();
		manager = GameObject.Find("Manager").GetComponent<EndGameManager>();
	}
	public void Continue()
	{
		manager.FadeOutSound();
		saveSound.GetComponent<HandleAudioButtons>().PlaySound();
		PlayerData.Reset();
		SaveData.Save();
		sceneTransition.LoadNextScene("LevelsScene");
	}
}
