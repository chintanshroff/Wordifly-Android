﻿using UnityEngine;
using System.Collections;
using Facebook.Unity;
using System.Collections.Generic;

public class FBHolder : MonoBehaviour
{
	List <string> perms = new List<string> (){ "public_profile", "email", "user_friends" };

	void Awake ()
	{
		if (!FB.IsInitialized)
		{
			FB.Init (InitCallback, OnHideUnity);
		}
		else
		{
			FB.ActivateApp ();
		}
	}

	private void InitCallback ()
	{
		if (FB.IsInitialized)
		{
			FB.ActivateApp ();
			if (FB.IsLoggedIn)
			{
				
			}
			else
			{
				FBLogin ();
				Console.Log ("FB log in done");

			}
		}
		else
		{
			Debug.Log ("Failed to Initialize the Facebook SDK");
		}
	}

	private void OnHideUnity (bool isGameShown)
	{
		if (!isGameShown)
		{
			Time.timeScale = 0;
		}
		else
		{
			Time.timeScale = 1;
		}
	}

	void FBLogin ()
	{
		FB.LogInWithReadPermissions (perms, AuthCallback);
	}



	private void AuthCallback (ILoginResult result)
	{
		if (FB.IsLoggedIn)
		{
			// AccessToken class will have session details
			var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
			// Print current access token's User ID
			Debug.Log (aToken.UserId);
			// Print current access token's granted permissions
			foreach (string perm in aToken.Permissions)
			{
				Debug.Log (perm);
			}
		}
		else
		{
			Debug.Log ("User cancelled login");
		}
	}
}
