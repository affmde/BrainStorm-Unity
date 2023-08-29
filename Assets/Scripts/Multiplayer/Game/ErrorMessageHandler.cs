using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ErrorMessageHandler : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI message;
	[SerializeField] private bool isShowingMessage;
	[SerializeField] private float timer;

	public string Message
	{
		get => message.text;
		set => message.text = value;
	}

	public bool IsShowingMessage
	{
		get => isShowingMessage;
		set => isShowingMessage = value;
	}

	private void Start()
	{
		gameObject.SetActive(false);
	}

	public IEnumerator ShowMessage(float duration)
	{
		gameObject.SetActive(true);
		isShowingMessage = true;
		while (timer < duration)
		{
			timer += Time.deltaTime;
			yield return null;
		}
		timer = 0;
		isShowingMessage = false;
		gameObject.SetActive(false);
	}


}
