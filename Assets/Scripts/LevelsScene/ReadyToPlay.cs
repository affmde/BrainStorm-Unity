using UnityEngine;

public class ReadyToPlay : MonoBehaviour
{
	private GameObject playSound;
	private GameObject cancelSound;
	private Animator audioManagerAnimator;
	private SceneTransitionManager sceneTransition;

	private void Awake()
	{
		playSound = GameObject.Find("PlayButtonSound");
		cancelSound = GameObject.Find("CloseMenuButtonSound");
		sceneTransition = GameObject.Find("LevelLoader").GetComponent<SceneTransitionManager>();
		audioManagerAnimator = GameObject.Find("AudioManager").GetComponent<Animator>();
	}
	private void Start()
	{
		gameObject.SetActive(false);
	}

	public void Play()
	{
		playSound.GetComponent<HandleAudioButtons>().PlaySound();
		audioManagerAnimator.SetTrigger("FadeOut");
		sceneTransition.LoadNextScene("GameScene");
	}

	public void ClosePanel()
	{
		cancelSound.GetComponent<HandleAudioButtons>().PlaySound();
		gameObject.SetActive(false);
	}
}
