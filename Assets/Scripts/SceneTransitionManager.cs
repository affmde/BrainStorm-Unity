using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
	[SerializeField] Animator logoAnimator;
	[SerializeField] Animator crossfadeAnimator;
	private Scene currentScene;
	private Animator soundAnimator;

	private float transitionDuration = 1f;
	bool isAlreadyAnimated;

	private void Awake()
	{
		soundAnimator = GameObject.Find("AudioManager").GetComponent<Animator>();
	}
	private void Start()
	{
		isAlreadyAnimated = false;
		currentScene = SceneManager.GetActiveScene();
		crossfadeAnimator.gameObject.SetActive(false);
		if (currentScene.name == "EndGameScene")
		{
			logoAnimator.gameObject.SetActive(false);
			crossfadeAnimator.gameObject.SetActive(true);
		}


	}
	public void LoadNextScene(string sceneName)
	{
		if (!isAlreadyAnimated)
			StartCoroutine(LoadScene(sceneName));
		isAlreadyAnimated = true;
	}

	private IEnumerator LoadScene(string sceneName)
	{
		soundAnimator.SetTrigger("Start");
		if (currentScene.name == "GameScene")
		{
			logoAnimator.gameObject.SetActive(false);
			crossfadeAnimator.gameObject.SetActive(true);
			crossfadeAnimator.SetTrigger("Start");
		}
		else if (currentScene.name == "EndGameScene")
		{
			logoAnimator.gameObject.SetActive(true);
			crossfadeAnimator.gameObject.SetActive(false);
			logoAnimator.SetTrigger("Start");
		}
		else
			logoAnimator.SetTrigger("Start");
		yield return new WaitForSeconds(transitionDuration);
		SceneManager.LoadScene(sceneName);
	}
}
