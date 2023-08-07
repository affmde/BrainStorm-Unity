using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
	[SerializeField] Animator transitionAnimator;
	private float transitionDuration = 2f;
	bool isAlreadyAnimated;


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
		transitionAnimator.SetTrigger("Start");
		yield return new WaitForSeconds(transitionDuration);
		SceneManager.LoadScene(sceneName);
	}
}
