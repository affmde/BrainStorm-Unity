using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using TMPro;

public class EndGameUIManager : NetworkBehaviour
{
	[SerializeField] private bool winner;
	[SerializeField] private TextMeshProUGUI winnerStateText;
	[SerializeField] private TextMeshProUGUI pointsText;
	[SerializeField] private TextMeshProUGUI usernameText;
	[SerializeField] private GameObject continueButton;
	[SerializeField] private XPBar xpBar;
	[SerializeField] private Image fillBar;

	public bool Winner
	{
		get => winner;
		set => winner = value;
	}

	private void OnEnable()
	{
		EndGameUIManagerActions.onEndSceneEnter += WinnerStateChangedCallback;
		continueButton.SetActive(false);
		xpBar.gameObject.SetActive(true);
		xpBar.SetXPUpdated(false);
	}

	private void OnDisable()
	{
		EndGameUIManagerActions.onEndSceneEnter -= WinnerStateChangedCallback;
	}


	private void Start()
	{
		continueButton.SetActive(false);
		usernameText.text = PlayerData.player.username;
	}

	private void WinnerStateChangedCallback(bool state)
	{
		xpBar.gameObject.SetActive(true);
		switch(state)
		{
			case true:
				WinnerScene();
				break;
			case false:
				LooserScene();
				break;
			default:
				break;
		}
	}

	private void WinnerScene()
	{
		NetworkObject no = NetworkManager.LocalClient.PlayerObject;
		PlayerAnswer pl = no.GetComponent<PlayerAnswer>();

		winnerStateText.text = "You won!";
		pointsText.text = "" + pl.TotalCorrectAnswers;
		StartCoroutine(XPBarFill(100));
	}

	private void LooserScene()
	{
		NetworkObject no = NetworkManager.LocalClient.PlayerObject;
		PlayerAnswer pl = no.GetComponent<PlayerAnswer>();

		winnerStateText.text = "You Lost!";
		pointsText.text = "" + pl.TotalCorrectAnswers;
		StartCoroutine(XPBarFill(15));
	}

	private IEnumerator XPBarFill(int xpAmount)
	{
		xpBar.UpdateXP(xpAmount);
		while (!xpBar.IsXPUpdated())
			yield return null;
		xpBar.gameObject.SetActive(false);
		continueButton.SetActive(true);
	}
}

public static class EndGameUIManagerActions
{
	public static Action<bool> onEndSceneEnter;
}
