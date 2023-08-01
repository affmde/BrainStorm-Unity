using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
	//[SerializeField] string sceneName;
	public void ChangeScene()
	{
		Debug.Log("Detected click to change scene");
		SceneManager.LoadScene("StartScene");
	}
}
