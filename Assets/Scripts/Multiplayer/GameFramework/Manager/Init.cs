using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
#if UNITY_EDITOR
	using ParrelSync;
#endif

namespace Game
{
	public class Init : MonoBehaviour
	{
		[SerializeField] GameObject loadingPanel;
		private async void Start()
		{
			loadingPanel.SetActive(true);
			await UnityServices.InitializeAsync();
			if (ParrelSync.ClonesManager.IsClone())
			{
				// When using a ParrelSync clone, switch to a different authentication profile to force the clone
				// to sign in as a different anonymous user account.
				string customArgument = ParrelSync.ClonesManager.GetArgument();
				AuthenticationService.Instance.SwitchProfile($"Clone_{customArgument}_Profile");
			}

			if (UnityServices.State == ServicesInitializationState.Initialized)
			{
				AuthenticationService.Instance.SignedIn += OnSignedIn;
				await AuthenticationService.Instance.SignInAnonymouslyAsync();
				if (AuthenticationService.Instance.IsSignedIn)
				{
					string username = "" + PlayerData.player.username;
				}
				loadingPanel.SetActive(false);
			}
		}

		private void OnSignedIn()
		{
			Debug.Log("Signed in. Id: " + AuthenticationService.Instance.PlayerId);
		}
	}

}
