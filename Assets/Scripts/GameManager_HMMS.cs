using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;
using UnityEngine.SocialPlatforms.GameCenter;


public delegate void GameOverEvent ();

public enum GameState
{
	MainMenu,
	Loading,
	GetReady,
	Playing,
	SaveMe,
	GameOver,
	Paused}
;

public class GameManager_HMMS : MonoBehaviour
{
	//Static Singleton Instance
	public static GameManager_HMMS _Instance = null;
	
	//property to get instance
	public static GameManager_HMMS Instance {
		get {
			//if we do not have Instance yet
			if (_Instance == null)
			{                                                                                   
				_Instance = (GameManager_HMMS)FindObjectOfType (typeof(GameManager_HMMS));
			}
			return _Instance;
		}	
	}


	public static event GameOverEvent GameOverShown;

	public Transform FireflyKaBaap;
	public Transform FireFly;
	public GameState CurrentState;
	public Color32 StartColor;
	private Transform myPooledStar;
	private Star star_Obj;
	public  float MaxStarSpeed;
	public float starSpeed;

	//Letters
	public List<string> VowelList;
	public List<string> ConsonantList;
	public List<Star> PickedLetterList = new List<Star> ();
	//the letter picked by the player

	public int WordCount;
	public int HiWordCount;
	public float tailScore;
	// count that determines the time until which the tail doesnt depelete
	
	private float starTimer;

	//add to trail
	public bool WordMade;
	public ParticleSystem Trail_ps;

	//UI pop-ups
	public RectTransform Credit_Panel;
	public RectTransform GameOver_Panel;
	public RectTransform MainMenu_Panel;
	public RectTransform HUD_Panel;
	public RectTransform Pause_Panel;
	public Button PauseButton;


	public Button RateUsBtn;


	//UI
	public Text HIWordCount_Text;
	public Text WordCount_Text;
	private wordCheck wordCheckScript;
	public List<Transform> emptyStarHUDList = new List<Transform> ();

	//configurables
	public float TailScorePower;
	public float startLifetimeReducer;

	//Sounds
	public bool SoundOff;
	public AudioSource source_BGSound;
	public AudioSource source_CorrectWord;
	public AudioSource source_Drop;
	public AudioSource source_Pick;
	public AudioSource source_Tap;
	public AudioSource source_AchivementComplete;
	public AudioSource source_MainMenuBG;
	public AudioSource source_WordReady;
	public AudioSource source_Dead;
	public AudioSource source_Play;


	public Transform AchievementNotification;
	public Color startColor;
	public Transform CompleteText;



	public GameObject StarPool;
	public GameObject YelStarCollectPool;
	public GameObject TapFXPool;

	public Vector3 screenSizeInWord;

	//tutorial
	public GameObject TiltImage;
	public GameObject TapImage;
	public GameObject FireHelp;
	public int starCollectedForTut;
	public GameObject CapacityFull;
	public bool notFirstTime;

	public GameObject BeeShield;
	public GameObject HurryText;

	public bool firstTimeAchievementLoad;

	//analytics
	public int wordCount;

	//share
	public bool shareOff;


	[SerializeField]
	private bool
		cleanData = false;

	void Awake ()
	{
		MainMenuHandler.OnPlay += OnGameStart;
		MainMenuHandler.OnMainMenu += OnQuit;
		PauseHandler.OnResume += OnResumeBtnClicked;
		BeeHolder.OnGameOver += OnGameOverHandler;
		Bee_HMMS.StartTurorial += OnStartTutorial;
		wordCheckScript = GetComponent<wordCheck> ();

	}

	// Use this for initialization
	void Start ()
	{
		
		AuthenticateToGooglePlay ();
		AuthenticateToGameCenter ();

		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		screenSizeInWord = Camera.main.ScreenToWorldPoint (new Vector3 (Screen.width, Screen.height, 0));

		HIWordCount_Text.transform.position = new Vector3 (-screenSizeInWord.x + 1f, -screenSizeInWord.y + 0.5f, 0);
		WordCount_Text.transform.position = new Vector3 (-screenSizeInWord.x + 1f, -screenSizeInWord.y + 1.3f, 0);
		PauseButton.transform.position = new Vector3 (screenSizeInWord.x - .7f, screenSizeInWord.y - 1.15f, 0);
		//hiscore
		HiWordCount = PlayerPrefs.GetInt ("HiWordCount", 0);
		if (HiWordCount == 0)
		{
			HIWordCount_Text.gameObject.SetActive (false);
		}
		else
		{
			HIWordCount_Text.gameObject.SetActive (true);
			HIWordCount_Text.text = "HiScore: " + HiWordCount.ToString ();
		}

		//WordCount_Text.transform.position = new Vector3 (screenSizeInWord.x - .7f, screenSizeInWord.y - 1.15f, 0);
		//PauseButton.transform.position = new Vector3 (screenSizeInWord.x - .7f, -screenSizeInWord.y + .75f, 0);
		RateUsBtn.transform.position = new Vector3 (-screenSizeInWord.x + 1f, screenSizeInWord.y - 0.7f, 0);

		//cleanData = (PlayerPrefs.GetInt ("CleanData") != 0);
		if (cleanData)
		{
			PlayerData.Instance.CleanData (cleanData);
			PlayerPrefs.SetInt ("FirstTime", (GameManager_HMMS.Instance.notFirstTime ? 0 : 0));
			cleanData = false;
			PlayerData.Instance.SaveKey ("CleanData", (cleanData ? 0 : 0));
			//PlayerPrefs.SetInt ("CleanData",(cleanData ? 0 : 0) );
			
		}
		notFirstTime = (PlayerPrefs.GetInt ("FirstTime") != 0);

		
		CurrentState = GameState.MainMenu;

		//ui panels
		MainMenu_Panel.gameObject.SetActive (true);
		Pause_Panel.gameObject.SetActive (false);
		HUD_Panel.gameObject.SetActive (false);
		GameOver_Panel.gameObject.SetActive (false);

		//variables
		WordCount = 0;
		starTimer = 0.7f;
		Trail_ps.startLifetime = 1.5f;

		starSpeed = MaxStarSpeed;

		//scores
		if (WordCount_Text != null)
			WordCount_Text.text = "Score: " + WordCount.ToString ("000"); 
	}


	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Escape))
		{
			if (CurrentState == GameState.Playing)
			{
				if (notFirstTime)
				{
					if (!SoundOff)
					{
						source_Tap.Play ();
					}
					CurrentState = GameState.Paused;
					Pause_Panel.gameObject.SetActive (true);
				}
			}
			else if (CurrentState == GameState.MainMenu)
			{
				Application.Quit (); 
			} 
		}
	}

	public IEnumerator SpawnStarTimer ()
	{
		while (CurrentState == GameState.Playing)
		{
			yield return new WaitForSeconds (starTimer);
			SpawnStar ();
		}
	}

	public void SpawnStar ()
	{
		myPooledStar = GameObjectPool.GetPool ("Star_Pool").GetInstance ();
		star_Obj = myPooledStar.GetComponent<Star> ();
		star_Obj.transform.position = new Vector3 (Random.Range (-GameManager_HMMS.Instance.screenSizeInWord.x + 0.5f, GameManager_HMMS.Instance.screenSizeInWord.x - 0.5f), 5.5f, 3f);
		star_Obj.transform.rotation = Quaternion.identity;
		star_Obj.picked = false;
	}


	public void ResetGameParam ()
	{
		firstTimeAchievementLoad = true;

		if (GameOverShown != null)
		{
			GameOverShown ();
		}

		if (HurryText.activeInHierarchy)
		{
			HurryText.SetActive (false);
		}
		if (CapacityFull.activeInHierarchy)
		{
			CapacityFull.SetActive (false);
		}
		HiWordCount = PlayerPrefs.GetInt ("HiWordCount", 0);
		if (HiWordCount > 0)
		{
			HIWordCount_Text.gameObject.SetActive (true);
			HIWordCount_Text.text = "HiScore: " + HiWordCount.ToString ();
		}
	
		wordCheckScript.WordToCheck = null;
		PickedLetterList = new List<Star> ();
		if (!FireFly.GetComponent<CircleCollider2D> ().isActiveAndEnabled)
			FireFly.GetComponent<CircleCollider2D> ().enabled = true;
		WordCount = 0;
		if (WordCount_Text != null)
			WordCount_Text.text = "Score: " + WordCount.ToString ("000"); 
		starTimer = 0.7f;
		FireflyKaBaap.gameObject.SetActive (true);
		FireflyKaBaap.position = new Vector3 (0f, -1.9f, -1f);
		FireflyKaBaap.rotation = Quaternion.Euler (0, 0, 0);
		FireFly.gameObject.SetActive (true);
		if (notFirstTime)
		{
			PauseButton.gameObject.SetActive (true);

		}
		else
		{
			PauseButton.gameObject.SetActive (false);
		}
		
			
		Trail_ps.startLifetime = 1.5f;
		Trail_ps.Play ();
		Trail_ps.startColor = StartColor;
		BeeShield.SetActive (true);
		WordMade = false;
		starSpeed = MaxStarSpeed;
	}


	void OnGameOverHandler ()
	{
		
		if (!GameOver_Panel.gameObject.activeInHierarchy)
		{
			GameOver_Panel.gameObject.SetActive (true);
		}
	}

	void OnGameStart ()
	{
		
		wordCount = 0;
		VowelList = new List<string> (new string[]{ "A", "E", "I", "O", "U" });
		ConsonantList = new List<string> (new string[] {
			"B",
			"C",
			"D",
			"F",
			"G",
			"H",
			"J",
			"K",
			"L",
			"M",
			"N",
			"P",
			"Q",
			"R",
			"S",
			"T",
			"V",
			"W",
			"X",
			"Y",
			"Z"
		});
		if (!SoundOff)
		{
			source_BGSound.Play ();
			iTween.AudioTo (this.gameObject, iTween.Hash ("audiosource", source_BGSound, "volume", 0.4f, "time", 10f));
			source_MainMenuBG.Stop ();
		}
		StartCoroutine (ShowHUD (1.75f));
	}


	IEnumerator ShowHUD (float x)
	{
		yield return new WaitForSeconds (x);
		if (notFirstTime)
		{
			if (!PauseButton.gameObject.activeInHierarchy)
				PauseButton.gameObject.SetActive (true);

		}
		else
		{
			if (PauseButton.gameObject.activeInHierarchy)
				PauseButton.gameObject.SetActive (false);

		}

		if (!HUD_Panel.gameObject.activeInHierarchy)
			HUD_Panel.gameObject.SetActive (true);
		AchievementNotification.gameObject.SetActive (true);
	}

	void OnQuit ()
	{
		CurrentState = GameState.MainMenu;
		ResetGameParam ();

		if (HUD_Panel.gameObject.activeInHierarchy)
			HUD_Panel.gameObject.SetActive (false);
		AchievementNotification.gameObject.SetActive (false);
		if (CompleteText.gameObject.activeInHierarchy)
		{
			CompleteText.gameObject.SetActive (false);
		}
	}

	void OnResumeBtnClicked ()
	{
		CurrentState = GameState.Playing;
		if (!SoundOff)
		{
			iTween.AudioTo (this.gameObject, iTween.Hash ("audiosource", source_BGSound, "volume", 0.4f, "time", 10f));
		}
		StartCoroutine ("SpawnStarTimer");
		if (!PauseButton.gameObject.activeInHierarchy)
		{
			PauseButton.gameObject.SetActive (true);
		}
		if (!HUD_Panel.gameObject.activeInHierarchy)
			HUD_Panel.gameObject.SetActive (true);
	}

	public void IncreaseStarSpeed ()
	{
		/*//difficulty 1
		if (starSpeed <= 3)
			starSpeed += 0.03f;
		else if (starSpeed > 3 && starSpeed <= 3.5f)
			starSpeed += 0.02f;
		if (starTimer >= 0.6f)
			starTimer += 0.001f;*/

		//difficulty2
		if (starSpeed <= 3)
			starSpeed += 0.04f;
		else if (starSpeed > 3 && starSpeed <= 3.5f)
			starSpeed += 0.02f;
		if (starTimer > 0.3f)
			starTimer -= 0.01f;

	}

	void OnStartTutorial ()
	{
		if (!notFirstTime)
		{
			if (PauseButton.gameObject.activeInHierarchy)
				PauseButton.gameObject.SetActive (false);
		}
		Console.Log ("TutorialStarted");
		SpawnStar ();
		TiltImage.SetActive (true);

	}

	public void ShowLeaderBoard ()
	{
		#if UNITY_ANDROID
		PlayGamesPlatform.Instance.ShowLeaderboardUI ("CgkI8oqxqZ4JEAIQBw");
		#elif UNITY_IPHONE
		GameCenterPlatform.ShowLeaderboardUI ("CgkI8oqxqZ4JEAIQBw", TimeScope.AllTime);
		#endif

	}


	public void RateUs ()
	{
		#if UNITY_ANDROID
		Application.OpenURL ("https://play.google.com/store/apps/details?id=com.TheGoodSideGames.Wordifly");
		#elif UNITY_IPHONE
		Application.OpenURL ("https://itunes.apple.com/us/app/wordifly-the-word-game/id1083876718?ls=1&mt=8");
		#endif
	}

	public static void  AuthenticateToGameCenter ()
	{
		#if UNITY_IPHONE
		Social.localUser.Authenticate (success => {
			if (success) {
				Console.Log ("Authentication successful");
			} else {
				Console.Log ("Authentication failed");
			}
		});
		#endif
	}

	public static void AuthenticateToGooglePlay ()
	{
		#if UNITY_ANDROID
		PlayGamesPlatform.Activate ();
		// authenticate user:
		Social.localUser.Authenticate ((bool success) =>
		{
			if (success)
			{
				Console.Log ("login success");
			}
			else
			{
				Console.Log ("login failed");
			}
		});
		#endif
	}




	void OnDestroy ()
	{
		MainMenuHandler.OnPlay -= OnGameStart;
		MainMenuHandler.OnMainMenu -= OnQuit;
		PauseHandler.OnResume -= OnResumeBtnClicked;
		BeeHolder.OnGameOver -= OnGameOverHandler;
		Bee_HMMS.StartTurorial -= OnStartTutorial;
	}


}
