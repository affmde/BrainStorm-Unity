using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
	[SerializeField] Animator transitionAnimator;
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
		transitionAnimator.SetTrigger("Start");
		yield return new WaitForSeconds(transitionDuration);
		SceneManager.LoadScene(sceneName);
	}
}
