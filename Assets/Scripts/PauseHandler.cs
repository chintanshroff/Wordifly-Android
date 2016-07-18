using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public delegate void ButtonEvent ();

public class PauseHandler : MonoBehaviour
{

	public static event ButtonEvent OnResume;

	public Button myResumeBtn;
	//private Text myResumeTxt;
	public Button myMainMenuBtn;
	//private Text myMainMenuTxt;
	private Animator myAnim;
	public Transform Tutorial;
	public Text MuteText;

	void OnEnable ()
	{
		if (!GameManager_HMMS.Instance.SoundOff) {
			GameManager_HMMS.Instance.source_BGSound.volume = 0.2f;
		}
		if (Tutorial.gameObject.activeInHierarchy) {
			Tutorial.gameObject.SetActive (false);
		}
		GameManager_HMMS.Instance.SoundOff = (PlayerPrefs.GetInt ("SoundSetting") != 0);
		if (!GameManager_HMMS.Instance.SoundOff) {
			MuteText.text = "MUTE";

		} else {
			MuteText.text = "UNMUTE";
		}

		GameManager_HMMS.Instance.StopCoroutine ("SpawnStarTimer");
		//myResumeTxt.color = new Color32 (255, 240, 0, 255);
		//myMainMenuTxt.color = new Color32 (255, 240, 0, 255);
		myResumeBtn.interactable = true;
		myMainMenuBtn.interactable = true;
		GameManager_HMMS.Instance.PauseButton.gameObject.SetActive (false);
		myAnim.Play ("PausePopUp");
	}

	void Awake ()
	{
		myAnim = GetComponent<Animator> ();
		//myResumeTxt = myResumeBtn.GetComponentInChildren<Text> ();
		//myMainMenuTxt = myMainMenuBtn.GetComponentInChildren<Text> ();
	}

	public void Resume ()
	{
		if (!GameManager_HMMS.Instance.SoundOff) {
			GameManager_HMMS.Instance.source_Tap.Play ();
		}
		GameManager_HMMS.Instance.source_BGSound.volume = 0.4f;
		//myResumeTxt.color = Color.white;
		//myResumeBtn.interactable = false;
		myMainMenuBtn.interactable = false;
		if (OnResume != null)
			OnResume ();
		this.gameObject.SetActive (false);

	}

	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Escape)) {
			if (GameManager_HMMS.Instance.CurrentState == GameState.Paused) {
				if (!GameManager_HMMS.Instance.SoundOff) {
					GameManager_HMMS.Instance.source_Tap.Play ();
				}
				GameManager_HMMS.Instance.source_BGSound.volume = 0.4f;
				myMainMenuBtn.interactable = false;
				if (OnResume != null)
					OnResume ();
				this.gameObject.SetActive (false);
			}
		}
	}


	public void MainMenu ()
	{
		if (!GameManager_HMMS.Instance.SoundOff) {
			GameManager_HMMS.Instance.source_Tap.Play ();
		}
		if (!GameManager_HMMS.Instance.SoundOff) {
			//stop all BG sounds
			iTween.AudioTo (GameManager_HMMS.Instance.gameObject, iTween.Hash ("audiosource", GameManager_HMMS.Instance.source_BGSound, "volume", 0f, "time", 1f));
			GameManager_HMMS.Instance.source_BGSound.Stop ();
		}
		//myMainMenuTxt.color = Color.white;
		myResumeBtn.interactable = false;
		myMainMenuBtn.interactable = false;
		GameManager_HMMS.Instance.MainMenu_Panel.gameObject.SetActive (true);
		this.gameObject.SetActive (false);
		
	}

	public void HelpButton ()
	{
		if (!Tutorial.gameObject.activeInHierarchy) {
			Tutorial.gameObject.SetActive (true);
		}
	}


	public void Mute ()
	{
		if (GameManager_HMMS.Instance.SoundOff) {
			GameManager_HMMS.Instance.source_Tap.Play ();
		}
		if (!GameManager_HMMS.Instance.SoundOff) {
			GameManager_HMMS.Instance.SoundOff = true;
			MuteText.text = "UNMUTE";
			GameManager_HMMS.Instance.source_BGSound.Pause ();
			PlayerPrefs.SetInt ("SoundSetting", (GameManager_HMMS.Instance.SoundOff ? 1 : 0));
			Console.Log (PlayerPrefs.GetInt ("SoundSetting"));
		} else {
			GameManager_HMMS.Instance.SoundOff = false;
			MuteText.text = "MUTE";
			GameManager_HMMS.Instance.source_BGSound.Play ();
			PlayerPrefs.SetInt ("SoundSetting", (GameManager_HMMS.Instance.SoundOff ? 1 : 0));
			Console.Log (PlayerPrefs.GetInt ("SoundSetting"));


		}
	}


}
