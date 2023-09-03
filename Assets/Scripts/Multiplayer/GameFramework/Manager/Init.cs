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
			//MenuManager.onLoading?.Invoke(true);
			AuthenticatingAPlayer();
		}

		async void AuthenticatingAPlayer()
		{
			try
			{
				await UnityServices.InitializeAsync();
				//await AuthenticationService.Instance.SignInAnonymouslyAsync();
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
				
				// Shows how to get the playerID
				Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");
				return true;

			}
			catch (AuthenticationException ex)
			{
				// Compare error code to AuthenticationErrorCodes
				// Notify the player with the proper error message
				Debug.Log("AuthenticationException");
				Debug.LogException(ex);
				return false;
			}
			catch (RequestFailedException ex)
			{
				// Compare error code to CommonErrorCodes
				// Notify the player with the proper error message
				Debug.Log("RequestFailedException");
				Debug.LogException(ex);
				return false;
			}
			catch (Exception ex)
			{
				// Compare error code to CommonErrorCodes
				// Notify the player with the proper error message
				Debug.Log("Exception");
				Debug.LogException(ex);
				return false;
			}
		}
	}

}
