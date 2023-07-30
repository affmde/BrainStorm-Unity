using UnityEngine.SceneManagement;
using UnityEngine;

public class ContinueButtonHandler : MonoBehaviour
{
	public void Continue()
	{
		SceneManager.LoadScene("LevelsScene");
	}
}
