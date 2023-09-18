using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Threading.Tasks;
using Unity.Services.Core;
using Unity.Services.Authentication;

#if UNITY_EDITOR
	using ParrelSync;
#endif

namespace Game
{
	public class Init : MonoBehaviour
	{
		private void Start()
		{
			AuthenticatingAPlayer();
		}

		async void AuthenticatingAPlayer()
		{
			try
			{
				await UnityServices.InitializeAsync();
				#if UNITY_EDITOR
				if (ParrelSync.ClonesManager.IsClone())
				{
					// When using a ParrelSync clone, switch to a different authentication profile to force the clone
					// to sign in as a different anonymous user account.
					string customArgument = ParrelSync.ClonesManager.GetArgument();
					AuthenticationService.Instance.SwitchProfile($"Clone_{customArgument}_Profile");
				}
				#endif
				bool succeeded = await SignInAnonymouslyAsync();
				if (!succeeded)
					SceneManager.LoadScene("StartScene");
				else
				{
					var playerID = AuthenticationService.Instance.PlayerId;
					Debug.Log("Authenticated with ID: " + playerID);
					SceneManager.LoadScene("MultiplayerMenu");
				}
			}
			catch (Exception e)
			{
				Debug.Log("Failed to initialize. Error: " + e);
				GameObject networkManager = GameObject.Find("NetworkManager");
				if (networkManager)
					Destroy(networkManager);
				SceneManager.LoadScene("StartScene");
			}
		}

		async Task<bool> SignInAnonymouslyAsync()
		{
			try
			{
				await AuthenticationService.Instance.SignInAnonymouslyAsync();
				Debug.Log("Sign in anonymously succeeded!");
				
				Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");
				return true;

			}
			catch (AuthenticationException ex)
			{
				Debug.Log("AuthenticationException");
				Debug.LogException(ex);
				return false;
			}
			catch (RequestFailedException ex)
			{
				Debug.Log("RequestFailedException");
				Debug.LogException(ex);
				return false;
			}
			catch (Exception ex)
			{
				Debug.Log("Exception");
				Debug.LogException(ex);
				return false;
			}
		}
	}

}
