using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingPanelScript : MonoBehaviour
{
	public void Start()
	{
		Debug.Log("ToggleLoadingView registered");
		MenuManager.onLoading += ToggleLoadingView;
	}

	public void OnDestroy()
	{
		MenuManager.onLoading -= ToggleLoadingView;
		Debug.Log("ToggleLoadingView registered unregistered");
	}

	public void ToggleLoadingView(bool val)
	{
		Debug.Log("ToggleLoadingView called and set to " + val);
		gameObject.SetActive(val);
	}
}
