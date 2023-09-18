using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingPanelScript : MonoBehaviour
{
	public void Start()
	{
		gameObject.SetActive(true);
		MenuManager.onLoading += ToggleLoadingView;
	}

	public void OnDestroy()
	{
		MenuManager.onLoading -= ToggleLoadingView;
	}

	public void ToggleLoadingView(bool val)
	{
		gameObject.SetActive(val);
	}
}
