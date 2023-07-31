using UnityEngine.SceneManagement;
using UnityEngine;

public class ContinueButtonHandler : MonoBehaviour
{
	public void Continue()
	{
		PlayerData.Reset();
		SceneManager.LoadScene("LevelsScene");
	}
}
