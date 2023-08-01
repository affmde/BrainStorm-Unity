using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BadgesPanelHandler : MonoBehaviour
{
	[SerializeField] Image closeButton;


	public void Close()
	{
		gameObject.SetActive(false);
	}
}
