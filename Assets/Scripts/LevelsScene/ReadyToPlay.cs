using UnityEngine;

public class ReadyToPlay : MonoBehaviour
{
	private GameObject playSound;
	private GameObject cancelSound;
	private SceneTransitionManager sceneTransition;

	private void Awake()
	{
		playSound = GameObject.Find("PlayButtonSound");
		cancelSound = GameObject.Find("CloseMenuButtonSound");
		sceneTransition = GameObject.Find("LevelLoader").GetComponent<SceneTransitionManager>();
	}
	private void Start()
	{
		gameObject.SetActive(false);
	}

	public void Play()
	{
		playSound.GetComponent<HandleAudioButtons>().PlaySound();
		sceneTransition.LoadNextScene("GameScene");
	}

	public void ClosePanel()
	{
		cancelSound.GetComponent<HandleAudioButtons>().PlaySound();
		gameObject.SetActive(false);
	}
}
