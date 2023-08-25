using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
	using ParrelSync;
#endif

namespace Game
{
	public class Init : MonoBehaviour
	{

		private void Start()
		{
			SceneManager.LoadScene("MultiplayerMenu");
		}
	}

}
