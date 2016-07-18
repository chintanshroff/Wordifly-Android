using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public delegate void ClickEvent ();

public class MainMenuHandler : MonoBehaviour
{
	public static event ClickEvent OnPlay;
	public static event ClickEvent OnMainMenu;

	public  Button myPlayBtn;
	public  Button myCreditsBtn;
	public  Button mySoundBtn;
	public Sprite SoundOnSprite;
	public Sprite SoundOffSprite;
	public TextMesh MissionsTest;
	private int MissionCount;
	private bool playClicked;

	void OnEnable ()
	{
		GameManager_HMMS.Instance.SoundOff = (PlayerPrefs.GetInt ("SoundSetting") != 0);
		if (!GameManager_HMMS.Instance.SoundOff)
		{
			mySoundBtn.GetComponent<Image> ().sprite = SoundOnSprite;
			if (!GameManager_HMMS.Instance.source_MainMenuBG.isPlaying)
			{
				GameManager_HMMS.Instance.source_MainMenuBG.Play ();
				iTween.AudioTo (GameManager_HMMS.Instance.gameObject, iTween.Hash ("audiosource", GameManager_HMMS.Instance.source_MainMenuBG, "volume", 0.01f, "time", 2f));
			}
		}
		else
		{
			mySoundBtn.GetComponent<Image> ().sprite = SoundOffSprite;
		}
		if (OnMainMenu != null)
			OnMainMenu ();
		StartCoroutine ("ActivateButtons");
	}

	public IEnumerator ActivateButtons ()
	{
		yield return new WaitForSeconds (1f);
		myPlayBtn.interactable = true;
		myCreditsBtn.interactable = true;
		mySoundBtn.interactable = true;
	}

	public void Play ()
	{
		if (!playClicked)
		{
			playClicked = true;
			myPlayBtn.interactable = false;
			myCreditsBtn.interactable = false;
			mySoundBtn.interactable = false;
			if (!GameManager_HMMS.Instance.SoundOff)
			{
				GameManager_HMMS.Instance.source_Tap.Play ();
			}
			if (OnPlay != null)
				OnPlay ();
			StartCoroutine (closePanel (1f));
		}
	}

	public void Credits ()
	{
		mySoundBtn.interactable = false;
		myCreditsBtn.interactable = false;
		myPlayBtn.interactable = false;
		if (!GameManager_HMMS.Instance.SoundOff)
		{
			GameManager_HMMS.Instance.source_Tap.Play ();
		}
		GameManager_HMMS.Instance.Credit_Panel.gameObject.SetActive (true); 
		//this.gameObject.SetActive (false);
	}

	public void Sound ()
	{
		if (GameManager_HMMS.Instance.SoundOff)
		{
			GameManager_HMMS.Instance.source_Tap.Play ();
		}
		if (!GameManager_HMMS.Instance.SoundOff)
		{
			GameManager_HMMS.Instance.SoundOff = true;
			mySoundBtn.GetComponent<Image> ().sprite = SoundOffSprite;
			GameManager_HMMS.Instance.source_MainMenuBG.mute = true;
			PlayerPrefs.SetInt ("SoundSetting", (GameManager_HMMS.Instance.SoundOff ? 1 : 0));
		}
		else
		{
			GameManager_HMMS.Instance.SoundOff = false;
			mySoundBtn.GetComponent<Image> ().sprite = SoundOnSprite;
			GameManager_HMMS.Instance.source_MainMenuBG.mute = false;
			PlayerPrefs.SetInt ("SoundSetting", (GameManager_HMMS.Instance.SoundOff ? 1 : 0));
			
		}
	}

	IEnumerator closePanel (float x)
	{
		yield return new WaitForSeconds (x);
		playClicked = false;
		GameManager_HMMS.Instance.MainMenu_Panel.gameObject.SetActive (false);
	}
}
