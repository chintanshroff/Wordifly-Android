using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Analytics;
using System.Collections.Generic;
using Facebook.Unity;
using System;

public class GameOverHandler : MonoBehaviour
{
	public Animator myScoreAnim;
	public Animator myHiScoreAnim;
	public Text ScoreText_GO;
	public Text HiScoreText_GO;
	private Button myMainMenuBtn;
	private Vector3 myStartPos;
	private Animator myAnim;
	public UnityAdsExample myAds;
	private int noOfDiesForAds;
	public Button myShareButton;
	public Sprite Tick;
	public Sprite UnTick;

	public string AppLinkURL{ get; set; }


	void Awake ()
	{
		myMainMenuBtn = GetComponentInChildren<Button> ();
		myAnim = GetComponent<Animator> ();
		//FB.GetAppLink (DealWithAppLink);
	}

	void OnEnable ()
	{
		if (GameManager_HMMS.Instance.CapacityFull.activeInHierarchy)
		{
			GameManager_HMMS.Instance.CapacityFull.SetActive (false);
		}
		myStartPos = transform.position;
		if (myMainMenuBtn.gameObject.activeInHierarchy)
		{
			myMainMenuBtn.interactable = false;
			myMainMenuBtn.gameObject.SetActive (false);
		}
		if (!GameManager_HMMS.Instance.SoundOff)
		{
			iTween.AudioTo (GameManager_HMMS.Instance.gameObject, iTween.Hash ("audiosource", GameManager_HMMS.Instance.source_BGSound, "volume", 0f, "time", 2f));
			GameManager_HMMS.Instance.source_MainMenuBG.Play ();
		}
		GameManager_HMMS.Instance.StopCoroutine ("SpawnStarTimer");
		StartCoroutine ("OpenGameOverPopUp");

		noOfDiesForAds++;
		Console.Log (noOfDiesForAds);
		if (noOfDiesForAds == 1)
		{
			noOfDiesForAds = 0;
			myAds.ShowAd ();
		}

		GameManager_HMMS.Instance.shareOff = (PlayerPrefs.GetInt ("ShareSetting") != 0);
		if (!GameManager_HMMS.Instance.shareOff) //share with friends
		{
			myShareButton.GetComponent<Image> ().sprite = Tick;
		}
		else //do not share with friends
		{
			myShareButton.GetComponent<Image> ().sprite = UnTick;
		}


	}


	public void MainMenu ()
	{
		if (!GameManager_HMMS.Instance.SoundOff)
		{
			GameManager_HMMS.Instance.source_Tap.Play ();
		}
		if (!GameManager_HMMS.Instance.SoundOff)
		{
			//stop all BG sounds
			GameManager_HMMS.Instance.source_BGSound.Stop ();
			GameManager_HMMS.Instance.source_BGSound.volume = 0;
		}
		CloseGameOverPopUp ();
	}

	void CloseGameOverPopUp ()
	{
		myMainMenuBtn.interactable = false;
		if (!GameManager_HMMS.Instance.shareOff)
		{
			#if UNITY_ANDROID
			FB.FeedShare 
			(
				string.Empty, //toId
				new Uri (AppLinkURL), //link
				"Wordifly", //linkName
				"I love Wordifly", //linkCaption
				"Beat my Score " + GameManager_HMMS.Instance.WordCount.ToString (), //linkDescription
				new Uri ("http://imgur.com/xU0fzBR"), //picture
				string.Empty, //mediaSource
				ShareCallback 
			);
			#elif UNITY_IPHONE
			FB.FeedShare 
			(
			string.Empty, //toId
			new Uri (AppLinkURL), //link
			"Wordifly", //linkName
			"I love Wordifly", //linkCaption
			"Beat my Score "+ GameManager_HMMS.Instance.WordCount.ToString (), //linkDescription
			new Uri ("http://imgur.com/xU0fzBR"), //picture
			string.Empty, //mediaSource
			ShareCallback 
			);
			#endif
		}


		myMainMenuBtn.gameObject.SetActive (false);
		
		transform.position = myStartPos;
		
		GameManager_HMMS.Instance.MainMenu_Panel.gameObject.SetActive (true);
		this.gameObject.SetActive (false);
	}



	IEnumerator OpenGameOverPopUp ()
	{
		iTween.MoveTo (this.gameObject, iTween.Hash ("y", 0, "easetype", "linear", "time", .5f));
		//could add coroutine to slide out hud 
		//myAnim.Play ("GameOver_Fall");
		yield return new WaitForSeconds (0.6f);
		StartCoroutine ("setScores");
	}

	IEnumerator setScores ()
	{
		Analytics.CustomEvent ("gameOver", new Dictionary<string, object> {
			{ "WordCountBeforeDying", GameManager_HMMS.Instance.wordCount },
		});

		ScoreText_GO.text = GameManager_HMMS.Instance.WordCount.ToString ();
		//fetch HiScore
		GameManager_HMMS.Instance.HiWordCount = PlayerPrefs.GetInt ("HiWordCount", 0);
		HiScoreText_GO.text = GameManager_HMMS.Instance.HiWordCount.ToString ();
		myScoreAnim.Play ("ScoreBounce");
		yield return new WaitForSeconds (0.3f);

		//chk for hi-score
		if (GameManager_HMMS.Instance.WordCount > GameManager_HMMS.Instance.HiWordCount)
		{
			myHiScoreAnim.Play ("HiScoreGO_Bounce");
			GameManager_HMMS.Instance.HiWordCount = GameManager_HMMS.Instance.WordCount;
			PlayerPrefs.SetInt ("HiWordCount", GameManager_HMMS.Instance.HiWordCount);
			if (HiScoreText_GO != null)
				HiScoreText_GO.text = GameManager_HMMS.Instance.HiWordCount.ToString (); 
			ReportScore (GameManager_HMMS.Instance.HiWordCount, "CgkI8oqxqZ4JEAIQBw");
		}
		myMainMenuBtn.gameObject.SetActive (true);
		myMainMenuBtn.interactable = true;
	}

	public static void ReportScore (long score, string leaderboardID)
	{
		Social.ReportScore (score, leaderboardID, success =>
		{
		});
	}

	public void ShareOnFB ()
	{
		if (GameManager_HMMS.Instance.shareOff)
		{
			//make button tick
			Console.Log ("share with friends");
			GameManager_HMMS.Instance.shareOff = false;
			myShareButton.GetComponent<Image> ().sprite = Tick;
			PlayerPrefs.SetInt ("ShareSetting", (GameManager_HMMS.Instance.shareOff ? 1 : 0));
		}
		else
		{
			//make button untick
			Console.Log ("do not share with friends");
			GameManager_HMMS.Instance.shareOff = true;
			myShareButton.GetComponent<Image> ().sprite = UnTick;
			PlayerPrefs.SetInt ("ShareSetting", (GameManager_HMMS.Instance.shareOff ? 1 : 0));
		}
	}

	public void InviteFriends ()
	{
		FB.Mobile.AppInvite (
			new Uri (AppLinkURL), //link
			new Uri ("http://imgur.com/xU0fzBR"), //picture
			InviteCallback		
		);
	}

	void DealWithAppLink (IAppLinkResult result)
	{
		if (!String.IsNullOrEmpty (result.Url))
		{
			AppLinkURL = result.Url;
		}
	}

	private void ShareCallback (IResult result)
	{
		if (result.Cancelled)
		{
			Console.Log ("ShareCancelled");
		}
		else if (!string.IsNullOrEmpty (result.Error))
		{
			Console.Log ("Error on share");
		}
		else if (!string.IsNullOrEmpty (result.RawResult))
		{
			Console.Log ("SUCCESS on share");
		}
	}

	private void InviteCallback (IResult result)
	{
		if (result.Cancelled)
		{
			Console.Log ("InviteCancelled");
		}
		else if (!string.IsNullOrEmpty (result.Error))
		{
			Console.Log ("Error on Invite");
		}
		else if (!string.IsNullOrEmpty (result.RawResult))
		{
			Console.Log ("SUCCESS on Invite");
		}
	}
}

